using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cerberus.Dto
{
    public class EsiAsset
    {
        [JsonPropertyName("item_id")]
        public long ItemId { get; set; }

        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("location_id")]
        public long? LocationId { get; set; }

        [JsonPropertyName("location_type")]
        public string LocationType { get; set; } = "";

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("is_singleton")]
        public bool IsSingleton { get; set; }

        [JsonPropertyName("is_blueprint_copy")]
        public bool? IsBlueprintCopy { get; set; }

        [JsonPropertyName("location_flag")]
        public string LocationFlag { get; set; } = "";

        /// <summary>
        /// TODO move this to another class since it's not from ESI>?
        /// </summary>
        public string ItemName { get; set; }
    }
}
