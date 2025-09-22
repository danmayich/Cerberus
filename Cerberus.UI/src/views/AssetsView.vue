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

.error {
  color: red;
  margin: 20px 0;
}

.assets-list {
  list-style: none;
  padding: 0;
}

.asset-item {
  padding: 10px;
  border-bottom: 1px solid #eee;
}

.asset-item:hover {
  background-color: #f5f5f5;
}
</style>