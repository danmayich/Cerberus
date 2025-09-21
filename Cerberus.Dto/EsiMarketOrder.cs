using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cerberus.Dto
{
    public class EsiMarketOrder
    {
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }

        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("volume_total")]
        public int VolumeTotal { get; set; }

        [JsonPropertyName("volume_remain")]
        public int VolumeRemain { get; set; }

        [JsonPropertyName("is_buy_order")]
        public bool IsBuyOrder { get; set; }

        [JsonPropertyName("is_corporation")]
        public bool IsCorporation { get; set; }

        [JsonPropertyName("location_id")]
        public long LocationId { get; set; }

        [JsonPropertyName("region_id")]
        public long RegionId { get; set; }

        [JsonPropertyName("issued")]
        public DateTime Issued { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("min_volume")]
        public int? MinVolume { get; set; }

        [JsonPropertyName("escrow")]
        public decimal? Escrow { get; set; }

        [JsonPropertyName("range")]
        public string Range { get; set; } = "";

        // Not in the JSON, but you had it before
        public string State { get; set; } = "";
    }
}
