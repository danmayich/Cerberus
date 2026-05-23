<template>
  <div class="app-container">
    <aside class="sidebar">
      <div class="rail-top">
        <div class="rail-pill"></div>
        <div class="logo">
          <div class="logo-letter">CB</div>
          <div class="logo-text">CERBERUS</div>
        </div>
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

      <nav class="lcars-nav">
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link v-if="authStore.isAuthenticated" to="/assets" class="nav-link">Assets</router-link>
        <router-link v-if="authStore.isAuthenticated" to="/character" class="nav-link">Character</router-link>
      </nav>

      <div v-if="authStore.isAuthenticated" class="sidebar-footer">
        <button class="logout-button" @click="handleLogout">Logout</button>
      </div>
    </aside>

    <main class="main-content">
      <div class="global-header" aria-hidden="true">
        <div class="global-header-pill global-header-pill-left"></div>
        <div class="global-header-pill global-header-pill-right"></div>
      </div>
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
* {
  box-sizing: border-box;
}

#app {
  font-family: 'Antonio', 'Segoe UI', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: var(--text-primary);
  background-color: var(--darker-bg);
  min-height: 100vh;
}

.app-container {
  display: flex;
  min-height: 100vh;
  background-color: var(--darker-bg);
}

.sidebar {
  width: 272px;
  background: linear-gradient(180deg, #14131f 0%, #0b0b13 100%);
  color: var(--text-primary);
  padding: 20px 16px;
  border-right: 4px solid rgba(240, 181, 122, 0.85);
  box-shadow: inset -14px 0 28px rgba(0, 0, 0, 0.45);
  position: relative;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.rail-top {
  display: grid;
  gap: 12px;
}

.rail-pill {
  height: 44px;
  border-radius: 24px;
  background: linear-gradient(90deg, var(--lcars-bar-b) 0%, var(--lcars-bar-a) 100%);
  box-shadow: inset -10px 0 0 rgba(0, 0, 0, 0.14);
}

.logo-letter {
  font-size: 2.4rem;
  font-weight: 700;
  color: var(--primary-color);
  line-height: 1;
}

.logo-text {
  font-size: 0.84rem;
  font-weight: 700;
  color: var(--text-secondary);
  letter-spacing: 2px;
  margin-top: 4px;
}

.logo {
  padding: 16px;
  border-radius: 24px 10px 10px 24px;
  background: linear-gradient(160deg, rgba(240, 181, 122, 0.18) 0%, rgba(18, 18, 29, 0.92) 70%);
  border: 2px solid rgba(240, 181, 122, 0.5);
}

.lcars-nav {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.sidebar-footer {
  margin-top: auto;
  padding-top: 16px;
  border-top: 2px solid rgba(240, 181, 122, 0.3);
}

.sidebar-character {
  margin-top: 0;
  padding: 10px;
  border: 2px solid rgba(240, 181, 122, 0.45);
  border-radius: 16px;
  background: linear-gradient(155deg, rgba(240, 181, 122, 0.14) 0%, rgba(10, 10, 18, 0.9) 68%);
  display: flex;
  gap: 10px;
  align-items: center;
}

.sidebar-character-portrait {
  width: 62px;
  height: 62px;
  border-radius: 12px;
  object-fit: cover;
  border: 2px solid rgba(240, 181, 122, 0.6);
  flex-shrink: 0;
}

.sidebar-character-content {
  min-width: 0;
}

.sidebar-character-name {
  margin: 0;
  color: var(--primary-color);
  font-size: 0.84rem;
  line-height: 1.15;
  letter-spacing: 0.5px;
}

.sidebar-character-meta {
  margin: 3px 0 0;
  color: var(--text-secondary);
  font-size: 0.68rem;
  line-height: 1.15;
  letter-spacing: 0.3px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.logout-button {
  width: 100%;
  padding: 10px 15px;
  background: linear-gradient(90deg, #ef6e77 0%, #f29b85 100%);
  border: 2px solid rgba(0, 0, 0, 0.35);
  color: #25110f;
  border-radius: 999px;
  font-weight: 700;
  letter-spacing: 1px;
  text-transform: uppercase;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.logout-button:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 16px rgba(239, 110, 119, 0.4);
}

.nav-link {
  display: block;
  color: #2a1610;
  text-decoration: none;
  padding: 10px 14px;
  margin: 0;
  border-radius: 18px 9px 9px 18px;
  border: 2px solid rgba(0, 0, 0, 0.35);
  transition: all 0.3s ease;
  font-weight: 600;
  letter-spacing: 0.8px;
  text-transform: uppercase;
  font-size: 0.78em;
  background: linear-gradient(90deg, var(--lcars-bar-a) 0%, var(--lcars-bar-b) 100%);
}

.nav-link::before {
  content: '●';
  display: inline-block;
  margin-right: 10px;
  font-size: 0.72em;
}

.nav-link:hover {
  transform: translateX(2px);
  filter: brightness(1.04);
}

.router-link-active {
  background: linear-gradient(90deg, var(--lcars-bar-d) 0%, var(--lcars-bar-c) 100%);
  color: #161227;
}

.main-content {
  flex: 1;
  padding: 18px 22px 26px;
  background-color: var(--dark-bg);
  position: relative;
  overflow: auto;
}

.global-header {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 12px;
  align-items: center;
  margin-bottom: 16px;
}

.global-header-pill {
  height: 26px;
  border-radius: 16px;
}

.global-header-pill-left {
  background: linear-gradient(90deg, var(--lcars-bar-a), var(--lcars-bar-b));
}

.global-header-pill-right {
  background: linear-gradient(90deg, var(--lcars-bar-d), var(--lcars-bar-c));
}

@media (max-width: 960px) {
  .sidebar {
    width: 220px;
    padding: 16px 12px;
  }

  .main-content {
    padding: 14px;
  }

  .global-header {
    margin-bottom: 14px;
  }
}

@media (max-width: 760px) {
  .app-container {
    flex-direction: column;
  }

  .sidebar {
    width: 100%;
    border-right: none;
    border-bottom: 4px solid rgba(240, 181, 122, 0.85);
    gap: 10px;
  }

  .lcars-nav {
    flex-direction: row;
    flex-wrap: wrap;
  }

  .nav-link {
    flex: 1;
    min-width: 100px;
    text-align: center;
    border-radius: 16px;
  }

  .global-header {
    grid-template-columns: 1fr;
    gap: 8px;
  }

  .sidebar-character {
    width: 100%;
  }
}
</style>
