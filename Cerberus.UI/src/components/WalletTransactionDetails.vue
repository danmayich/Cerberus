<template>
  <div class="wallet-transaction" :class="{ 'transaction-buy': transaction.isBuy, 'transaction-sell': !transaction.isBuy }">
    <div class="transaction-header">
      <div class="transaction-main">
        <div class="transaction-type">
          {{ transaction.isBuy ? 'Buy' : 'Sell' }}
        </div>
        <div class="item-name">
          {{ transaction.itemName }}
        </div>
      </div>
      <div class="transaction-date">
        {{ formatDate(transaction.date) }}
      </div>
    </div>
    <div class="details-grid">
      <div class="detail-item">
        <label>Quantity:</label>
        <span>{{ formatNumber(transaction.quantity) }}</span>
      </div>
      <div class="detail-item">
        <label>Unit Price:</label>
        <span>{{ formatCurrency(transaction.unitPrice) }}</span>
      </div>
      <div class="detail-item">
        <label>Total Value:</label>
        <span>{{ formatCurrency(transaction.unitPrice * transaction.quantity) }}</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'WalletTransactionDetails',
  props: {
    transaction: {
      type: Object,
      required: true
    }
  },
  methods: {
    formatNumber(value) {
      return new Intl.NumberFormat().format(value)
    },
    formatCurrency(value) {
      return new Intl.NumberFormat('en-US', {
        style: 'decimal',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }).format(value) + ' ISK'
    },
    formatDate(dateString) {
      return new Date(dateString).toLocaleString()
    }
  }
}
</script>

<style scoped>
.wallet-transaction {
  background: var(--panel-bg-soft);
  border: 1px solid var(--border-color);
  border-radius: 8px;
  margin-bottom: 1rem;
  overflow: hidden;
}

.transaction-header {
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  border-bottom: 1px solid var(--border-color);
  background: var(--surface-raised);
}

.transaction-main {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.transaction-type {
  font-weight: 700;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  text-transform: capitalize;
  font-size: 0.85em;
  white-space: nowrap;
  letter-spacing: 0.4px;
}

.item-name {
  font-weight: 600;
  font-size: 1.1rem;
  color: var(--text-primary);
}

.transaction-buy .transaction-type {
  background: var(--success-bg);
  color: var(--success-text);
  border: 1px solid var(--success-border);
}

.transaction-sell .transaction-type {
  background: var(--danger-bg);
  color: var(--danger-text);
  border: 1px solid var(--danger-border);
}

.transaction-date {
  color: var(--text-secondary);
  font-size: 0.9rem;
}

.details-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  padding: 1rem;
}

.detail-item {
  display: flex;
  flex-direction: column;
  padding: 10px;
  background: var(--surface-raised);
  border-left: 3px solid var(--border-color);
  border-radius: 4px;
}

.detail-item label {
  font-size: 0.85rem;
  color: var(--text-secondary);
  margin-bottom: 0.5rem;
  letter-spacing: 0.4px;
}

.detail-item span {
  font-weight: 600;
  color: var(--text-primary);
}

.transaction-buy {
  border-left: 4px solid #22c55e;
}

.transaction-sell {
  border-left: 4px solid #ef4444;
}
</style>