<template>
  <div id="app">
    <component v-bind:is="layout"></component>
  </div>
</template>

<script>
import MainLayout from '@/layouts/MainLayout.vue'
import ConsoleLayout from '@/layouts/ConsoleLayout.vue'

const default_layout = 'main';

export default {
  name: 'App',
  computed: {
    layout () {
      return( this.$route.meta.layout || default_layout) + '-layout';
    }
  },
  components: {
    'main-layout': MainLayout,
    'console-layout': ConsoleLayout
  }
}
</script>

<style>
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
@import "assets/css/not-found.css";
</style>
