using Cerberus.Application.Assets;
using Cerberus.Dto;
using Cerberus.Services.Data;
using Cerberus.Services.Esi;

namespace Cerberus.Application
{
    public class CharacterApplication(CharacterRepository characterRepository, AssetRetrievalApplication assetRetrievalApplication, WalletApplication walletApplication, EsiClient esiClient)
    {
        private const decimal TransactionGroupTaxRate = 0.03375m;

        public async Task<Dictionary<long, TransactionGroup>> TrackPosition(long characterId, EsiWalletTransaction transaction, string accessToken)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            var character = characterRepository.GetById(characterId);
            character.TrackedPositions ??= new Dictionary<long, EsiWalletTransaction>();
            character.TrackedPositions[transaction.TransactionId] = transaction;

            GroupTrackedTransactionsByType(character);
            AddItemNamesToTransactionGroups(character);
            await UpdateTrackedGroupMarketValues(character, accessToken);

            characterRepository.Save(character);
            return character.TransactionGroups;
        }

        public async Task<Dictionary<long, TransactionGroup>> UntrackPosition(long characterId, long transactionId, string accessToken)
        {
            var character = characterRepository.GetById(characterId);
            if (character.TrackedPositions is null)
            {
                return character.TransactionGroups ?? new Dictionary<long, TransactionGroup>();
            }

            character.TrackedPositions.Remove(transactionId);

            GroupTrackedTransactionsByType(character);
            AddItemNamesToTransactionGroups(character);
            await UpdateTrackedGroupMarketValues(character, accessToken);

            characterRepository.Save(character);
            return character.TransactionGroups;
        }

        public async Task<CharacterDto> LoadCharacter(long id, string accessToken)
        {
            var character = characterRepository.GetById(id);

            // Only update info once an hour
            //if (DateTime.UtcNow.AddHours(-1) > character.LastUpdated)
            //{
                // fetch and attach character info
                try
                {
                    var esiCharacter = await esiClient.GetCharacterAsync(id, accessToken);
                    character.CharacterInfo = esiCharacter;
                }
                catch
                {
                    // ignore character-info failures and continue updating other data
                }

                character.Assets = await assetRetrievalApplication.GetAssets(id, accessToken);

                var lookup = new Dictionary<long, string>();

                // TODO - Items in eve can be multiple stacks of 1 at a specific location, the api is returning all the mutliple stacks, update it to just combine them 
                // into a single entry

                // TODO - location look up
                foreach (var line in File.ReadLines($"C:\\test\\esi\\typeids.csv"))
                {
                    var parts = line.Split(',');

                    var itemTypeId = long.Parse(parts[0].Trim('"'));
                    var name = parts[1].Trim('"');

                    lookup[itemTypeId] = name;
                }

                foreach (var asset in character.Assets)
                {
                    if (lookup.TryGetValue(asset.TypeId, out var name))
                    {
                        asset.ItemName = name;
                    }
                }

                var walletTransactions = await walletApplication.GetTransactions(id, accessToken);
                ReconcileWallet(character, walletTransactions);

                var walletJournalEntries = await walletApplication.GetWalleyJournalEntries(id, accessToken);
                ReconcileWalletJourneyTransactions(character, walletJournalEntries.ToList());

            //}

            GroupTrackedTransactionsByType(character);

            AddItemNamesToTransactionGroups(character);

            await UpdateTrackedGroupMarketValues(character, accessToken);

            characterRepository.Save(character);

            return character;
        }

        private async Task UpdateTrackedGroupMarketValues(CharacterDto character, string accessToken)
        {
            foreach (var transactionGroup in character.TransactionGroups)
            {
                var group = transactionGroup.Value;
                var latestBuyOrder = await assetRetrievalApplication.GetMarketOrderDtosAsync(accessToken, transactionGroup.Key.ToString());

                if (latestBuyOrder is null)
                {
                    group.TotalAssetValue = 0;
                    group.TotalTax = 0;
                    group.TotalProfit = -group.TotalTrackedAssetPrice;
                    continue;
                }

                group.TotalAssetValue = latestBuyOrder.Price * group.TotalTrackedQuantity;
                group.TotalTax = group.TotalAssetValue * TransactionGroupTaxRate;
                group.TotalProfit = group.TotalAssetValue - group.TotalTrackedAssetPrice - group.TotalTax;
            }
        }

        private void AddItemNamesToTransactionGroups(CharacterDto character)
        {
            var assetNamesDict = ParseCsvToDictionary();
            try
            {
                foreach (var transactionGroup in character.TransactionGroups)
                {
                    if (assetNamesDict.TryGetValue(transactionGroup.Key, out var itemName))
                    {
                        transactionGroup.Value.ItemName = itemName;
                    }
                }

                foreach (var walletTransaction in character.WalletTransactions)
                {
                    if (assetNamesDict.TryGetValue(walletTransaction.Value.TypeId, out var itemName))
                    {
                        walletTransaction.Value.ItemName = itemName;
                    }
                }

                foreach (var trackedTransaction in character.TrackedPositions)
                {
                    if (assetNamesDict.TryGetValue(trackedTransaction.Value.TypeId, out var itemName))
                    {
                        trackedTransaction.Value.ItemName = itemName;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO what causes this
            }

        }

        /// <summary>
        /// This is ass
        /// </summary>
        public Dictionary<long, string> ParseCsvToDictionary()
        {
            var result = new Dictionary<long, string>();

            foreach (var line in File.ReadLines(@"C:\test\esi\typeids.csv"))
            {
                // Remove surrounding quotes and split by comma
                var parts = line.Split(',').Select(p => p.Trim('"')).ToArray();

                if (parts.Length >= 2 && long.TryParse(parts[0], out long key))
                {
                    result[key] = parts[1];
                }
            }

            return result;
        }


        /// <summary>
        ///  TODO should be in domain logic
        /// </summary>
        private void ReconcileWallet(CharacterDto character, List<EsiWalletTransaction> latestWalletTransactions)
        {
            // Order the new transactions by date desc
            latestWalletTransactions = latestWalletTransactions.OrderByDescending(order => order.Date).ToList();

            foreach (var transaction in latestWalletTransactions)
            {
                if (!character.WalletTransactions.ContainsKey(transaction.TransactionId))
                {
                    character.WalletTransactions.Add(transaction.TransactionId, transaction);
                }
            }
        }

        /// <summary>
        ///  TODO should be in domain logic
        /// </summary>
        private void ReconcileWalletJourneyTransactions(CharacterDto character, List<WalletJournalEntry> latestWalletTransactions)
        {
            // Order the new transactions by date desc
            latestWalletTransactions = latestWalletTransactions.OrderByDescending(order => order.Date).ToList();

            foreach (var transaction in latestWalletTransactions)
            {
                if (!character.WalletJournalEntries.ContainsKey(transaction.Id))
                {
                    character.WalletJournalEntries.Add(transaction.Id, transaction);
                }
            }
        }


        /// <summary>
        /// Group tracked buy transactions by type id and calculate their aggregate cost basis.
        /// </summary>
        /// <param name="character"></param>
        private void GroupTrackedTransactionsByType(CharacterDto character)
        {
            character.TransactionGroups = new Dictionary<long, TransactionGroup>();

            foreach (var transaction in character.TrackedPositions)
            {
                var typeId = transaction.Value.TypeId;

                // We only want to aggregate buy-side tracked transactions for cost basis.
                if (!transaction.Value.IsBuy || transaction.Value.Quantity <= 0)
                {
                    continue;
                }

                var totalCost = transaction.Value.UnitPrice * transaction.Value.Quantity;

                if (character.TransactionGroups.TryGetValue(typeId, out var existingGroup))
                {
                    existingGroup.TotalTrackedQuantity += transaction.Value.Quantity;
                    existingGroup.TotalTrackedAssetPrice += totalCost;

                    if (existingGroup.TotalTrackedQuantity > 0)
                    {
                        existingGroup.AverageTrackedPrice = existingGroup.TotalTrackedAssetPrice / existingGroup.TotalTrackedQuantity;
                    }

                    continue;
                }

                character.TransactionGroups.Add(typeId, new TransactionGroup()
                {
                    TotalTrackedQuantity = transaction.Value.Quantity,
                    TotalTrackedAssetPrice = totalCost,
                    AverageTrackedPrice = totalCost / transaction.Value.Quantity
                });
            }
        }
    }
}
