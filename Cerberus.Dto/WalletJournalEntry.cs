
using System.Text.Json.Serialization;

namespace Cerberus.Dto
{
    public class WalletJournalEntry
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("balance")]
        public double Balance { get; set; }

        [JsonPropertyName("context_id")]
        public long ContextId { get; set; }

        [JsonPropertyName("context_id_type")]
        public string ContextIdType { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("first_party_id")]
        public long FirstPartyId { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("ref_type")]
        public string RefType { get; set; }

        [JsonPropertyName("second_party_id")]
        public long SecondPartyId { get; set; }
    }
}

