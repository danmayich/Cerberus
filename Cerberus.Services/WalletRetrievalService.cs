using Cerberus.Dto;
using Cerberus.Services.Esi;


namespace Cerberus.Services
{
    public class WalletRetrievalService(EsiClient esiClient)
    {
        public async Task<List<EsiWalletTransaction>> GetTransactions(long characterId, string accessToken)
        {
           return await esiClient.GetWalletTransactionsAsync(characterId, accessToken);
        }

        public async Task<WalletJournalEntry[]> GetWalleyJournalEntries(long characterId, string accessToken)
        {
            return await esiClient.GetCharacterWalletJournal(characterId, accessToken);
        }
    }
}
