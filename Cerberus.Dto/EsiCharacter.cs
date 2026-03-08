using System;
using System.Text.Json.Serialization;

namespace Cerberus.Dto
{
    public class EsiCharacter
    {
        [JsonPropertyName("alliance_id")]
        public long? AllianceId { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonPropertyName("bloodline_id")]
        public int? BloodlineId { get; set; }

        [JsonPropertyName("corporation_id")]
        public long CorporationId { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("faction_id")]
        public long? FactionId { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("race_id")]
        public int? RaceId { get; set; }

        [JsonPropertyName("security_status")]
        public decimal? SecurityStatus { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }
}
