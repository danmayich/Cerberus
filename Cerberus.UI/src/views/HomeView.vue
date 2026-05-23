<template>
  <div class="home lcars-home-frame">
    <div class="home-bars" aria-hidden="true">
      <div class="bar bar-a"></div>
      <div class="bar bar-b"></div>
    </div>

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
  padding: 26px;
  text-align: left;
  max-width: 760px;
  margin: 0 auto;
}

.lcars-home-frame {
  border: 2px solid rgba(240, 181, 122, 0.55);
  border-radius: 26px;
  background: linear-gradient(160deg, rgba(240, 181, 122, 0.1) 0%, rgba(10, 10, 18, 0.94) 54%);
  box-shadow: 0 14px 30px rgba(0, 0, 0, 0.35);
}

.home-bars {
  display: flex;
  gap: 10px;
  margin-bottom: 14px;
}

.bar {
  height: 16px;
  border-radius: 10px;
}

.bar-a {
  flex: 2;
  background: linear-gradient(90deg, var(--lcars-bar-a), var(--lcars-bar-b));
}

.bar-b {
  flex: 1;
  background: linear-gradient(90deg, var(--lcars-bar-d), var(--lcars-bar-c));
}

.home h1 {
  font-size: 2.3em;
  margin-bottom: 8px;
}

.home-subtitle {
  margin: 0 0 20px;
  color: var(--text-secondary);
  letter-spacing: 0.4px;
}

.login-container {
  margin-top: 1.5rem;
  padding: 20px;
  border: 2px solid rgba(240, 181, 122, 0.45);
  border-radius: 18px;
  background: rgba(10, 10, 18, 0.74);
}

.login-button {
  min-width: 200px;
  font-size: 1em;
  letter-spacing: 1px;
}

.error-message {
  margin-top: 1.5rem;
  animation: scan 2s ease-in-out infinite;
}

.status-card {
  margin-top: 14px;
  padding: 12px 16px;
  border-radius: 14px;
  border: 2px solid rgba(135, 208, 168, 0.45);
  color: #b6f3cb;
  display: flex;
  align-items: center;
  gap: 10px;
  background: rgba(8, 14, 10, 0.7);
}

.status-dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: var(--ok-color);
  box-shadow: 0 0 10px rgba(135, 208, 168, 0.6);
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