using Cerberus.Dto;
using Cerberus.Services.Assets;
using Cerberus.Services.Esi;

namespace Cerberus.Application.Assets
{
    public class AssetRetrievalApplication(AssetRetrievalService assetRetreivalService)
    {
        public async Task<EsiAsset[]> GetAssets(long characterId, string accessToken)
        {
            return await assetRetreivalService.GetAssets(characterId, accessToken);
        }

        /// <summary>
        /// Take the highest buy order
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="itemTypeId"></param>
        /// <returns></returns>
        public async Task<MarketOrderDto?> GetMarketOrderDtosAsync(string accessToken, string itemTypeId)
        {
            var marketOrders =  await assetRetreivalService.GetMarketOrderDtosAsync(accessToken, itemTypeId);
            return marketOrders.OrderByDescending(x => x.Price).FirstOrDefault();
        }
    }
}
