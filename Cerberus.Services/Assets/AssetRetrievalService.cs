using Cerberus.Dto;
using Cerberus.Services.Esi;

namespace Cerberus.Services.Assets
{
    public class AssetRetrievalService(EsiClient esiClient) 
    {
        public async Task<EsiAsset[]> GetAssets(long characterId, string accessToken)
        {
            return await esiClient.GetCharacterAssetsAsync(characterId, accessToken);
        }

        public async Task<MarketOrderDto[]> GetMarketOrderDtosAsync(string accessToken, string itemTypeId)
        {
            return await esiClient.GetMarketOrderDtosAsync(accessToken, itemTypeId);
        }
    }
}
