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
          <h3>Assets ({{ character.assets.length }})</h3>
          <span class="toggle-icon">{{ sectionStates.assets ? '▼' : '▶' }}</span>
        </div>
        <div v-show="sectionStates.assets" class="section-content">
          <div class="table-container" v-if="character.assets.length > 0">
            <table>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Location</th>
                  <th>Quantity</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="asset in character.assets" :key="asset.id">
                  <td>{{ asset.name || asset.id }}</td>
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
          <div v-if="Object.keys(character.walletTransactions).length > 0" class="wallet-transactions-container">
            <WalletTransactionDetails
              v-for="(transaction, id) in character.walletTransactions"
              :key="id"
              :transaction="transaction"
            />
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
import { useCharacterStore } from '../stores/character'
import TransactionGroupDetails from '../components/TransactionGroupDetails.vue'
import WalletTransactionDetails from '../components/WalletTransactionDetails.vue'

export default {
  name: 'CharacterView',
  components: {
    TransactionGroupDetails,
    WalletTransactionDetails
  },
  setup() {
    const characterStore = useCharacterStore()
    return { characterStore }
  },
  data() {
    return {
      sectionStates: {
        assets: false,
        walletTransactions: false,
        transactionGroups: false
      }
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
  padding: 20px;
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
  margin-bottom: 2rem;
  padding: 20px;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 20px rgba(0, 153, 255, 0.2);
}

.character-header-content {
  display: flex;
  gap: 20px;
  align-items: flex-start;
}

.character-portrait {
  width: 150px;
  height: 150px;
  border-radius: 8px;
  border: 3px solid var(--primary-color);
  box-shadow: 0 0 20px rgba(0, 212, 255, 0.5);
  flex-shrink: 0;
  object-fit: cover;
}

.character-info-section {
  flex: 1;
}

.character-header h2 {
  margin-bottom: 15px;
  color: var(--secondary-color);
}

.character-details {
  margin: 15px 0;
  padding: 15px;
  background: rgba(0, 0, 0, 0.2);
  border-radius: 4px;
  border-left: 3px solid var(--primary-color);
}

.character-details p {
  margin: 8px 0;
  color: var(--text-secondary);
  font-size: 0.95em;
}

.character-details strong {
  color: var(--primary-color);
  margin-right: 8px;
}

.last-updated {
  color: var(--text-secondary);
  font-style: italic;
  font-size: 0.95em;
  letter-spacing: 1px;
  margin-top: 15px;
}

.section {
  margin: 2rem 0;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  overflow: hidden;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.05) 0%, transparent 100%);
  box-shadow: 0 0 15px rgba(0, 153, 255, 0.1);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  cursor: pointer;
  background: linear-gradient(90deg, rgba(0, 153, 255, 0.15) 0%, rgba(0, 212, 255, 0.05) 100%);
  border-bottom: 1px solid var(--accent-blue);
  transition: all 0.3s ease;
}

.section-header:hover {
  background: linear-gradient(90deg, rgba(0, 212, 255, 0.2) 0%, rgba(0, 153, 255, 0.1) 100%);
  box-shadow: inset 0 0 10px rgba(0, 212, 255, 0.2);
}

.section-header h3 {
  margin: 0;
  color: var(--secondary-color);
  font-size: 1.1em;
  text-transform: uppercase;
  letter-spacing: 2px;
}

.toggle-icon {
  font-size: 1.2rem;
  transition: transform 0.2s;
  color: var(--primary-color);
}

.section-content {
  padding: 20px;
}

.table-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

thead {
  background: linear-gradient(90deg, var(--accent-blue), var(--primary-color));
  color: var(--darker-bg);
}

th {
  padding: 12px;
  text-align: left;
  font-weight: 700;
  letter-spacing: 1px;
  text-transform: uppercase;
}

td {
  padding: 12px;
  border-bottom: 1px solid rgba(0, 212, 255, 0.2);
  color: var(--text-secondary);
}

tr:hover {
  background-color: rgba(0, 212, 255, 0.1);
}

.transaction-groups-container {
  display: grid;
  gap: 1.5rem;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
}

.wallet-transactions-container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>