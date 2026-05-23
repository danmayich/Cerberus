<template>
  <div class="app-container">
    <aside class="sidebar">
      <div class="logo">
        <div class="logo-letter">CB</div>
        <div class="logo-text">Cerberus</div>
      </div>

      <div v-if="sidebarCharacter" class="sidebar-character">
        <img
          v-if="sidebarCharacter.id"
          :src="`https://images.evetech.net/characters/${sidebarCharacter.id}/portrait?tenant=tranquility&size=256`"
          :alt="`${sidebarCharacter.name || 'Character'} portrait`"
          class="sidebar-character-portrait"
        />
        <div class="sidebar-character-content">
          <p class="sidebar-character-name">{{ sidebarCharacter.name || `Character ${sidebarCharacter.id}` }}</p>
          <p class="sidebar-character-meta">ID {{ sidebarCharacter.id }}</p>
          <p v-if="sidebarCharacter.corporationId" class="sidebar-character-meta">Corp {{ sidebarCharacter.corporationId }}</p>
          <p v-if="sidebarCharacter.allianceId" class="sidebar-character-meta">Alliance {{ sidebarCharacter.allianceId }}</p>
        </div>
      </div>

      <nav class="sidebar-nav">
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link v-if="authStore.isAuthenticated" to="/assets" class="nav-link">Assets</router-link>
        <router-link v-if="authStore.isAuthenticated" to="/character" class="nav-link">Character</router-link>
      </nav>

      <div v-if="authStore.isAuthenticated" class="sidebar-footer">
        <button class="logout-button" @click="handleLogout">Logout</button>
      </div>
    </aside>

    <main class="main-content">
      <router-view></router-view>
    </main>
  </div>
</template>

<script>
import { computed } from 'vue'
import authService from './services/auth.service'
import { useCharacterStore } from './stores/character'
import { useAuthStore } from './stores/auth'

export default {
  name: 'App',
  setup() {
    const authStore = useAuthStore()
    const characterStore = useCharacterStore()
    
    // Check if we already have character data (e.g., from cache)
    if (characterStore.hasCharacter) {
      authStore.setAuthenticated(true)
    }
    
    const sidebarCharacter = computed(() => {
      const character = characterStore.character
      if (!character || !character.id) {
        return null
      }

      return {
        id: character.id,
        name: character.characterInfo?.name,
        corporationId: character.characterInfo?.corporationId,
        allianceId: character.characterInfo?.allianceId
      }
    })

    return { authStore, sidebarCharacter }
  },
  methods: {
    handleLogout() {
      // Clear client-side state before logout
      const characterStore = useCharacterStore()
      characterStore.clearCharacter()
      this.authStore.logout()
      authService.logout()
    }
  }
}
</script>

<style>
#app {
  color: var(--text-primary);
  min-height: 100vh;
}

.app-container {
  display: flex;
  min-height: 100vh;
  background-color: var(--darker-bg);
}

.sidebar {
  width: 260px;
  background: var(--panel-bg);
  color: var(--text-primary);
  padding: 16px;
  border-right: 1px solid var(--border-color);
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.logo {
  padding: 10px 12px;
  border-radius: 10px;
  border: 1px solid var(--border-color);
  background: var(--panel-bg-soft);
}

.logo-letter {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--text-primary);
  line-height: 1;
}

.logo-text {
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-secondary);
  letter-spacing: 0.06em;
  margin-top: 2px;
  text-transform: uppercase;
}

.sidebar-nav {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.nav-link {
  display: block;
  color: var(--text-secondary);
  text-decoration: none;
  padding: 8px 10px;
  border-radius: 8px;
  border: 1px solid transparent;
  font-weight: 500;
}

.nav-link:hover {
  background: var(--panel-bg-soft);
  color: var(--text-primary);
  border-color: var(--border-color);
}

.router-link-active {
  background: #eef2ff;
  color: #1e3a8a;
  border-color: #c7d2fe;
}

.sidebar-character {
  padding: 10px;
  border: 1px solid var(--border-color);
  border-radius: 10px;
  background: var(--panel-bg-soft);
  display: flex;
  gap: 10px;
  align-items: center;
}

.sidebar-character-portrait {
  width: 56px;
  height: 56px;
  border-radius: 8px;
  object-fit: cover;
  border: 1px solid var(--border-color);
  flex-shrink: 0;
}

.sidebar-character-content {
  min-width: 0;
}

.sidebar-character-name {
  margin: 0;
  color: var(--text-primary);
  font-size: 0.85rem;
  line-height: 1.15;
}

.sidebar-character-meta {
  margin: 3px 0 0;
  color: var(--text-secondary);
  font-size: 0.72rem;
  line-height: 1.15;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.sidebar-footer {
  margin-top: auto;
  padding-top: 10px;
  border-top: 1px solid var(--border-color);
}

.logout-button {
  width: 100%;
}

.main-content {
  flex: 1;
  padding: 18px;
  background-color: var(--dark-bg);
  overflow: auto;
}

@media (max-width: 900px) {
  .sidebar {
    width: 220px;
  }

  .main-content {
    padding: 12px;
  }
}

@media (max-width: 760px) {
  .app-container {
    flex-direction: column;
  }

  .sidebar {
    width: 100%;
    border-right: none;
    border-bottom: 1px solid var(--border-color);
  }

  .sidebar-nav {
    flex-direction: row;
    flex-wrap: wrap;
  }

  .nav-link {
    flex: 1;
    min-width: 96px;
    text-align: center;
  }
}
</style>
