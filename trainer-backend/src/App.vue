<template>
  <div id="app">
    <!-- <div id="nav">
      <router-link to="/">Home</router-link> |
      <router-link to="/about">About</router-link> |
      <router-link to="/leaderboard">Leaderboard</router-link>
    </div>-->
    <!-- 
    I think we don't need this  
    <div class="no-vertical">
      <div class="main">
        <div class="content">
          <img src="./assets/images/svg/horizontal_mobile.svg" />
          <p>This game is only available in horizontal mode</p>
        </div>
      </div>
      <div class="background"></div>
      <div class="stars-container">
        <div class="stars-item-1"></div>
        <div class="stars-item-2"></div>
        <div class="stars-item-3"></div>
      </div>
    </div> -->

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
  name: 'App',
  components: {
    Footer
  }
}
</script>

<style>
#nav a.router-link-exact-active {
  color: #42b983;
}

@import "assets/css/main.css";
@import "assets/css/reconnect.css";
@import "assets/css/stars.css";
@import "assets/css/controls.css";
@import "assets/css/index.css";
@import "assets/css/challengers.css";
@import "assets/css/battle.css";
@import "assets/css/console.css";
@import "assets/css/mainTitle.css";
@import "assets/css/result.css";
/* @import "assets/css/no-vertical.css"; */
@import "assets/css/not-found.css";
</style>
