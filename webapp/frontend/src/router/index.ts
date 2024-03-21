import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

export type Modules = { weather: any };

const initializeRouter = (modules: Modules) => createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      props: {
        weatherModule: modules.weather
      }
    }
  ]
})

export default initializeRouter
