using Cerberus.Dto;
using Cerberus.Services.Assets;
using Cerberus.Services.Esi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Application.Assets
{
    public class AssetRetrievalApplication(AssetRetrievalService assetRetreivalService)
    {
        public async Task<EsiAsset[]> GetAssets(long characterId, string accessToken)
        {
            return await assetRetreivalService.GetAssets(characterId, accessToken);
        }
    }
}
