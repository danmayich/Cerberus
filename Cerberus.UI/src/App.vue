<template>
  <div class="app-container">
    <div class="sidebar">
      <div class="logo">
        <img alt="Vue logo" src="./assets/logo.png" width="50">
      </div>
      <nav>
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link to="/assets" class="nav-link">Assets</router-link>
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
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

.app-container {
  display: flex;
  min-height: 100vh;
}

.sidebar {
  width: 250px;
  background-color: #2c3e50;
  color: white;
  padding: 20px;
}

.logo {
  text-align: center;
  margin-bottom: 30px;
}

.nav-link {
  display: block;
  color: white;
  text-decoration: none;
  padding: 10px;
  margin: 5px 0;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.nav-link:hover {
  background-color: #34495e;
}

.router-link-active {
  background-color: #34495e;
}

.main-content {
  flex: 1;
  padding: 20px;
}
</style>
