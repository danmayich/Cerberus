using Cerberus.Dto;
using Cerberus.Services.Esi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Services
{
    public class WalletRetrievalService(EsiClient esiClient)
    {
        public async Task<List<EsiWalletTransaction>> GetTransactions(long characterId, string accessToken)
        {
           return await esiClient.GetWalletTransactionsAsync(characterId, accessToken);
        }
    }
}
