using Cerberus.Dto;
using Cerberus.Services;
using Cerberus.Services.Esi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Application
{
    public class WalletApplication(WalletRetrievalService walletRetrievalService)
    {
        public async Task<List<EsiWalletTransaction>> GetTransactions(long characterId, string accessToken)
        {
            return await walletRetrievalService.GetTransactions(characterId, accessToken);
        }
    }
}
