<template>
  <div class="home">
    <h1>Cerberus Console</h1>
    <p class="home-subtitle">Authenticate to open operations, assets, and character telemetry.</p>

    <div v-if="!authStore.isAuthenticated" class="login-container">
      <button 
        @click="handleLogin" 
        class="login-button"
        :disabled="isLoading"
      >
        {{ isLoading ? 'Linking Identity...' : 'Initiate Login' }}
      </button>
      
      <div v-if="error" class="error-message">
        {{ error }}
      </div>
    </div>

    <div v-else class="status-card">
      <span class="status-dot"></span>
      Session active. Use the left rail to continue.
    </div>
  </div>
</template>

<script>
import authService from '../services/auth.service'
import { useAuthStore } from '../stores/auth'

export default {
  name: 'HomeView',
  setup() {
    const authStore = useAuthStore()
    return { authStore }
  },
  data() {
    return {
      isLoading: false,
      error: null
    }
  },
  methods: {
    handleLogin() {
      this.isLoading = true;
      // Simply redirect to the login endpoint
      authService.login();
      // No need for error handling here as we're doing a direct navigation
    }
  }
}
</script>

<style scoped>
.home {
  padding: 24px;
  text-align: left;
  max-width: 760px;
  margin: 0 auto;
  border: 1px solid var(--border-color);
  border-radius: 12px;
  background: var(--panel-bg);
}

.home h1 {
  font-size: 1.8rem;
  margin-bottom: 8px;
}

.home-subtitle {
  margin: 0 0 20px;
  color: var(--text-secondary);
  letter-spacing: 0.4px;
}

.login-container {
  margin-top: 1.5rem;
  padding: 16px;
  border: 1px solid var(--border-color);
  border-radius: 10px;
  background: var(--panel-bg-soft);
}

.login-button {
  min-width: 200px;
  font-size: 1rem;
}

.error-message {
  margin-top: 1.5rem;
}

.status-card {
  margin-top: 14px;
  padding: 12px 16px;
  border-radius: 10px;
  border: 1px solid var(--success-border);
  color: var(--success-text);
  display: flex;
  align-items: center;
  gap: 10px;
  background: var(--success-bg);
}

.status-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background: var(--ok-color);
}

@media (max-width: 700px) {
  .home {
    padding: 16px;
  }

  .home h1 {
    font-size: 1.8em;
  }
}
</style>