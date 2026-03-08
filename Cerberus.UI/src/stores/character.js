import { defineStore } from 'pinia'
import characterService from '../services/character.service'
import { useAuthStore } from './auth'

export const useCharacterStore = defineStore('character', {
  state: () => ({
    character: null,
    loading: false,
    error: null,
    lastFetched: null
  }),

  getters: {
    hasCharacter: (state) => state.character !== null,
    shouldRefetch: (state) => {
      if (!state.lastFetched) return true
      // Refetch if data is older than 5 minutes
      const fiveMinutes = 5 * 60 * 1000
      return Date.now() - state.lastFetched > fiveMinutes
    }
  },

  actions: {
    async fetchCharacter(force = false) {
      // If we have data and don't need to refresh, return cached data
      if (!force && this.character && !this.shouldRefetch) {
        return this.character
      }

      this.loading = true
      this.error = null

      try {
        this.character = await characterService.getCharacter()
        this.lastFetched = Date.now()
        
        // Mark user as authenticated after successful character fetch
        const authStore = useAuthStore()
        authStore.setAuthenticated(true)
        
        return this.character
      } catch (error) {
        this.error = 'Failed to load character data: ' + error.message
        console.error('Character loading error:', error)
        throw error
      } finally {
        this.loading = false
      }
    },

    clearCharacter() {
      this.character = null
      this.lastFetched = null
      this.error = null
    }
  }
})
