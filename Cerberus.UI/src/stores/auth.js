import { defineStore } from 'pinia'
import { apiService } from '../services/api.service'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    isChecking: true
  }),

  actions: {
    async checkAuth() {
      this.isChecking = true
      try {
        // Try to hit an authenticated endpoint to check if user has valid session
        await apiService.get('Character')
        this.isAuthenticated = true
      } catch (error) {
        this.isAuthenticated = false
      } finally {
        this.isChecking = false
      }
    },

    setAuthenticated(value) {
      this.isAuthenticated = value
    },

    logout() {
      this.isAuthenticated = false
    }
  }
})
