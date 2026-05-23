<template>
  <div class="character">
    <div class="page-header">
      <div>
        <h1>Character Information</h1>
        <p class="page-subtitle">Operational snapshot with compact section drill-downs.</p>
      </div>
      <div v-if="character" class="overview-stats">
        <div class="stat-chip">
          <span class="stat-label">Assets</span>
          <span class="stat-value">{{ assetCount }}</span>
        </div>
        <div class="stat-chip">
          <span class="stat-label">Wallet Tx</span>
          <span class="stat-value">{{ walletTransactionCount }}</span>
        </div>
        <div class="stat-chip">
          <span class="stat-label">Groups</span>
          <span class="stat-value">{{ transactionGroupCount }}</span>
        </div>
        <div class="stat-chip">
          <span class="stat-label">Tracked</span>
          <span class="stat-value">{{ trackedPositionCount }}</span>
        </div>
      </div>
    </div>
    
    <div v-if="loading" class="loading">
      Loading character data...
    </div>
    
    <div v-else-if="error" class="error">
      {{ error }}
    </div>
    
    <div v-else-if="character" class="character-data">
      <div class="character-header">
        <div class="character-header-content">
          <img 
            v-if="character.id" 
            :src="`https://images.evetech.net/characters/${character.id}/portrait?tenant=tranquility&size=256`" 
            :alt="`${character.characterInfo?.name || 'Character'} portrait`"
            class="character-portrait"
          />
          <div class="character-info-section">
            <h2>{{ character.characterInfo?.name || `Character ${character.id}` }}</h2>
            <div v-if="character.characterInfo" class="character-details">
              <p><strong>Character ID:</strong> {{ character.id }}</p>
              <p v-if="character.characterInfo.corporationId"><strong>Corporation ID:</strong> {{ character.characterInfo.corporationId }}</p>
              <p v-if="character.characterInfo.allianceId"><strong>Alliance ID:</strong> {{ character.characterInfo.allianceId }}</p>
              <p v-if="character.characterInfo.securityStatus !== null && character.characterInfo.securityStatus !== undefined">
                <strong>Security Status:</strong> {{ character.characterInfo.securityStatus.toFixed(2) }}
              </p>
              <p v-if="character.characterInfo.gender"><strong>Gender:</strong> {{ character.characterInfo.gender }}</p>
              <p v-if="character.characterInfo.birthday"><strong>Birthday:</strong> {{ formatDate(character.characterInfo.birthday) }}</p>
            </div>
            <p class="last-updated">Last Updated: {{ formatDate(character.lastUpdated) }}</p>
          </div>
        </div>
      </div>

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
                      <label class="track-switch" :for="`track-${transaction._rowKey}`">
                        <input
                          :id="`track-${transaction._rowKey}`"
                          type="checkbox"
                          :checked="isPositionTracked(transaction._rowKey)"
                          :disabled="isTrackingInFlight(transaction._rowKey)"
                          @change="setPositionTracking(transaction, $event.target.checked)"
                        />
                        <span class="track-slider"></span>
                      </label>
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
            <div class="table-container">
              <table class="wallet-table">
                <thead>
                  <tr>
                    <th class="wallet-item-col">Item</th>
                    <th>Total Amount</th>
                    <th>Average Cost</th>
                    <th>Total Cost</th>
                    <th>Current Market Value</th>
                    <th>Profit</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="group in trackedTransactionGroups" :key="group._rowKey">
                    <td class="wallet-item-col">{{ group.itemName }}</td>
                    <td>{{ formatNumber(group.totalTrackedQuantity) }}</td>
                    <td>{{ formatCurrency(group.averageTrackedPrice) }}</td>
                    <td>{{ formatCurrency(group.totalTrackedAssetPrice) }}</td>
                    <td>{{ formatCurrency(group.totalAssetValue) }}</td>
                    <td :class="profitCellClass(group.totalProfit)">{{ formatCurrency(group.totalProfit) }}</td>
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
      trackedPositions: {},
      trackingInFlight: {}
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
      return Object.entries(this.character?.transactionGroups || {})
        .filter(([, group]) => !!group)
        .map(([typeId, group]) => ({
          ...group,
          itemName: group.itemName || `Type ${typeId}`,
          _rowKey: `${typeId}`
        }))
        .sort((a, b) => (b.totalTrackedAssetPrice || 0) - (a.totalTrackedAssetPrice || 0))
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
    profitCellClass(value) {
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

      this.trackedPositions[rowKey] = isTracked

      this.trackingInFlight[rowKey] = true
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
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 10px;
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
  opacity: 0.9;
  letter-spacing: 0.3px;
}

.overview-stats {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.stat-chip {
  display: flex;
  flex-direction: column;
  min-width: 86px;
  padding: 6px 8px;
  border-radius: 8px;
  border: 1px solid rgba(0, 212, 255, 0.35);
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.12) 0%, rgba(0, 212, 255, 0.06) 100%);
}

.stat-label {
  font-size: 0.7rem;
  letter-spacing: 0.8px;
  text-transform: uppercase;
  color: var(--text-secondary);
}

.stat-value {
  margin-top: 2px;
  font-size: 1.05rem;
  font-weight: 700;
  color: var(--secondary-color);
}

.loading {
  padding: 30px;
  text-align: center;
  color: var(--text-secondary);
  font-size: 1.1em;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 20px rgba(0, 153, 255, 0.2);
  animation: scan 2s ease-in-out infinite;
}

.error {
  padding: 20px;
  text-align: center;
  color: #ff4444;
  background: linear-gradient(135deg, rgba(255, 0, 0, 0.1) 0%, rgba(200, 0, 0, 0.05) 100%);
  border: 2px solid #ff4444;
  border-radius: 8px;
  box-shadow: 0 0 20px rgba(255, 0, 0, 0.2);
  text-shadow: 0 0 10px rgba(255, 0, 0, 0.5);
}

.character-header {
  margin-bottom: 1rem;
  padding: 14px;
  border: 1px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 12px rgba(0, 153, 255, 0.16);
}

.character-header-content {
  display: flex;
  gap: 14px;
  align-items: flex-start;
}

.character-portrait {
  width: 120px;
  height: 120px;
  border-radius: 10px;
  border: 2px solid var(--primary-color);
  box-shadow: 0 0 14px rgba(0, 212, 255, 0.35);
  flex-shrink: 0;
  object-fit: cover;
}

.character-info-section {
  flex: 1;
}

.character-header h2 {
  margin: 0;
  color: var(--secondary-color);
  font-size: 1.35rem;
  line-height: 1.2;
}

.character-details {
  margin: 10px 0;
  padding: 10px 12px;
  background: rgba(0, 0, 0, 0.24);
  border-radius: 8px;
  border-left: 2px solid var(--primary-color);
}

.character-details p {
  margin: 5px 0;
  color: var(--text-secondary);
  font-size: 0.9em;
}

.character-details strong {
  color: var(--primary-color);
  margin-right: 8px;
}

.last-updated {
  color: var(--text-secondary);
  font-style: italic;
  font-size: 0.85em;
  letter-spacing: 0.5px;
  margin-top: 8px;
}

.section {
  margin: 0.75rem 0;
  border: 1px solid var(--accent-blue);
  border-radius: 8px;
  overflow: hidden;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.05) 0%, transparent 100%);
  box-shadow: 0 0 10px rgba(0, 153, 255, 0.08);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 12px;
  cursor: pointer;
  background: linear-gradient(90deg, rgba(0, 153, 255, 0.15) 0%, rgba(0, 212, 255, 0.05) 100%);
  border-bottom: 1px solid rgba(0, 212, 255, 0.3);
  transition: all 0.3s ease;
}

.section-header:hover {
  background: linear-gradient(90deg, rgba(0, 212, 255, 0.2) 0%, rgba(0, 153, 255, 0.1) 100%);
  box-shadow: inset 0 0 10px rgba(0, 212, 255, 0.2);
}

.section-header h3 {
  display: flex;
  align-items: center;
  gap: 8px;
  margin: 0;
  color: var(--secondary-color);
  font-size: 0.98em;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.section-count {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 26px;
  height: 20px;
  padding: 0 6px;
  border-radius: 999px;
  background: rgba(0, 212, 255, 0.16);
  border: 1px solid rgba(0, 212, 255, 0.45);
  color: var(--primary-color);
  font-size: 0.8rem;
  letter-spacing: 0;
  font-weight: 700;
}

.toggle-icon {
  font-size: 1rem;
  transition: transform 0.2s;
  color: var(--primary-color);
}

.section-content {
  padding: 12px;
  background: rgba(0, 0, 0, 0.12);
}

.asset-filters {
  display: flex;
  gap: 10px;
  margin-bottom: 12px;
  padding: 10px;
  background: rgba(0, 0, 0, 0.25);
  border-radius: 6px;
  border: 1px solid rgba(0, 212, 255, 0.28);
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
  color: var(--primary-color);
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.7px;
}

.filter-input,
.filter-select {
  padding: 6px 8px;
  background: rgba(0, 0, 0, 0.3);
  border: 1px solid var(--accent-blue);
  border-radius: 4px;
  color: var(--text-secondary);
  font-family: inherit;
  font-size: 0.88em;
  transition: all 0.3s ease;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: var(--primary-color);
  box-shadow: 0 0 8px rgba(0, 212, 255, 0.4);
  background: rgba(0, 0, 0, 0.4);
}

.filter-select option {
  background: var(--darker-bg);
  color: var(--text-secondary);
}

.table-container {
  overflow-x: auto;
  border: 1px solid rgba(0, 212, 255, 0.22);
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
  background: linear-gradient(90deg, var(--accent-blue), var(--primary-color));
  color: var(--darker-bg);
}

th {
  padding: 8px 10px;
  text-align: left;
  font-weight: 700;
  letter-spacing: 0.6px;
  text-transform: uppercase;
}

td {
  padding: 8px 10px;
  border-bottom: 1px solid rgba(0, 212, 255, 0.2);
  color: var(--text-secondary);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

tr:hover {
  background-color: rgba(0, 212, 255, 0.1);
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
  color: #ff6b6b;
  background: rgba(255, 0, 0, 0.12);
  border: 1px solid rgba(255, 0, 0, 0.45);
}

.wallet-side-sell {
  color: #59d98e;
  background: rgba(0, 180, 0, 0.12);
  border: 1px solid rgba(0, 180, 0, 0.45);
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
  background-color: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(0, 212, 255, 0.35);
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
  background-color: #b9c8d6;
  transition: 0.2s;
  border-radius: 50%;
}

.track-switch input:checked + .track-slider {
  background-color: rgba(0, 212, 255, 0.35);
  border-color: rgba(0, 212, 255, 0.7);
}

.track-switch input:checked + .track-slider:before {
  transform: translateX(18px);
  background-color: var(--secondary-color);
}

.profit-positive {
  color: #44d17a;
  font-weight: 700;
}

.profit-negative {
  color: #ff6b6b;
  font-weight: 700;
}

@media (max-width: 768px) {
  .character {
    padding: 10px;
  }

  .page-header {
    flex-direction: column;
  }

  .overview-stats {
    width: 100%;
  }

  .stat-chip {
    flex: 1;
    min-width: 0;
  }

  .character-header-content {
    flex-direction: column;
    gap: 10px;
  }

  .character-portrait {
    width: 92px;
    height: 92px;
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