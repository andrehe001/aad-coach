<template>
  <div class="leaderboard">
    <h1>The Leaderboard</h1>
    <div class="content overflow-auto">
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col" class="min rank">Rank</th>
            <th scope="col">Team</th>
            <th scope="col" class="num">Score</th>
            <!-- Wins + Profit - Errors -->
            <th scope="col" class="num">Wins</th>
            <th scope="col" class="num">Loses</th>
            <th scope="col" class="num">Errors</th>
            <!-- Errors: Exceptions, Network, Timeouts, Hacker Attacks ... -->
            <th scope="col" class="num">Profit</th>
            <!-- Profit = Income - Costs -->
            <!-- Income : Won games -->
            <!-- Costs = Lost Games + Azure Costs -->
          </tr>
        </thead>
        <tbody>
          <tr v-for="(team, index) in TeamScores" v-bind:key="team.name">
            <td class="min rank">
              <span>{{ index+1 | formatPosition }}</span>
            </td>
            <th scope="row" title="Team ID: {{ team.id}}">{{ team.name }}</th>
            <td class="num highlight">{{ team.score }}</td>
            <td class="num">{{ team.wins }}</td>
            <td class="num">{{ team.loses }}</td>
            <td class="num">{{ team.errors }}</td>
            <td class="num">{{ team.profit | formatProfit }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: "Leaderboard",
  data() {
    return {
      timer: "",
      TeamScores: null,
    };
  },
  created() {
    const fetchIntervalMs = 10 * 1000;

    this.fetchLeaderboardStats();
    this.timer = setInterval(this.fetchLeaderboardStats, fetchIntervalMs);
  },
  methods: {
    fetchLeaderboardStats() {
      this.$http
        .get("Statistics/leaderboard")
        .then((response) => {
          this.TeamScores = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    },
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
  filters: {
    formatPosition: function (value) {
      return ("0" + value).slice(-2);
    },
    formatProfit: function (value) {
      return "$ " + value;
    },
  },
};
</script>
