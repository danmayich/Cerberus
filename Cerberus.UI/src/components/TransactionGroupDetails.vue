<template>
  <div class="transaction-group">
    <div class="item-header">
      <span class="item-name">{{ group.itemName }}</span>
    </div>
    <div class="details-grid">
      <div class="detail-item">
        <label>Total Amount:</label>
        <span>{{ formatNumber(group.totalTrackedQuantity) }}</span>
      </div>
      <div class="detail-item">
        <label>Average Cost:</label>
        <span>{{ formatCurrency(group.averageTrackedPrice) }}</span>
      </div>
      <div class="detail-item">
        <label>Total Cost:</label>
        <span>{{ formatCurrency(group.totalTrackedAssetPrice) }}</span>
      </div>
      <div class="detail-item">
        <label>Profit %:</label>
        <span>{{ formatPercentage(resolveProfitPercent(group)) }}</span>
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
    },
    formatPercentage(value) {
      if (value === null || value === undefined || Number.isNaN(value)) {
        return 'N/A'
      }

      return new Intl.NumberFormat('en-US', {
        style: 'decimal',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
        signDisplay: 'always'
      }).format(value) + '%'
    },
    resolveProfitPercent(group) {
      if (group?.totalProfitPercent !== null && group?.totalProfitPercent !== undefined && !Number.isNaN(group.totalProfitPercent)) {
        return group.totalProfitPercent
      }

      const totalCost = group?.totalTrackedAssetPrice || 0
      if (totalCost <= 0) {
        return null
      }

      const profit = group?.totalProfit || 0
      return (profit / totalCost) * 100
    }
  }
}
</script>

<style scoped>
.transaction-group {
  background: var(--panel-bg-soft);
  border: 1px solid var(--border-color);
  border-radius: 8px;
  margin-bottom: 1rem;
  padding: 1rem;
}

.item-header {
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid var(--border-color);
}

.item-name {
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--text-primary);
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
</style>