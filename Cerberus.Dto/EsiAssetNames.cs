using System.Text.Json.Serialization;

namespace Cerberus.Dto
{
    public class EsiAssetNames
    {
        [JsonPropertyName("item_id")]
        public long ItemId { get; set; }

        [JsonPropertyName("name")]
        public int Name { get; set; }
    }
}
