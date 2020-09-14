<template>
  <div class="team-console">
    <h1>Team Console</h1>
    <div class="container-fluid content">
      <div class="row">
        <div id="team-status" class="col-3">
          <p class="status-text">
            <span>Rank:</span>
            <!-- TODO -->
            <span>01</span>
          </p>
          <p class="status-text">
            <span>Score:</span>
            <span>{{ TeamStats.score }}</span>
          </p>
          <br />
          <p class="status-text">
            <span>Wins:</span>
            <span>{{ TeamStats.wins }} ({{ winsShare }})</span>
          </p>
          <p class="status-text">
            <span>Loses:</span>
            <span>{{ TeamStats.loses }} ({{ losesShare }})</span>
          </p>
          <p class="status-text">
            <span>Errors:</span>
            <span>{{ TeamStats.errors }} ({{ errorsShare }})</span>
          </p>
          <br />
          <p class="status-text">
            <span>Profit:</span>
            <span>{{ TeamStats.profit }}</span>
          </p>
          <p class="status-text">
            <span>Income:</span>
            <span>{{ TeamStats.income }}</span>
          </p>
          <p class="status-text">
            <span>Costs:</span>
            <span>{{ TeamStats.costs }}</span>
          </p>
          <br />
          <p class="status-text">
            <span>
              Current
              <br />Phase:
            </span>
            <!-- TODO -->
            <span>2. Change Management</span>
          </p>
        </div>
        <div id="team-logs" class="col overflow-auto">
          <table class="table table-hover">
            <thead>
              <tr>
                <th scope="col" class="min">Timestamp</th>
                <th scope="col" class="min">Response Time</th>
                <th scope="col" class="min">Status</th>
                <th scope="col">Reason</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="logEntry in TeamLog" v-bind:key="logEntry.timestamp">
                <td class="min">{{ logEntry.timestamp | formatTimestamp }}</td>
                <td class="min">{{ logEntry.responeTimeMs }}</td>
                <td class="min highlight">
                  <span class="badge "
                        v-bind:class="{ 'badge-success': logEntry.status == 'SUCCESS', 'badge-danger': logEntry.status != 'SUCCESS' }">
                    {{ logEntry.status }}
                  </span>
                </td>
                <td>{{ logEntry.reason }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TeamConsole",
  data() {
    return {
      timerTeamStats: "",
      timerTeamLog: "",
      TeamStats: null,
      TeamLog: null
    };
  },
  computed: {
    winsShare: function () {
      const total = this.TeamStats.wins + this.TeamStats.loses + this.TeamStats.errors;
      return (this.TeamStats.wins / total * 100).toFixed(1) + "%";
    },
    losesShare: function () {
      const total = this.TeamStats.wins + this.TeamStats.loses + this.TeamStats.errors;
      return (this.TeamStats.loses / total * 100).toFixed(1) + "%";
    },
    errorsShare: function () {
      const total = this.TeamStats.wins + this.TeamStats.loses + this.TeamStats.errors;
      return (this.TeamStats.errors / total * 100).toFixed(1) + "%";
    }
  },
  created() {
    const fetchIntervalMs = 10 * 1000;

    this.fetchTeamStats();
    this.fetchTeamLog();
    this.timerTeamStats = setInterval(this.fetchTeamStats, fetchIntervalMs);
    this.timerTeamLog = setInterval(this.fetchTeamLog, fetchIntervalMs);
  },
  methods: {
    fetchTeamStats() {
      this.$http
        .get("Statistics/team/current/stats")
        .then((response) => {
          this.TeamStats = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
    fetchTeamLog() {
      this.$http
        .get("Statistics/team/current/log")
        .then((response) => {
          this.TeamLog = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    }
  },
  beforeDestroy() {
    clearInterval(this.timerTeamStats);
    clearInterval(this.timerTeamLog);
  },
  filters: {
    formatTimestamp: function (value) {
      // TODO: from UTC
      var timestamp = new Date(value);
      var now = new Date();
      var diffMs = now - timestamp;

      if (diffMs < 1000) {
        return diffMs + "ms ago";
      } else if (diffMs < 60 * 1000) {
        return Math.round(diffMs/1000)  + "s ago";
      } else if (diffMs < 60 * 60 * 1000) {
        return Math.round(diffMs/(60 * 1000)) + "min ago";
      } else if (diffMs < 24 * 60 * 60 * 1000) {
        return Math.round(diffMs/(60 * 60 * 1000)) + "hrs ago";
      }

      return Math.round(diffMs/(24 * 60 * 60 * 1000)) + "days ago";
    },
  },
};
</script>

<style>
.team-console .table {
  font-size: 1em;
}

.team-console .badge {
  font-size: 1em;
  font-weight: 500;
}

#team-status {
  background: #19304f;
  display: table;
  flex-direction: column;
  align-items: baseline;
  padding-bottom: 15px;
}

.status-text {
  display: table-row;
  color: #ffffff;
  margin: 0;
  margin-top: 10px;
}

.status-text span {
  display: table-cell;
  padding-left: 10px;
  padding-top: 20px;
  font-weight: 500;
  vertical-align: bottom;
}

.status-text span:first-child {
  text-transform: uppercase;
  font-weight: normal;
  padding-left: 0;
}

#team-logs {
  height: 100%;
}

.row {
  margin: 0;
  height: 100%;
}
</style>
