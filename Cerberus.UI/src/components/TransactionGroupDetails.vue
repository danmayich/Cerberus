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
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  margin-bottom: 1rem;
  padding: 1rem;
  box-shadow: 0 0 15px rgba(0, 153, 255, 0.2);
  transition: all 0.3s ease;
}

.transaction-group:hover {
  border-color: var(--primary-color);
  box-shadow: 0 0 25px rgba(0, 212, 255, 0.4);
}

.item-header {
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid rgba(0, 212, 255, 0.2);
}

.item-name {
  font-size: 1.1rem;
  font-weight: bold;
  color: var(--secondary-color);
  letter-spacing: 1px;
  text-shadow: 0 0 10px rgba(255, 215, 0, 0.3);
}

.details-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
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
</style>