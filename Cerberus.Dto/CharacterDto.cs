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

        /// <summary>
        /// Wallet transactions explicitly selected for position tracking.
        /// </summary>
        public Dictionary<long, EsiWalletTransaction> TrackedPositions { get; set; } = new Dictionary<long, EsiWalletTransaction>();

        /// <summary>
        /// Aggregated tracked buy transactions grouped by type id.
        /// </summary>
        public Dictionary<long, TransactionGroup> TransactionGroups { get; set; } = new Dictionary<long, TransactionGroup>();

        public Dictionary<long, WalletJournalEntry> WalletJournalEntries { get; set; } = new Dictionary<long, WalletJournalEntry>();

        /// <summary>
        /// ESI character info
        /// </summary>
        public EsiCharacter? CharacterInfo { get; set; }
    }

    public class TransactionGroup
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Legacy asset quantity field retained for compatibility.
        /// </summary>
        public long TotalQuantity { get; set; }

        /// <summary>
        /// Total number of tracked units in this group.
        /// </summary>
        public long TotalTrackedQuantity { get; set; }

        /// <summary>
        /// Average cost per unit across tracked buy transactions.
        /// </summary>
        public decimal AverageTrackedPrice { get; set; }

        /// <summary>
        /// Running total cost of the tracked buy transactions.
        /// </summary>
        public decimal TotalTrackedAssetPrice { get; set; }

        /// <summary>
        /// Legacy asset valuation field retained for compatibility.
        /// </summary>
        public decimal TotalAssetValue { get; set; }
    }
}
