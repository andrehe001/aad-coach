import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const defaultRequiresAuth = true;

const routes = [
  {
    path: '/',
    name: 'Home',
    redirect: '/leaderboard'
  },
  {
    path: '/login',
    name: 'Login',
    meta: { layout: 'main', guest: true },
    component: () => import(/* webpackChunkName: "login" */ '../views/Login.vue')
  },
  {
    path: '/code-of-conduct',
    name: 'Code of Conduct',
    meta: { layout: 'main' },
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
    path: '/phase-guides/:name?',
    name: 'PhaseGuides',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "phase-guides" */ '../views/PhaseGuides.vue')
  },
  {
    path: '/team-accounts',
    name: 'Your Accounts',
    meta: { layout: 'main' },
    component: () => import(/* webpackChunkName: "team-accounts" */ '../views/TeamAccounts.vue')
  },
  {
    path: '/administration',
    name: 'Administration',
    meta: { layout: 'main', is_admin: true },
    component: () => import(/* webpackChunkName: "administration" */ '../views/Administration.vue')
  }
]

const router = new VueRouter({
  routes
})

router.beforeEach((to, from, next) => {
  if(to.path == '/logout') {
    localStorage.clear();
    next({ path: '/'});
    router.go();
  } else if(to.matched.some(record => record.meta.requiresAuth) || (defaultRequiresAuth && !to.matched.some(record => record.meta.guest))) {
      if (localStorage.getItem('jwt') == null) {
          next({
              path: '/login',
              params: { nextUrl: to.fullPath }
          })
      } else {
          let user = JSON.parse(localStorage.getItem('user'))
          if(to.matched.some(record => record.meta.is_admin)) {
              if(user.is_admin == 1){
                  next()
              }
              else{
                  next({ path: '/leaderboard'})
              }
          }else {
              next()
          }
      }
  } else if(to.matched.some(record => record.meta.guest)) {
      if(localStorage.getItem('jwt') == null){
          next()
      }
      else{
          next({ path: '/leaderboard'})
      }
  }else {
      next()
  }
})

export default router
