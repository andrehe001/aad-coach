import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    redirect: '/leaderboard'
  },
  {
    path: '/code-of-conduct',
    name: 'Code of Conduct',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "code-of-conduct" */ '../views/CodeOfConduct.vue')
  },
  {
    path: '/leaderboard',
    name: 'Leaderboard',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "leaderboard" */ '../views/Leaderboard.vue')
  },
  {
    path: '/team-console',
    name: 'Team Console',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "team-console" */ '../views/TeamConsole.vue')
  },
  {
    path: '/phase-guides',
    name: 'Phase Guides',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "phase-guides" */ '../views/PhaseGuides.vue')
  },
  {
    path: '/team-accounts',
    name: 'Your Accounts',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "team-accounts" */ '../views/TeamAccounts.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
