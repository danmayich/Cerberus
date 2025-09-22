import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/HomeView.vue'
import Assets from '../views/AssetsView.vue'

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
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router