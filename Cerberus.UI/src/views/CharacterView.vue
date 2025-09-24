<template>
  <div class="character">
    <h1>Character Information</h1>
    
    <div v-if="loading" class="loading">
      Loading character data...
    </div>
    
    <div v-else-if="error" class="error">
      {{ error }}
    </div>
    
    <div v-else-if="character" class="character-data">
      <div class="character-header">
        <h2>Character ID: {{ character.id }}</h2>
        <p class="last-updated">Last Updated: {{ formatDate(character.lastUpdated) }}</p>
      </div>

      <!-- Assets Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('assets')">
          <h3>Assets ({{ character.assets.length }})</h3>
          <span class="toggle-icon">{{ sectionStates.assets ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.assets" class="section-content">
          <div class="table-container" v-if="character.assets.length > 0">
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Type</th>
                  <th>Location</th>
                  <th>Quantity</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="asset in character.assets" :key="asset.id">
                  <td>{{ asset.id }}</td>
                  <td>{{ asset.type }}</td>
                  <td>{{ asset.location }}</td>
                  <td>{{ asset.quantity }}</td>
                </tr>
              </tbody>
            </table>
          </div>
          <p v-else>No assets found</p>
        </div>
      </div>

      <!-- Wallet Transactions Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('walletTransactions')">
          <h3>Wallet Transactions ({{ Object.keys(character.walletTransactions).length }})</h3>
          <span class="toggle-icon">{{ sectionStates.walletTransactions ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.walletTransactions" class="section-content">
          <div class="table-container" v-if="Object.keys(character.walletTransactions).length > 0">
            <table>
              <thead>
                <tr>
                  <th>Transaction ID</th>
                  <th>Details</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(transaction, id) in character.walletTransactions" :key="id">
                  <td>{{ id }}</td>
                  <td>{{ JSON.stringify(transaction) }}</td>
                </tr>
              </tbody>
            </table>
          </div>
          <p v-else>No wallet transactions found</p>
        </div>
      </div>

      <!-- Transaction Groups Section -->
      <div class="section">
        <div class="section-header" @click="toggleSection('transactionGroups')">
          <h3>Transaction Groups ({{ Object.keys(character.transactionGroups).length }})</h3>
          <span class="toggle-icon">{{ sectionStates.transactionGroups ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.transactionGroups" class="section-content">
          <div v-if="Object.keys(character.transactionGroups).length > 0" class="transaction-groups-container">
            <TransactionGroupDetails
              v-for="(group, id) in character.transactionGroups"
              :key="id"
              :group="group"
            />
          </div>
          <p v-else>No transaction groups found</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import characterService from '../services/character.service'
import TransactionGroupDetails from '../components/TransactionGroupDetails.vue'

export default {
  name: 'CharacterView',
  components: {
    TransactionGroupDetails
  },
  data() {
    return {
      character: null,
      loading: false,
      error: null,
      sectionStates: {
        assets: false,
        walletTransactions: false,
        transactionGroups: false
      }
    }
  },
  methods: {
    toggleSection(sectionName) {
      this.sectionStates[sectionName] = !this.sectionStates[sectionName]
    },
    formatDate(dateString) {
      return new Date(dateString).toLocaleString()
    },
    async fetchCharacterData() {
      this.loading = true
      this.error = null
      
      try {
        this.character = await characterService.getCharacter()
      } catch (err) {
        this.error = 'Failed to load character data: ' + err.message
        console.error('Character loading error:', err)
      } finally {
        this.loading = false
      }
    }
  },
  created() {
    this.fetchCharacterData()
  }
}
</script>

<style scoped>
.character {
  padding: 20px;
}

.loading, .error {
  padding: 20px;
  text-align: center;
}

.error {
  color: red;
  background-color: #ffe6e6;
  border-radius: 4px;
}

.character-header {
  margin-bottom: 2rem;
}

.last-updated {
  color: #666;
  font-style: italic;
}

.section {
  margin: 2rem 0;
  border: 1px solid #eee;
  border-radius: 4px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  cursor: pointer;
  background-color: #f8f9fa;
  transition: background-color 0.2s;
}

.section-header:hover {
  background-color: #e9ecef;
}

.section-content {
  padding: 1rem;
}

.toggle-icon {
  font-size: 1.2rem;
  transition: transform 0.2s;
}

.table-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

th, td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f5f5f5;
  font-weight: bold;
}

tr:hover {
  background-color: #f9f9f9;
}

.transaction-groups-container {
  display: grid;
  gap: 1rem;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
}
</style>