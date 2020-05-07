import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/leaderboard',
    name: 'Leaderboard',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "leaderboard" */ '../views/Leaderboard.vue')
  },
  {
    path: '/team-details',
    name: 'Team Details',
    meta: { layout: 'console' },
    component: () => import(/* webpackChunkName: "team-details" */ '../views/TeamDetails.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
