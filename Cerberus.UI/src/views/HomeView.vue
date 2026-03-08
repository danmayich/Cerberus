<template>
  <div class="home">
    <h1>Home</h1>
    <p>Welcome to the home page!</p>
    
    <div class="login-container">
      <button 
        @click="handleLogin" 
        class="login-button"
        :disabled="isLoading"
      >
        {{ isLoading ? 'Logging in...' : 'Log In' }}
      </button>
      
      <div v-if="error" class="error-message">
        {{ error }}
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/auth.service'

export default {
  name: 'HomeView',
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
  padding: 40px;
  text-align: center;
  max-width: 600px;
  margin: 0 auto;
}

.home h1 {
  font-size: 3em;
  margin-bottom: 40px;
  animation: glow 2s ease-in-out infinite;
}

.home p {
  font-size: 1.1em;
  color: var(--text-secondary);
  margin-bottom: 40px;
  letter-spacing: 1px;
}

.login-container {
  margin-top: 3rem;
  padding: 30px;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: linear-gradient(135deg, rgba(0, 153, 255, 0.1) 0%, rgba(0, 212, 255, 0.05) 100%);
  box-shadow: 0 0 30px rgba(0, 212, 255, 0.2), inset 0 0 20px rgba(0, 212, 255, 0.05);
}

.login-button {
  min-width: 200px;
  font-size: 1.1em;
  letter-spacing: 2px;
}

.error-message {
  margin-top: 1.5rem;
  animation: scan 2s ease-in-out infinite;
}
</style>