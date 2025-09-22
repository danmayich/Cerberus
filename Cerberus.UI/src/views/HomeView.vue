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
    async handleLogin() {
      this.isLoading = true
      this.error = null
      
      try {
        await authService.login()
      } catch (err) {
        this.error = 'Failed to initiate login: ' + err.message
        console.error('Login error:', err)
      } finally {
        this.isLoading = false
      }
    }
  }
}
</script>

<style scoped>
.home {
  padding: 20px;
  text-align: center;
}

.login-container {
  margin-top: 2rem;
}

.login-button {
  background-color: #4CAF50;
  color: white;
  padding: 12px 24px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.login-button:hover {
  background-color: #45a049;
}

.login-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.error-message {
  color: #ff0000;
  margin-top: 1rem;
  padding: 10px;
  border-radius: 4px;
  background-color: #ffe6e6;
}
</style>