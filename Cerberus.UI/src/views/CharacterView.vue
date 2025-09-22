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
        <h3>Assets ({{ character.assets.length }})</h3>
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

      <!-- Wallet Transactions Section -->
      <div class="section">
        <h3>Wallet Transactions ({{ Object.keys(character.walletTransactions).length }})</h3>
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

      <!-- Transaction Groups Section -->
      <div class="section">
        <h3>Transaction Groups ({{ Object.keys(character.transactionGroups).length }})</h3>
        <div class="table-container" v-if="Object.keys(character.transactionGroups).length > 0">
          <table>
            <thead>
              <tr>
                <th>Group ID</th>
                <th>Details</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(group, id) in character.transactionGroups" :key="id">
                <td>{{ id }}</td>
                <td>{{ JSON.stringify(group) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <p v-else>No transaction groups found</p>
      </div>
    </div>
  </div>
</template>

<script>
import characterService from '../services/character.service'

export default {
  name: 'CharacterView',
  data() {
    return {
      character: null,
      loading: false,
      error: null
    }
  },
  methods: {
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
  padding: 1rem;
  border: 1px solid #eee;
  border-radius: 4px;
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
</style>