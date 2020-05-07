<template>
  <div id="main">
    <!-- <div id="nav">
      <router-link to="/">Home</router-link> |
      <router-link to="/about">About</router-link> |
      <router-link to="/leaderboard">Leaderboard</router-link>
    </div>-->

    <div class="stars-container">
      <div class="stars-item-1"></div>
      <div class="stars-item-2"></div>
      <div class="stars-item-3"></div>
    </div>

    <router-view />

    <Footer />
  </div>
</template>

<script>
import Footer from '@/components/Footer.vue'

export default {
  name: 'MainLayout',
  components: {
    Footer
  }
}
</script>

<style>
#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
