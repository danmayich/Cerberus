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
        <label>Transaction ID:</label>
        <span>{{ transaction.transactionId }}</span>
      </div>
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
      <div class="detail-item">
        <label>Type ID:</label>
        <span>{{ transaction.typeId }}</span>
      </div>
      <div class="detail-item">
        <label>Location ID:</label>
        <span>{{ transaction.locationId }}</span>
      </div>
      <div class="detail-item">
        <label>Journal Ref:</label>
        <span>{{ transaction.journalRefId }}</span>
      </div>
      <div class="detail-item">
        <label>Type:</label>
        <span>{{ transaction.isPersonal ? 'Personal' : 'Corporate' }}</span>
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
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  margin-bottom: 1rem;
  overflow: hidden;
  box-shadow: 0 0 15px rgba(0, 153, 255, 0.2);
  transition: all 0.3s ease;
}

.wallet-transaction:hover {
  border-color: var(--primary-color);
  box-shadow: 0 0 25px rgba(0, 212, 255, 0.4);
}

.transaction-header {
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  border-bottom: 2px solid var(--accent-blue);
  background: linear-gradient(90deg, rgba(0, 153, 255, 0.15) 0%, rgba(0, 212, 255, 0.05) 100%);
}

.transaction-main {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.transaction-type {
  font-weight: bold;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  text-transform: uppercase;
  font-size: 0.85em;
  white-space: nowrap;
  letter-spacing: 1px;
  box-shadow: 0 0 10px rgba(0, 212, 255, 0.3);
}

.item-name {
  font-weight: 600;
  font-size: 1.1rem;
  color: var(--secondary-color);
  text-shadow: 0 0 10px rgba(255, 215, 0, 0.3);
  letter-spacing: 1px;
}

.transaction-buy .transaction-type {
  background: linear-gradient(135deg, rgba(0, 255, 0, 0.2) 0%, rgba(0, 200, 0, 0.1) 100%);
  color: #00ff00;
  border: 2px solid #00ff00;
  text-shadow: 0 0 10px rgba(0, 255, 0, 0.5);
}

.transaction-sell .transaction-type {
  background: linear-gradient(135deg, rgba(255, 0, 0, 0.2) 0%, rgba(200, 0, 0, 0.1) 100%);
  color: #ff4444;
  border: 2px solid #ff4444;
  text-shadow: 0 0 10px rgba(255, 0, 0, 0.5);
}

.transaction-date {
  color: var(--text-secondary);
  font-size: 0.9rem;
  font-family: 'Courier New', monospace;
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
  background: rgba(0, 153, 255, 0.05);
  border-left: 3px solid var(--accent-blue);
  border-radius: 4px;
  transition: all 0.3s ease;
}

.detail-item:hover {
  background: rgba(0, 212, 255, 0.1);
  border-left-color: var(--primary-color);
}

.detail-item label {
  font-size: 0.85rem;
  color: var(--text-secondary);
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.detail-item span {
  font-weight: 600;
  color: var(--primary-color);
  font-family: 'Courier New', monospace;
}

.transaction-buy {
  border-left: 5px solid #00ff00;
}

.transaction-sell {
  border-left: 5px solid #ff4444;
}
</style>