<template>
  <div class="character">
    <div class="page-header">
      <div>
        <h1>Character Information</h1>
      </div>
    </div>
    
    <div v-if="loading" class="loading" role="status" aria-live="polite">
      <span class="loading-spinner" aria-hidden="true"></span>
      <span>Loading character information...</span>
    </div>
    
    <div v-else-if="error" class="error">
      {{ error }}
    </div>
    
    <div v-else-if="character" class="character-data">
      <!-- Assets Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('assets')">
          <h3>
            <span>Assets</span>
            <span class="section-count">{{ assetCount }}</span>
          </h3>
          <span class="toggle-icon">{{ sectionStates.assets ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.assets" class="section-content">
          <div v-if="character.assets.length > 0">
            <div class="asset-filters">
              <div class="filter-group">
                <label for="nameFilter">Filter by Name:</label>
                <input 
                  id="nameFilter" 
                  v-model="assetFilters.nameFilter" 
                  type="text" 
                  placeholder="Search name..."
                  class="filter-input"
                />
              </div>
              <div class="filter-group">
                <label for="locationFilter">Filter by Location:</label>
                <input 
                  id="locationFilter" 
                  v-model="assetFilters.locationFilter" 
                  type="text" 
                  placeholder="Search location..."
                  class="filter-input"
                />
              </div>
              <div class="filter-group">
                <label for="sortOrder">Sort by Quantity:</label>
                <select id="sortOrder" v-model="assetFilters.sortOrder" class="filter-select">
                  <option value="none">None</option>
                  <option value="asc">Low to High</option>
                  <option value="desc">High to Low</option>
                </select>
              </div>
            </div>
            <div class="table-container">
              <table>
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Location</th>
                    <th>Quantity</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="asset in filteredAndSortedAssets" :key="asset.id">
                    <td>{{ asset.name || asset.id }}</td>
                    <td>{{ asset.location }}</td>
                    <td>{{ asset.quantity }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <p v-else>No assets found</p>
        </div>
      </div>

      <!-- Wallet Transactions Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('walletTransactions')">
          <h3>
            <span>Wallet Transactions</span>
            <span class="section-count">{{ walletTransactionCount }}</span>
          </h3>
          <span class="toggle-icon">{{ sectionStates.walletTransactions ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.walletTransactions" class="section-content">
          <div v-if="walletTransactionCount > 0">
            <div class="asset-filters">
              <div class="filter-group">
                <label for="walletItemFilter">Filter by Item:</label>
                <input
                  id="walletItemFilter"
                  v-model="walletFilters.itemFilter"
                  type="text"
                  placeholder="Search item..."
                  class="filter-input"
                />
              </div>
              <div class="filter-group">
                <label for="walletSideFilter">Filter by Side:</label>
                <select id="walletSideFilter" v-model="walletFilters.sideFilter" class="filter-select">
                  <option value="all">All</option>
                  <option value="buy">Buy</option>
                  <option value="sell">Sell</option>
                </select>
              </div>
              <div class="filter-group">
                <label for="walletSortOrder">Sort by:</label>
                <select id="walletSortOrder" v-model="walletFilters.sortOrder" class="filter-select">
                  <option value="date-desc">Newest First</option>
                  <option value="date-asc">Oldest First</option>
                  <option value="value-desc">Highest Value</option>
                  <option value="value-asc">Lowest Value</option>
                </select>
              </div>
            </div>
            <div class="table-container">
              <table class="wallet-table">
                <thead>
                  <tr>
                    <th class="wallet-side-col">Side</th>
                    <th class="wallet-item-col">Item</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Total Value</th>
                    <th class="wallet-track-col">Track Position</th>
                    <th>Date</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="transaction in filteredWalletTransactions" :key="transaction._rowKey">
                    <td class="wallet-side-col">
                      <span
                        class="wallet-side-badge"
                        :class="transaction.isBuy ? 'wallet-side-buy' : 'wallet-side-sell'"
                      >
                        {{ transaction.isBuy ? 'Buy' : 'Sell' }}
                      </span>
                    </td>
                    <td class="wallet-item-col">{{ transaction.itemName }}</td>
                    <td>{{ formatNumber(transaction.quantity) }}</td>
                    <td>{{ formatCurrency(transaction.unitPrice) }}</td>
                    <td>{{ formatCurrency((transaction.unitPrice || 0) * (transaction.quantity || 0)) }}</td>
                    <td class="wallet-track-col">
                      <div class="track-control">
                        <label class="track-switch" :for="`track-${transaction._rowKey}`">
                          <input
                            :id="`track-${transaction._rowKey}`"
                            type="checkbox"
                            :checked="isPositionTracked(transaction._rowKey)"
                            :disabled="trackingRequestInFlight || isTrackingInFlight(transaction._rowKey)"
                            @change="setPositionTracking(transaction, $event.target.checked)"
                          />
                          <span class="track-slider"></span>
                        </label>
                        <span v-if="isTrackingInFlight(transaction._rowKey)" class="track-spinner" aria-hidden="true"></span>
                      </div>
                    </td>
                    <td>{{ formatShortDate(transaction.date) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <p v-else>No wallet transactions found</p>
        </div>
      </div>

      <!-- Tracked Positions Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('trackedPositions')">
          <h3>
            <span>Tracked Positions</span>
            <span class="section-count">{{ trackedPositionCount }}</span>
          </h3>
          <span class="toggle-icon">{{ sectionStates.trackedPositions ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.trackedPositions" class="section-content">
          <div v-if="trackedPositionCount > 0">
            <div class="table-container">
              <table class="wallet-table">
                <thead>
                  <tr>
                    <th class="wallet-side-col">Side</th>
                    <th class="wallet-item-col">Item</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Total Value</th>
                    <th>Date</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="transaction in trackedWalletTransactions" :key="`tracked-${transaction._rowKey}`">
                    <td class="wallet-side-col">
                      <span
                        class="wallet-side-badge"
                        :class="transaction.isBuy ? 'wallet-side-buy' : 'wallet-side-sell'"
                      >
                        {{ transaction.isBuy ? 'Buy' : 'Sell' }}
                      </span>
                    </td>
                    <td class="wallet-item-col">{{ transaction.itemName }}</td>
                    <td>{{ formatNumber(transaction.quantity) }}</td>
                    <td>{{ formatCurrency(transaction.unitPrice) }}</td>
                    <td>{{ formatCurrency((transaction.unitPrice || 0) * (transaction.quantity || 0)) }}</td>
                    <td>{{ formatShortDate(transaction.date) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <p v-else>No tracked positions found</p>
        </div>
      </div>

      <!-- Transaction Groups Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('transactionGroups')">
          <h3>
            <span>Tracked Transaction Groups</span>
            <span class="section-count">{{ transactionGroupCount }}</span>
          </h3>
          <span class="toggle-icon">{{ sectionStates.transactionGroups ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.transactionGroups" class="section-content">
          <div v-if="transactionGroupCount > 0">
            <div class="asset-filters">
              <div class="filter-group">
                <label for="groupNameFilter">Filter by Name:</label>
                <input
                  id="groupNameFilter"
                  v-model="transactionGroupFilters.nameFilter"
                  type="text"
                  placeholder="Search name..."
                  class="filter-input"
                />
              </div>
              <div class="filter-group">
                <label for="groupSortOrder">Sort by:</label>
                <select id="groupSortOrder" v-model="transactionGroupFilters.sortOrder" class="filter-select">
                  <option value="cost-desc">Highest Avg Cost</option>
                  <option value="cost-asc">Lowest Avg Cost</option>
                  <option value="total-desc">Highest Total Cost</option>
                  <option value="total-asc">Lowest Total Cost</option>
                  <option value="quantity-desc">Most Amount</option>
                  <option value="quantity-asc">Least Amount</option>
                  <option value="value-desc">Highest Market Value</option>
                  <option value="value-asc">Lowest Market Value</option>
                  <option value="profit-desc">Highest Profit</option>
                  <option value="profit-asc">Lowest Profit</option>
                  <option value="name-asc">Name A-Z</option>
                  <option value="name-desc">Name Z-A</option>
                </select>
              </div>
            </div>
            <div class="table-container">
              <table class="wallet-table">
                <thead>
                  <tr>
                    <th class="wallet-item-col">Item</th>
                    <th>Total Amount</th>
                    <th>Average Cost</th>
                    <th>Total Cost</th>
                    <th>Current Market Value</th>
                    <th>Tax</th>
                    <th>Profit</th>
                    <th>Profit %</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="group in trackedTransactionGroups" :key="group._rowKey">
                    <td class="wallet-item-col">{{ group.itemName }}</td>
                    <td>{{ formatNumber(group.totalTrackedQuantity) }}</td>
                    <td>{{ formatCurrency(group.averageTrackedPrice) }}</td>
                    <td>{{ formatCurrency(group.totalTrackedAssetPrice) }}</td>
                    <td>{{ formatCurrency(group.totalAssetValue) }}</td>
                    <td>{{ formatCurrency(group.totalTax) }}</td>
                    <td :class="profitCellClass(group.totalProfit)">{{ formatCurrency(group.totalProfit) }}</td>
                    <td :class="profitCellClass(resolveProfitPercent(group))">{{ formatPercentage(resolveProfitPercent(group)) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <p v-else>No transaction groups found</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useCharacterStore } from '../stores/character'
import characterService from '../services/character.service'

export default {
  name: 'CharacterView',
  setup() {
    const characterStore = useCharacterStore()
    return { characterStore }
  },
  data() {
    return {
      sectionStates: {
        assets: false,
        walletTransactions: false,
        trackedPositions: false,
        transactionGroups: false
      },
      assetFilters: {
        nameFilter: '',
        locationFilter: '',
        sortOrder: 'none'
      },
      walletFilters: {
        itemFilter: '',
        sideFilter: 'all',
        sortOrder: 'date-desc'
      },
      transactionGroupFilters: {
        nameFilter: '',
        sortOrder: 'cost-desc'
      },
      trackedPositions: {},
      trackingInFlight: {},
      trackingRequestInFlight: false
    }
  },
  computed: {
    character() {
      return this.characterStore.character
    },
    loading() {
      return this.characterStore.loading
    },
    error() {
      return this.characterStore.error
    },
    assetCount() {
      return this.character?.assets?.length || 0
    },
    walletTransactionCount() {
      return Object.keys(this.character?.walletTransactions || {}).length
    },
    trackedPositionCount() {
      return this.trackedWalletTransactions.length
    },
    transactionGroupCount() {
      return Object.keys(this.character?.transactionGroups || {}).length
    },
    filteredAndSortedAssets() {
      if (!this.character || !this.character.assets) return []
      
      let assets = [...this.character.assets]
      
      // Filter by name
      if (this.assetFilters.nameFilter) {
        const nameSearch = this.assetFilters.nameFilter.toLowerCase()
        assets = assets.filter(asset => 
          (asset.name || asset.id || '').toString().toLowerCase().includes(nameSearch)
        )
      }
      
      // Filter by location
      if (this.assetFilters.locationFilter) {
        const locationSearch = this.assetFilters.locationFilter.toLowerCase()
        assets = assets.filter(asset => 
          (asset.location || '').toString().toLowerCase().includes(locationSearch)
        )
      }
      
      // Sort by quantity
      if (this.assetFilters.sortOrder === 'asc') {
        assets.sort((a, b) => (a.quantity || 0) - (b.quantity || 0))
      } else if (this.assetFilters.sortOrder === 'desc') {
        assets.sort((a, b) => (b.quantity || 0) - (a.quantity || 0))
      }
      
      return assets
    },
    filteredWalletTransactions() {
      let transactions = Object.values(this.character?.walletTransactions || {})
        .filter(transaction => !!transaction)
        .map((transaction, index) => ({
          ...transaction,
          _rowKey: `${transaction.transactionId ?? index}`
        }))

      if (this.walletFilters.itemFilter) {
        const itemSearch = this.walletFilters.itemFilter.toLowerCase()
        transactions = transactions.filter(transaction =>
          (transaction.itemName || '').toString().toLowerCase().includes(itemSearch)
        )
      }

      if (this.walletFilters.sideFilter === 'buy') {
        transactions = transactions.filter(transaction => transaction.isBuy)
      } else if (this.walletFilters.sideFilter === 'sell') {
        transactions = transactions.filter(transaction => !transaction.isBuy)
      }

      if (this.walletFilters.sortOrder === 'date-asc') {
        transactions.sort((a, b) => new Date(a.date) - new Date(b.date))
      } else if (this.walletFilters.sortOrder === 'date-desc') {
        transactions.sort((a, b) => new Date(b.date) - new Date(a.date))
      } else if (this.walletFilters.sortOrder === 'value-asc') {
        transactions.sort((a, b) => ((a.unitPrice || 0) * (a.quantity || 0)) - ((b.unitPrice || 0) * (b.quantity || 0)))
      } else if (this.walletFilters.sortOrder === 'value-desc') {
        transactions.sort((a, b) => ((b.unitPrice || 0) * (b.quantity || 0)) - ((a.unitPrice || 0) * (a.quantity || 0)))
      }

      return transactions
    },
    trackedWalletTransactions() {
      const persistedTracked = Object.values(this.character?.trackedPositions || {})
        .filter(transaction => !!transaction)
        .map((transaction, index) => ({
          ...transaction,
          _rowKey: `${transaction.transactionId ?? `persisted-${index}`}`
        }))

      const walletTransactions = Object.values(this.character?.walletTransactions || {})
        .filter(transaction => !!transaction)
        .map((transaction, index) => ({
          ...transaction,
          _rowKey: `${transaction.transactionId ?? index}`
        }))

      const transactionMap = new Map()

      for (const transaction of persistedTracked) {
        transactionMap.set(transaction._rowKey, transaction)
      }

      for (const transaction of walletTransactions) {
        if (this.isPositionTracked(transaction._rowKey)) {
          transactionMap.set(transaction._rowKey, transaction)
        }
      }

      return Array.from(transactionMap.values())
        .filter(transaction => this.isPositionTracked(transaction._rowKey))
        .sort((a, b) => new Date(b.date) - new Date(a.date))
    },
    trackedTransactionGroups() {
      let groups = Object.entries(this.character?.transactionGroups || {})
        .filter(([, group]) => !!group)
        .map(([typeId, group]) => ({
          ...group,
          itemName: group.itemName || `Type ${typeId}`,
          _rowKey: `${typeId}`
        }))

      // Filter by name
      if (this.transactionGroupFilters.nameFilter) {
        const nameSearch = this.transactionGroupFilters.nameFilter.toLowerCase()
        groups = groups.filter(group =>
          (group.itemName || '').toString().toLowerCase().includes(nameSearch)
        )
      }

      // Sort by selected column
      if (this.transactionGroupFilters.sortOrder === 'name-asc') {
        groups.sort((a, b) => (a.itemName || '').localeCompare(b.itemName || ''))
      } else if (this.transactionGroupFilters.sortOrder === 'name-desc') {
        groups.sort((a, b) => (b.itemName || '').localeCompare(a.itemName || ''))
      } else if (this.transactionGroupFilters.sortOrder === 'quantity-asc') {
        groups.sort((a, b) => (a.totalTrackedQuantity || 0) - (b.totalTrackedQuantity || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'quantity-desc') {
        groups.sort((a, b) => (b.totalTrackedQuantity || 0) - (a.totalTrackedQuantity || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'cost-asc') {
        groups.sort((a, b) => (a.averageTrackedPrice || 0) - (b.averageTrackedPrice || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'cost-desc') {
        groups.sort((a, b) => (b.averageTrackedPrice || 0) - (a.averageTrackedPrice || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'total-asc') {
        groups.sort((a, b) => (a.totalTrackedAssetPrice || 0) - (b.totalTrackedAssetPrice || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'total-desc') {
        groups.sort((a, b) => (b.totalTrackedAssetPrice || 0) - (a.totalTrackedAssetPrice || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'value-asc') {
        groups.sort((a, b) => (a.totalAssetValue || 0) - (b.totalAssetValue || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'value-desc') {
        groups.sort((a, b) => (b.totalAssetValue || 0) - (a.totalAssetValue || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'profit-asc') {
        groups.sort((a, b) => (a.totalProfit || 0) - (b.totalProfit || 0))
      } else if (this.transactionGroupFilters.sortOrder === 'profit-desc') {
        groups.sort((a, b) => (b.totalProfit || 0) - (a.totalProfit || 0))
      }

      return groups
    }
  },
  methods: {
    toggleSection(sectionName) {
      this.sectionStates[sectionName] = !this.sectionStates[sectionName]
    },
    formatDate(dateString) {
      return new Date(dateString).toLocaleString()
    },
    formatShortDate(dateString) {
      return new Date(dateString).toLocaleDateString('en-US', {
        month: '2-digit',
        day: '2-digit',
        year: 'numeric'
      })
    },
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
    },
    profitCellClass(value) {
      if (value === null || value === undefined || Number.isNaN(value)) {
        return 'profit-neutral'
      }

      return value >= 0 ? 'profit-positive' : 'profit-negative'
    },
    isPositionTracked(rowKey) {
      if (this.trackedPositions[rowKey] !== undefined) {
        return !!this.trackedPositions[rowKey]
      }

      return !!this.character?.trackedPositions?.[rowKey]
    },
    isTrackingInFlight(rowKey) {
      return !!this.trackingInFlight[rowKey]
    },
    async setPositionTracking(transaction, isTracked) {
      const rowKey = transaction?._rowKey
      if (!rowKey) return

      // Only allow one tracking mutation at a time across the whole table.
      if (this.trackingRequestInFlight && !this.trackingInFlight[rowKey]) {
        return
      }

      this.trackedPositions[rowKey] = isTracked

      this.trackingInFlight[rowKey] = true
      this.trackingRequestInFlight = true
      try {
        let result
        if (isTracked) {
          const payload = { ...transaction }
          delete payload._rowKey
          result = await characterService.trackPosition(payload)
        } else {
          result = await characterService.untrackPosition(transaction.transactionId)
        }

        if (result?.transactionGroups && this.characterStore.character) {
          this.characterStore.character.transactionGroups = result.transactionGroups
        }
      } catch (error) {
        console.error('Failed to update tracked position:', error)
        this.trackedPositions[rowKey] = !isTracked
      } finally {
        this.trackingInFlight[rowKey] = false
        this.trackingRequestInFlight = Object.values(this.trackingInFlight).some(Boolean)
      }
    },
    async fetchCharacterData() {
      await this.characterStore.fetchCharacter()
    }
  },
  created() {
    this.fetchCharacterData()
  }
}
</script>

<style scoped>
.character {
  max-width: 1280px;
  margin: 0 auto;
  padding: 12px 14px;
}

.page-header {
  margin-bottom: 12px;
}

h1 {
  margin: 0;
  font-size: 1.55rem;
  line-height: 1.1;
}

.page-subtitle {
  margin: 4px 0 0;
  font-size: 0.84rem;
  color: var(--text-secondary);
}

.loading {
  padding: 20px;
  display: inline-flex;
  align-items: center;
  gap: 10px;
  color: var(--text-secondary);
  font-size: 1rem;
  border: 1px solid var(--border-color);
  border-radius: 8px;
  background: var(--panel-bg-soft);
}

.loading-spinner {
  display: inline-block;
  width: 14px;
  height: 14px;
  border-radius: 50%;
  border: 2px solid var(--border-color);
  border-top-color: var(--accent-blue);
  transform-origin: center center;
  vertical-align: middle;
  animation: loading-spin 0.8s linear infinite;
}

@keyframes loading-spin {
  to {
    transform: rotate(360deg);
  }
}

.error {
  padding: 14px;
  text-align: center;
  color: var(--danger-text);
  background: var(--danger-bg);
  border: 1px solid var(--danger-border);
  border-radius: 8px;
}

.section {
  margin: 0.75rem 0;
  border: 1px solid var(--border-color);
  border-radius: 8px;
  overflow: hidden;
  background: var(--panel-bg);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 12px;
  cursor: pointer;
  background: var(--panel-bg-soft);
  border-bottom: 1px solid var(--border-color);
  transition: background-color 0.2s ease;
}

.section-header:hover {
  background: var(--surface-hover);
}

.section-header h3 {
  display: flex;
  align-items: center;
  gap: 8px;
  margin: 0;
  color: var(--text-primary);
  font-size: 0.98em;
  letter-spacing: 0.4px;
}

.section-count {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 26px;
  height: 20px;
  padding: 0 6px;
  border-radius: 999px;
  background: var(--info-bg);
  border: 1px solid var(--info-border);
  color: var(--info-text);
  font-size: 0.8rem;
  letter-spacing: 0;
  font-weight: 700;
}

.toggle-icon {
  font-size: 1rem;
  transition: transform 0.2s;
  color: var(--text-secondary);
}

.section-content {
  padding: 12px;
  background: var(--panel-bg);
}

.asset-filters {
  display: flex;
  gap: 10px;
  margin-bottom: 12px;
  padding: 10px;
  background: var(--panel-bg-soft);
  border-radius: 6px;
  border: 1px solid var(--border-color);
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 4px;
  flex: 1;
  min-width: 160px;
}

.filter-group label {
  font-size: 0.78em;
  color: var(--text-secondary);
  font-weight: 600;
  letter-spacing: 0.3px;
}

.filter-input,
.filter-select {
  padding: 6px 8px;
  background: var(--input-bg);
  border: 1px solid var(--border-color);
  border-radius: 4px;
  color: var(--text-primary);
  font-family: inherit;
  font-size: 0.88em;
  transition: border-color 0.2s ease;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: var(--accent-blue);
  box-shadow: 0 0 0 3px rgba(96, 165, 250, 0.2);
}

.filter-select option {
  background: var(--input-bg);
  color: var(--text-primary);
}

.table-container {
  overflow-x: auto;
  border: 1px solid var(--border-color);
  border-radius: 8px;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 0;
  font-size: 0.92em;
  table-layout: fixed;
}

thead {
  background: var(--table-head-bg);
  color: var(--table-head-text);
}

th {
  padding: 8px 10px;
  text-align: left;
  font-weight: 700;
  letter-spacing: 0.3px;
}

td {
  padding: 8px 10px;
  border-bottom: 1px solid var(--border-color);
  color: var(--text-secondary);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

tr:hover {
  background-color: var(--surface-hover);
}

.wallet-table {
  table-layout: auto;
}

.wallet-table .wallet-side-col {
  width: 1%;
  white-space: nowrap;
  text-align: center;
  padding-left: 6px;
  padding-right: 6px;
}

.wallet-side-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 42px;
  padding: 2px 7px;
  border-radius: 999px;
  font-size: 0.74rem;
  font-weight: 700;
  letter-spacing: 0.4px;
  text-transform: uppercase;
}

.wallet-side-buy {
  color: #fecaca;
  background: #31181d;
  border: 1px solid #6d3038;
}

.wallet-side-sell {
  color: #bbf7d0;
  background: #11251a;
  border: 1px solid #264a34;
}

.wallet-table .wallet-item-col {
  min-width: 340px;
  white-space: normal;
  overflow: visible;
  text-overflow: clip;
  word-break: break-word;
}

.wallet-track-col {
  width: 1%;
  white-space: nowrap;
  text-align: center;
}

.track-control {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 38px;
}

.track-switch {
  position: relative;
  display: inline-block;
  width: 38px;
  height: 20px;
}

.track-switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.track-slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #243040;
  border: 1px solid #334155;
  transition: 0.2s;
  border-radius: 999px;
}

.track-slider:before {
  position: absolute;
  content: '';
  height: 14px;
  width: 14px;
  left: 2px;
  top: 2px;
  background-color: #94a3b8;
  transition: 0.2s;
  border-radius: 50%;
}

.track-switch input:checked + .track-slider {
  background-color: #1e3a5f;
  border-color: var(--accent-blue);
}

.track-switch input:checked + .track-slider:before {
  transform: translateX(18px);
  background-color: var(--accent-blue);
}

.track-spinner {
  position: absolute;
  right: -18px;
  top: 50%;
  transform: translateY(-50%) rotate(0deg);
  display: inline-block;
  width: 12px;
  height: 12px;
  border-radius: 50%;
  border: 2px solid var(--border-color);
  border-top-color: var(--accent-blue);
  transform-origin: center center;
  backface-visibility: hidden;
  animation: loading-spin-track 0.8s linear infinite;
}

@keyframes loading-spin-track {
  to {
    transform: translateY(-50%) rotate(360deg);
  }
}

.profit-positive {
  color: #86efac;
  font-weight: 700;
}

.profit-negative {
  color: #fca5a5;
  font-weight: 700;
}

.profit-neutral {
  color: var(--text-secondary);
  font-weight: 700;
}

@media (max-width: 768px) {
  .character {
    padding: 10px;
  }

  .section-header {
    padding: 9px 10px;
  }

  .section-content {
    padding: 10px;
  }

  .filter-group {
    min-width: 130px;
  }
}
</style>