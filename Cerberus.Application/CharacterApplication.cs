using Cerberus.Application.Assets;
using Cerberus.Dto;
using Cerberus.Services.Data;

namespace Cerberus.Application
{
    public class CharacterApplication(CharacterRepository characterRepository, AssetRetrievalApplication assetRetrievalApplication, WalletApplication walletApplication)
    {
        public async Task<CharacterDto> LoadCharacter(long id, string accessToken)
        {
            var character = characterRepository.GetById(id);

            // Only update info once an hour
            //if (DateTime.UtcNow.AddHours(-1) > character.LastUpdated)
            //{
                character.Assets = await assetRetrievalApplication.GetAssets(id, accessToken);

                var walletTransactions = await walletApplication.GetTransactions(id, accessToken);
                ReconcileWallet(character, walletTransactions);

                GroupTransactionsByItem(character);

                AddItemNamesToTransactionGroups(character);

                UpdateTotalAssetQuantities(character);

                await UpdateTotalTrackedAssetValue(character, accessToken);
            //}

            characterRepository.Save(character);

            return character;
        }

        private async Task UpdateTotalTrackedAssetValue(CharacterDto character, string accessToken)
        {
            foreach (var transactionGroup in character.TransactionGroups)
            {
                var order = await assetRetrievalApplication.GetMarketOrderDtosAsync(accessToken, transactionGroup.Key.ToString());

                if (order != null)
                {
                    long multiplierToUse = transactionGroup.Value.TotalTrackedQuantity;
                    if (transactionGroup.Value.TotalQuantity < transactionGroup.Value.TotalTrackedQuantity)
                    {
                        multiplierToUse = transactionGroup.Value.TotalQuantity;
                    }

                    transactionGroup.Value.TotalAssetValue = (order.Price * multiplierToUse);
                }
            }
        }

        private void AddItemNamesToTransactionGroups(CharacterDto character)
        {
            var assetNamesDict = ParseCsvToDictionary();
            foreach (var transactionGroup in character.TransactionGroups)
            {
                transactionGroup.Value.ItemName = assetNamesDict[transactionGroup.Key];
            }

            foreach (var walletTransaction in character.WalletTransactions)
            {
                walletTransaction.Value.ItemName = assetNamesDict[walletTransaction.Value.TypeId];
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
        private void ReconcileWallet(CharacterDto character, List<EsiWalletTransaction> walletTransactions)
        {
            foreach (var transaction in walletTransactions)
            {
                if (!character.WalletTransactions.ContainsKey(transaction.TransactionId))
                {
                    character.WalletTransactions.Add(transaction.TransactionId, transaction);
                }
            }
        }

        /// <summary>
        /// Group all of our transactions by item time and calculate the tracked value.
        /// </summary>
        /// <param name="character"></param>
        private void GroupTransactionsByItem(CharacterDto character)
        {
            character.TransactionGroups = new Dictionary<long, TransactionGroup>();

            foreach (var transaction in character.WalletTransactions)
            {
                var typeId = transaction.Value.TypeId;

                // We only want to track our buy
                // This _should_ cover buying from sell orders and putting in buy orders
                if (transaction.Value.IsBuy)
                {
                    
                    if (character.TransactionGroups.ContainsKey(typeId))
                    {
                        character.TransactionGroups[typeId].TotalTrackedQuantity += transaction.Value.Quantity;
                        character.TransactionGroups[typeId].TotalTrackedAssetPrice += (transaction.Value.UnitPrice * transaction.Value.Quantity);
                        character.TransactionGroups[typeId].AverageTrackedPrice = (character.TransactionGroups[typeId].TotalTrackedAssetPrice / character.TransactionGroups[typeId].TotalTrackedQuantity);
                    }
                    else
                    {
                        character.TransactionGroups.Add(typeId, new TransactionGroup()
                        {
                            TotalTrackedQuantity = transaction.Value.Quantity,
                            TotalTrackedAssetPrice = transaction.Value.UnitPrice * transaction.Value.Quantity,
                            AverageTrackedPrice = (transaction.Value.UnitPrice * transaction.Value.Quantity) / transaction.Value.Quantity
                        });
                    }
                }
                else
                {
                    // Is sell?
                    if (character.TransactionGroups.ContainsKey(typeId))
                    {

                    }
                }
            }
        }

        private void UpdateTotalAssetQuantities(CharacterDto character)
        {
            foreach (var asset in character.Assets)
            {
                if (character.TransactionGroups.ContainsKey(asset.TypeId))
                {
                    character.TransactionGroups[asset.TypeId].TotalQuantity += asset.Quantity;
                }
            }


            var itemsGreaterThan0 = new Dictionary<long, TransactionGroup>();
            foreach (var transactionGroup in character.TransactionGroups)
            {
                if (transactionGroup.Value.TotalQuantity > 0)
                {
                    itemsGreaterThan0.Add(transactionGroup.Key, transactionGroup.Value);
                }
            }

            character.TransactionGroups = itemsGreaterThan0;
        }
    }
}
