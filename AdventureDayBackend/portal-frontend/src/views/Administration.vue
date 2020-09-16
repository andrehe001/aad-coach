<template>
  <div class="administration">
    <h1>Administration</h1>
    <div class="content overflow-auto">
      <h2>Phases:</h2>
      <ul class="nav nav-dark nav-pills">
        <li class="nav-item" v-for="phase in availablePhases" v-bind:key="phase">
          <a
            class="nav-link"
            :class="{ 'disabled': phase.startsWith('Phase'+currentPhase) }"
            href="#"
            @click="changePhase(phase, $event)"
          >{{phase | formatPhase}}</a>
        </li>
      </ul>
      <h2>Actions:</h2>
      <ul class="nav nav-dark nav-pills">
        <li class="nav-item">
          <a class="nav-link" v-if="currentRunnerStatus != 'Started'" href="#" @click="start">Start</a>
          <a class="nav-link disabled" v-if="currentRunnerStatus == 'Started'">Started</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" v-if="currentRunnerStatus != 'Stopped'" href="#" @click="stop">Stop</a>
          <a class="nav-link disabled" v-if="currentRunnerStatus == 'Stopped'" href="#">Stopped</a>
        </li>
      </ul>
      <h2>Access Admin Logs</h2>
      <p>
        To access the dashboard run the following commands:
      </p>
      <pre>
        EXPORT COACH_ID="02"
        EXPORT COACH_PASSWORD=""
        EXPORT COACH_SUBSCRIPTION_ID=""

        az login -u coach${COACH_ID}@asmw13.onmicrosoft.com -p "${COACH_PASSWORD}"
        az account set -s ${COACH_SUBSCRIPTION_ID}
        
        az aks get-credentials --resource-group azure_adventure_day_prod_rg --name azure-adventure-day-prod --overwrite-existing --admin
        kubectl port-forward deployment/po-grafana 3000 3000 -n monitoring
      </pre>
      <p>After that navigate to <a href="http://localhost:3000" target="_blank">http://localhost:3000</a> and use the following credentials:</p>
      <pre>
        UserName: admin
        Password: prom-operator
      </pre>
      <p>If this is the initial login to Grafana after the cluster has been created, you need to configure a Loki connector:</p>
      <pre>
        URL: http://loki:3100
      </pre>
    </div>
  </div>
</template>

<script>
export default {
  name: "Administration",
  data() {
    return {
      timerCurrentPhase: null,
      timerCurrentRunnerStatus: null,
      availablePhases: null,
      currentPhase: "Phase1_Deployment",
      currentRunnerStatus: "Stopped",
    };
  },
  created() {
    const fetchIntervalMs = 10 * 1000;

    this.$http
      .get("Runner/available-phases")
      .then((response) => {
        this.availablePhases = response.data;

        this.fetchCurrentPhase();
        this.timerCurrentPhase = setInterval(
          this.fetchCurrentPhase,
          fetchIntervalMs
        );
      })
      .catch(function (error) {
        console.error(error.response);
      });

    this.fetchCurrentRunnerStatus();
    this.timerCurrentRunnerStatus = setInterval(
      this.fetchCurrentRunnerStatus,
      fetchIntervalMs
    );
  },
  methods: {
    changePhase(phase, e) {
      e.preventDefault();
      this.$http
        .post("Runner/phase", {
          phaseName: phase,
        })
        .then((response) => {
          this.currentPhase = response.data.currentPhase;
        })
        .catch(function (error) {
          console.error(error.response);
        });
      return false;
    },
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
    fetchCurrentRunnerStatus() {
      return this.$http
        .get("Runner/status")
        .then((response) => {
          this.currentRunnerStatus = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
    start(e) {
      e.preventDefault();
      return this.$http
        .post("Runner/start")
        .then(() => {
          this.currentRunnerStatus = "Started";
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
    stop(e) {
      e.preventDefault();
      return this.$http
        .post("Runner/stop")
        .then(() => {
          this.currentRunnerStatus = "Stopped";
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
  },
  beforeDestroy() {
    clearInterval(this.timerCurrentPhase);
    clearInterval(this.timerCurrentRunnerStatus);
  },
  filters: {
    formatPhase: function (value) {
      return value.replace(/Phase/g, "");
    },
  },
};
</script>

<style>
.administration h2 {
  margin-top: 20px;
}

.administration .nav-pills .nav-link.disabled {
  background-color: #366aaf;
  text-decoration: none;
}

pre {
  color: #FFF;
}
</style>