using Cerberus.Dto;
using Cerberus.Services.Esi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Services.Assets
{
    public class AssetRetrievalService(EsiClient esiClient) 
    {
        public async Task<EsiAsset[]> GetAssets(long characterId, string accessToken)
        {
            return await esiClient.GetCharacterAssetsAsync(characterId, accessToken);
        }
    }
}
