<template>
  <div class="assets lcars-assets-frame">
    <div class="assets-header-bars" aria-hidden="true">
      <div class="assets-bar assets-bar-a"></div>
      <div class="assets-bar assets-bar-b"></div>
      <div class="assets-bar assets-bar-c"></div>
    </div>

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
}

.lcars-assets-frame {
  border: 2px solid rgba(240, 181, 122, 0.55);
  border-radius: 26px;
  background: linear-gradient(160deg, rgba(135, 184, 255, 0.08) 0%, rgba(10, 10, 18, 0.95) 52%);
  box-shadow: 0 14px 30px rgba(0, 0, 0, 0.34);
}

.assets h1 {
  margin-bottom: 6px;
}

.assets-subtitle {
  color: var(--text-secondary);
  margin-bottom: 16px;
}

.assets-header-bars {
  display: grid;
  grid-template-columns: 3fr 1fr 2fr;
  gap: 10px;
  margin-bottom: 14px;
}

.assets-bar {
  height: 14px;
  border-radius: 8px;
}

.assets-bar-a {
  background: linear-gradient(90deg, var(--lcars-bar-a), var(--lcars-bar-b));
}

.assets-bar-b {
  background: linear-gradient(90deg, var(--lcars-bar-d), var(--lcars-bar-c));
}

.assets-bar-c {
  background: linear-gradient(90deg, var(--lcars-bar-b), var(--lcars-bar-a));
}

.loading {
  color: var(--text-secondary);
  font-size: 1.1em;
  padding: 18px;
  border: 2px solid rgba(240, 181, 122, 0.4);
  border-radius: 16px;
  background: rgba(10, 10, 18, 0.8);
  animation: scan 2s ease-in-out infinite;
}

.error {
  color: #ffb3b8;
  margin: 20px 0;
  padding: 20px;
  border: 2px solid var(--alert-color);
  border-radius: 16px;
  background: linear-gradient(145deg, rgba(239, 110, 119, 0.14) 0%, rgba(12, 8, 10, 0.8) 88%);
  box-shadow: 0 10px 22px rgba(239, 110, 119, 0.22);
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
  border: 2px solid rgba(240, 181, 122, 0.45);
  border-radius: 18px 10px 10px 18px;
  background: linear-gradient(145deg, rgba(240, 181, 122, 0.14) 0%, rgba(18, 18, 28, 0.95) 68%);
  box-shadow: 0 10px 18px rgba(0, 0, 0, 0.3);
  transition: all 0.3s ease;
  cursor: pointer;
  color: var(--text-secondary);
  font-weight: 600;
  letter-spacing: 0.5px;
}

.asset-item:hover {
  border-color: var(--secondary-color);
  background: linear-gradient(145deg, rgba(233, 133, 109, 0.22) 0%, rgba(24, 16, 14, 0.98) 70%);
  box-shadow: 0 14px 24px rgba(233, 133, 109, 0.3);
  transform: translateY(-2px);
  color: var(--text-primary);
}

@media (max-width: 700px) {
  .assets {
    padding: 16px;
  }
}
</style>