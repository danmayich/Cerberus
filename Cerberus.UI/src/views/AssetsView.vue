<template>
  <div class="assets">
    <h1>Assets</h1>
    <p class="assets-subtitle">Live manifest of retrieved inventory objects.</p>
    
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
  padding: 22px;
  border: 1px solid var(--border-color);
  border-radius: 12px;
  background: var(--panel-bg);
}

.assets h1 {
  margin-bottom: 6px;
}

.assets-subtitle {
  color: var(--text-secondary);
  margin-bottom: 16px;
}

.loading {
  color: var(--text-secondary);
  font-size: 1rem;
  padding: 14px;
  border: 1px solid var(--border-color);
  border-radius: 10px;
  background: var(--panel-bg-soft);
}

.error {
  color: var(--danger-text);
  margin: 14px 0;
  padding: 14px;
  border: 1px solid var(--danger-border);
  border-radius: 10px;
  background: var(--danger-bg);
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
  border: 1px solid var(--border-color);
  border-radius: 10px;
  background: var(--panel-bg-soft);
  transition: background-color 0.2s ease, border-color 0.2s ease;
  cursor: pointer;
  color: var(--text-secondary);
  font-weight: 500;
}

.asset-item:hover {
  border-color: #52647f;
  background: var(--surface-hover);
  color: var(--text-primary);
}

@media (max-width: 700px) {
  .assets {
    padding: 16px;
  }
}
</style>