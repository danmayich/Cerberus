using Cerberus.Dto;
using Cerberus.Services.Assets;
using Cerberus.Services.Esi;
using System.Linq;

namespace Cerberus.Application.Assets
{
    public class AssetRetrievalApplication(AssetRetrievalService assetRetreivalService)
    {
        public async Task<EsiAsset[]> GetAssets(long characterId, string accessToken)
        {
            var assets = await assetRetreivalService.GetAssets(characterId, accessToken);
            if (assets == null || assets.Length == 0)
                return assets;

            // Group by TypeId and LocationId and collapse duplicates by summing quantities
            var merged = assets
                .GroupBy(a => new { a.TypeId, a.LocationId })
                .Select(g =>
                {
                    var first = g.First();
                    return new EsiAsset
                    {
                        // keep the original item id if present (may be 0 for stackables)
                        ItemId = first.ItemId,
                        TypeId = g.Key.TypeId,
                        LocationId = g.Key.LocationId,
                        LocationType = first.LocationType,
                        Quantity = g.Sum(x => x.Quantity),
                        IsSingleton = first.IsSingleton,
                        IsBlueprintCopy = first.IsBlueprintCopy,
                        LocationFlag = first.LocationFlag,
                        ItemName = first.ItemName
                    };
                })
                .ToArray();

            return merged;
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
