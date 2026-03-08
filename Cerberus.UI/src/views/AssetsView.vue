<template>
  <div class="assets">
    <h1>Assets</h1>
    
    <div v-if="loading">Loading assets...</div>
    
    <div v-else-if="error" class="error">
      {{ error }}
    </div>
    
    <div v-else>
      <ul class="assets-list">
        <li v-for="asset in assets" :key="asset.id" class="asset-item">
          {{ asset.name }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import assetsService from '../services/assets.service'

export default {
  name: 'AssetsView',
  data() {
    return {
      assets: [],
      loading: false,
      error: null
    }
  },
  async created() {
    console.log('AssetsView component created')
    try {
      this.loading = true
      console.log('Fetching assets...')
      this.assets = await assetsService.getAll()
      console.log('Assets fetched:', this.assets)
    } catch (err) {
      console.error('Error fetching assets:', err)
      this.error = 'Failed to load assets: ' + err.message
    } finally {
      this.loading = false
    }
  },
  methods: {
    async refreshAssets() {
      try {
        this.loading = true
        this.assets = await assetsService.getAll()
      } catch (err) {
        this.error = 'Failed to refresh assets: ' + err.message
      } finally {
        this.loading = false
      }
    }
  }
}
</script>

<style scoped>
.assets {
  padding: 20px;
}

.assets h1 {
  margin-bottom: 30px;
}

.loading {
  color: var(--text-secondary);
  font-size: 1.1em;
  padding: 20px;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 20px rgba(0, 212, 255, 0.2);
  animation: scan 2s ease-in-out infinite;
}

.error {
  color: #ff4444;
  margin: 20px 0;
  padding: 20px;
  border: 2px solid #ff4444;
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(255, 0, 0, 0.1) 0%, rgba(200, 0, 0, 0.05) 100%);
  box-shadow: 0 0 20px rgba(255, 0, 0, 0.2);
  text-shadow: 0 0 10px rgba(255, 0, 0, 0.5);
}

.assets-list {
  list-style: none;
  padding: 0;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 15px;
}

.asset-item {
  padding: 15px;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 15px rgba(0, 153, 255, 0.2);
  transition: all 0.3s ease;
  cursor: pointer;
  color: var(--text-secondary);
  font-weight: 600;
  letter-spacing: 1px;
}

.asset-item:hover {
  border-color: var(--primary-color);
  background: linear-gradient(135deg, rgba(0, 212, 255, 0.2) 0%, rgba(0, 153, 255, 0.1) 100%);
  box-shadow: 0 0 25px rgba(0, 212, 255, 0.4);
  transform: translateY(-2px);
  color: var(--primary-color);
}
</style>