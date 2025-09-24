using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Dto
{
    public class CharacterDto
    {
        /// <summary>
        /// ESI character id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The last time this characters data was updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// This characters assets.
        /// </summary>
        public EsiAsset[] Assets { get; set; } = new EsiAsset[0];

        /// <summary>
        /// Wallet transactions for this character.
        /// </summary>
        public Dictionary<long, EsiWalletTransaction> WalletTransactions { get; set; } = new Dictionary<long, EsiWalletTransaction>();

        public Dictionary<long, TransactionGroup> TransactionGroups { get; set; } = new Dictionary<long, TransactionGroup>();
    }

    public class TransactionGroup
    {
        /// <summary>
        /// Name of the asset.
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Total number of these items you have, even before tracking began.
        /// </summary>
        public long TotalQuantity { get; set; }

        /// <summary>
        /// Total number of these items you have, since tracking began.
        /// </summary>
        public long TotalTrackedQuantity { get; set; }

        /// <summary>
        /// Average tracked price of the asset.
        /// </summary>
        public decimal AverageTrackedPrice { get; set; }

        /// <summary>
        /// Running cost of the tracked portion of the asset.
        /// </summary>
        public decimal TotalTrackedAssetPrice { get; set; }

        /// <summary>
        /// Total current value of the asset including tracked and not tracked.
        /// </summary>
        public decimal TotalAssetValue { get; set; }
    }
}
