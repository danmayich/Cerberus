import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/HomeView.vue'
import Assets from '../views/AssetsView.vue'
import Character from '../views/CharacterView.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/assets',
    name: 'Assets',
    component: Assets
  },
  {
    path: '/character',
    name: 'Character',
    component: Character
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router