using Cerberus.Dto;
using Cerberus.Services;

namespace Cerberus.Application
{
    public class WalletApplication(WalletRetrievalService walletRetrievalService)
    {
        public async Task<List<EsiWalletTransaction>> GetTransactions(long characterId, string accessToken)
        {
            return await walletRetrievalService.GetTransactions(characterId, accessToken);
        }

        public async Task<WalletJournalEntry[]> GetWalleyJournalEntries(long characterId, string accessToken)
        {
            return await walletRetrievalService.GetWalleyJournalEntries(characterId, accessToken);
        }
    }
}
