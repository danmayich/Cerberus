using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cerberus.Dto
{
    public class EsiWalletTransaction
    {
        [JsonPropertyName("transaction_id")]
        public long TransactionId { get; set; }

        [JsonPropertyName("client_id")]
        public long ClientId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("is_buy")]
        public bool IsBuy { get; set; }

        [JsonPropertyName("is_personal")]
        public bool IsPersonal { get; set; }

        [JsonPropertyName("journal_ref_id")]
        public long? JournalRefId { get; set; }

        [JsonPropertyName("location_id")]
        public long LocationId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }
    }
}
