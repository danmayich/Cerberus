using Cerberus.Application.Assets;
using Cerberus.Dto;
using Cerberus.Services.Data;
using System.Data;

namespace Cerberus.Application
{
    public class CharacterApplication(CharacterRepository characterRepository, AssetRetrievalApplication assetRetrievalApplication, WalletApplication walletApplication)
    {
        public async Task<CharacterDto> LoadCharacter(long id, string accessToken)
        {
            var character = characterRepository.GetById(id);

            // Only update info once an hour
            if (DateTime.UtcNow.AddHours(-1) > character.LastUpdated)
            {
                character.Assets = await assetRetrievalApplication.GetAssets(id, accessToken);

                
                var walletTransactions = await walletApplication.GetTransactions(id, accessToken);
                ReconcileWallet(character, walletTransactions);

                GroupTransactionsByItem(character);

                UpdateTotalAssetQuantities(character);
            }

            characterRepository.Save(character);

            return character;
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
                        AverageTrackedPrice = transaction.Value.UnitPrice / transaction.Value.Quantity
                    });
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
        }
    }
}
