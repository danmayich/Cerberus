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
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 4px;
  margin-bottom: 1rem;
  overflow: hidden;
}

.transaction-header {
  padding: 0.75rem 1rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  border-bottom: 1px solid #e9ecef;
}

.transaction-main {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.transaction-type {
  font-weight: bold;
  padding: 0.25rem 0.75rem;
  border-radius: 3px;
  text-transform: uppercase;
  font-size: 0.9rem;
  white-space: nowrap;
}

.item-name {
  font-weight: 600;
  font-size: 1.1rem;
  color: #2c3e50;
}

.transaction-buy .transaction-type {
  background-color: #d4edda;
  color: #155724;
}

.transaction-sell .transaction-type {
  background-color: #f8d7da;
  color: #721c24;
}

.transaction-date {
  color: #6c757d;
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
}

.detail-item label {
  font-size: 0.9rem;
  color: #6c757d;
  margin-bottom: 0.25rem;
}

.detail-item span {
  font-weight: 500;
}

.transaction-buy {
  border-left: 4px solid #28a745;
}

.transaction-sell {
  border-left: 4px solid #dc3545;
}
</style>