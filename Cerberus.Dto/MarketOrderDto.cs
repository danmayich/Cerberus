using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cerberus.Dto
{
    public class MarketOrderDto
    {
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("is_buy_order")]
        public bool IsBuyOrder { get; set; }

        [JsonPropertyName("issued")]
        public DateTime Issued { get; set; }

        [JsonPropertyName("location_id")]
        public long LocationId { get; set; }

        [JsonPropertyName("min_volume")]
        public int MinVolume { get; set; }

        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("range")]
        public string Range { get; set; } = string.Empty;

        [JsonPropertyName("system_id")]
        public long SystemId { get; set; }

        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("volume_remain")]
        public int VolumeRemain { get; set; }

        [JsonPropertyName("volume_total")]
        public int VolumeTotal { get; set; }
    }
}
