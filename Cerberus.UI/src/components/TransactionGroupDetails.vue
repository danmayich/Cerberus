<template>
  <div class="transaction-group">
    <div class="item-header">
      <span class="item-name">{{ group.itemName }}</span>
    </div>
    <div class="details-grid">
      <div class="detail-item">
        <label>Total Quantity:</label>
        <span>{{ formatNumber(group.totalQuantity) }}</span>
      </div>
      <div class="detail-item">
        <label>Tracked Quantity:</label>
        <span>{{ formatNumber(group.totalTrackedQuantity) }}</span>
      </div>
      <div class="detail-item">
        <label>Average Price:</label>
        <span>{{ formatCurrency(group.averageTrackedPrice) }}</span>
      </div>
      <div class="detail-item">
        <label>Tracked Asset Price:</label>
        <span>{{ formatCurrency(group.totalTrackedAssetPrice) }}</span>
      </div>
      <div class="detail-item">
        <label>Total Asset Value:</label>
        <span>{{ formatCurrency(group.totalAssetValue) }}</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TransactionGroupDetails',
  props: {
    group: {
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
    }
  }
}
</script>

<style scoped>
.transaction-group {
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 4px;
  margin-bottom: 1rem;
  padding: 1rem;
}

.item-header {
  margin-bottom: 1rem;
}

.item-name {
  font-size: 1.1rem;
  font-weight: bold;
  color: #2c3e50;
}

.details-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
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
</style>