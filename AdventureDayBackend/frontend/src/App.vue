<template>
  <div id="app">
    <component v-bind:is="layout"></component>
  </div>
</template>

<script>
import MainLayout from '@/layouts/MainLayout.vue'

const default_layout = 'main';

export default {
  name: 'App',
  computed: {
    layout () {
      return( this.$route.meta.layout || default_layout) + '-layout';
    }
  },
  components: {
    'main-layout': MainLayout
  }
}
</script>

<style>
@import "assets/css/main.css";
</style>
