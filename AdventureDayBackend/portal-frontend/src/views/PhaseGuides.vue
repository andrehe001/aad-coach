<template>
  <div class="phase-guides">
    <h1>Phase Guides</h1>
    <div class="container-fluid content overflow-auto">
      <ul class="nav nav-dark justify-content-center">
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '1-Deployment' }}">Phase 1</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '2-Change' }}">Phase 2</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '3-Monitoring' }}">Phase 3</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '4-Scale' }}">Phase 4</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '5-Security' }}">Phase 5</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '6-AI' }}">Phase 6</router-link>
        </li>
      </ul>
      <component v-bind:is="currentPhaseGuide"></component>
    </div>
  </div>
</template>

<script>
const availablePhaseGuides = [
  "1-Deployment",
  "2-Change",
  "3-Monitoring",
  "4-Scale",
  "5-Security",
  "6-AI",
];

export default {
  name: "PhaseGuides",
  data() {
    return {
      currentPhaseGuide: null,
    };
  },
  created() {
    this.setPhaseGuide();
  },
  watch: {
    $route: "setPhaseGuide",
  },
  methods: {
    async setPhaseGuide() {
      const requestedPhaseGuide = this.$route.params.name;
      // TODO: get current phase, route to current if name is empty and mark current phase on UI
      // TODO: hide phases in the future

      if (availablePhaseGuides.indexOf(requestedPhaseGuide) != -1) {
        this.currentPhaseGuide = () =>
          import("@/components/phases/" + requestedPhaseGuide + ".vue");
      } else {
        this.currentPhaseGuide = () =>
          import("@/components/phases/1-Deployment.vue");
      }
    },
  },
};
</script>

<style>
.phase-guides .content {
  font-size: 1.2em;
  text-align: justify;
  padding-right: 20px;
}

.tablet {
  width:50%;
  border: 1px solid silver;
  border-radius: 10px;
  margin-top: 0.7rem;
  margin-left: 20px;
  margin-bottom: 20px;
  
  float:right; 
}

.tablet img {
  border: 20px solid #000;
  border-radius: 10px;
  width: 100%;
}
</style>