<template>
  <div class="app-container">
    <div class="sidebar">
      <div class="logo">
        <img alt="Vue logo" src="./assets/logo.png" width="50">
      </div>
      <nav>
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link to="/assets" class="nav-link">Assets</router-link>
        <router-link to="/character" class="nav-link">Character</router-link>
      </nav>
    </div>
    <div class="main-content">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Orbitron:wght@400;700;900&display=swap');

:root {
  --primary-color: #00d4ff;
  --secondary-color: #ffd700;
  --dark-bg: #0a0e27;
  --darker-bg: #050812;
  --accent-blue: #0099ff;
  --accent-cyan: #00ffff;
  --text-primary: #00d4ff;
  --text-secondary: #b0b0b0;
  --border-color: #00d4ff;
}

* {
  box-sizing: border-box;
}

#app {
  font-family: 'Orbitron', 'Courier New', monospace;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: var(--text-primary);
  background-color: var(--darker-bg);
  background-image: 
    radial-gradient(circle at 20% 50%, rgba(0, 212, 255, 0.03) 0%, transparent 50%),
    radial-gradient(circle at 80% 80%, rgba(0, 153, 255, 0.03) 0%, transparent 50%);
}

.app-container {
  display: flex;
  min-height: 100vh;
  background-color: var(--darker-bg);
}

.sidebar {
  width: 250px;
  background: linear-gradient(135deg, var(--darker-bg) 0%, #0f1535 100%);
  color: var(--text-primary);
  padding: 20px;
  border-right: 2px solid var(--accent-blue);
  box-shadow: inset -10px 0 30px rgba(0, 0, 0, 0.5), -5px 0 20px rgba(0, 153, 255, 0.2);
  position: relative;
  overflow: hidden;
}

.sidebar::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(180deg, transparent 0%, rgba(0, 212, 255, 0.03) 50%, transparent 100%);
  pointer-events: none;
}

.logo {
  text-align: center;
  margin-bottom: 30px;
  position: relative;
  z-index: 1;
  padding: 15px;
  border: 2px solid var(--accent-blue);
  border-radius: 8px;
  background: rgba(0, 153, 255, 0.05);
  box-shadow: 0 0 20px rgba(0, 153, 255, 0.2), inset 0 0 20px rgba(0, 212, 255, 0.1);
}

.logo img {
  filter: brightness(0) saturate(100%) invert(1) sepia(1) hue-rotate(180deg) saturate(1.5);
  max-width: 100%;
  height: auto;
}

nav {
  position: relative;
  z-index: 1;
}

.nav-link {
  display: block;
  color: var(--text-primary);
  text-decoration: none;
  padding: 12px 15px;
  margin: 8px 0;
  border-radius: 4px;
  border-left: 3px solid transparent;
  transition: all 0.3s ease;
  font-weight: 600;
  letter-spacing: 2px;
  text-transform: uppercase;
  font-size: 0.85em;
  position: relative;
}

.nav-link::before {
  content: '▶';
  display: inline-block;
  margin-right: 8px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.nav-link:hover {
  background-color: rgba(0, 153, 255, 0.2);
  border-left-color: var(--secondary-color);
  box-shadow: inset 0 0 15px rgba(0, 212, 255, 0.2), 0 0 15px rgba(0, 153, 255, 0.3);
  text-shadow: 0 0 10px var(--accent-blue);
}

.nav-link:hover::before {
  opacity: 1;
}

.router-link-active {
  background: linear-gradient(90deg, rgba(0, 153, 255, 0.3) 0%, rgba(0, 212, 255, 0.1) 100%);
  border-left-color: var(--secondary-color);
  box-shadow: inset 0 0 20px rgba(0, 212, 255, 0.3), 0 0 20px rgba(0, 153, 255, 0.4);
  color: var(--secondary-color);
  text-shadow: 0 0 10px var(--accent-blue);
}

.router-link-active::before {
  opacity: 1;
}

.main-content {
  flex: 1;
  padding: 30px;
  background-color: var(--dark-bg);
  position: relative;
  overflow: auto;
}

.main-content::before {
  content: '';
  position: fixed;
  top: 0;
  right: 0;
  width: 300px;
  height: 300px;
  background: radial-gradient(circle, rgba(0, 212, 255, 0.1) 0%, transparent 70%);
  pointer-events: none;
  z-index: 0;
}

.main-content > * {
  position: relative;
  z-index: 1;
}
</style>
