<template>
  <div class="phase-guides">
    <h1>Phase Guides</h1>
    <div class="container-fluid content overflow-auto">
      <ul class="nav nav-dark justify-content-center">
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides'}">Intro</router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '1-Deployment' }}">Phase 1</router-link>
        </li>
        <li class="nav-item" v-if="currentPhase >= 2">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '2-Change' }}">Phase 2</router-link>
        </li>
        <li class="nav-item" v-if="currentPhase >= 3">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '3-Monitoring' }}">Phase 3</router-link>
        </li>
        <li class="nav-item" v-if="currentPhase >= 4">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '4-Scale' }}">Phase 4</router-link>
        </li>
        <li class="nav-item" v-if="currentPhase >= 5">
          <router-link class="nav-link" :to="{ name: 'PhaseGuides', params: { name: '5-Security' }}">Phase 5</router-link>
        </li>
        <li class="nav-item" v-if="currentPhase >= 6">
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
      timer: "",
      currentPhase : 1,
      currentPhaseGuide: null,
    };
  },
  created() {
    const fetchIntervalMs = 10 * 1000;

    this.fetchCurrentPhase()
      .then(() => this.setPhaseGuideBasedOnRoute());
    this.timer = setInterval(this.fetchCurrentPhase, fetchIntervalMs);
  },
  watch: {
    $route: "setPhaseGuideBasedOnRoute",
  },
  methods: {
    fetchCurrentPhase() {
      return this.$http
        .get("Runner/phase")
        .then((response) => {
          this.currentPhase = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
    async setPhaseGuideBasedOnRoute() {
      const requestedPhaseGuide = this.$route.params.name;
      if (availablePhaseGuides.indexOf(requestedPhaseGuide) != -1 && 
          requestedPhaseGuide.charAt(0) <= this.currentPhase) {
        this.currentPhaseGuide = () =>
          import("@/components/phases/" + requestedPhaseGuide + ".vue");
      } else {
        this.currentPhaseGuide = () =>
          import("@/components/phases/0-Intro.vue");
      }
    },
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
};
</script>

<style>
.phase-guides .content {
  font-size: 1.2em;
  text-align: justify;
  padding-right: 20px;
}

.phase-guides button.btn {
  margin-left: 5px;
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

.full-image img {
  border: 1px solid #000;
  border-radius: 10px;
  width: 100%;
}
</style>