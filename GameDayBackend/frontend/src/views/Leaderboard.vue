<template>
  <div class="Leaderboard">
    <h1>The Leaderboard</h1>
    <div class="content overflow-auto">
      <table class="leaderboard">
        <thead>
          <tr>
            <th class="lb-position">Rank</th>
            <th class="lb-user">Team Name</th>
            <th class="lb-points">Score</th>
            <th class="lb-metrics">Games per Second</th>
            <th class="lb-metrics">Win Rate</th>
            <th class="lb-metrics">Error Rate</th>
            <th class="lb-metrics">Profit</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="team in Teams" v-bind:key="team.Name">
            <td class="lb-position">
              <span>{{ team.Position | formatPosition }}</span>
            </td>
            <td class="lb-user">
              {{ team.Name }}
            </td>
            <td class="lb-points">{{ team.Score }}</td>
            <td class="lb-metrics">{{ team.Score }}</td>
            <td class="lb-metrics">{{ team.Score }}</td>
            <td class="lb-metrics">{{ team.Score }}</td>
            <td class="lb-metrics">{{ team.Score }}</td>
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
      Teams: [
        { Position: 1, Name: "Teddybären", Score: 1100 },
        { Position: 2, Name: "Hackers", Score: 980 },
        { Position: 3, Name: "Sharks", Score: 650 },
        { Position: 4, Name: "Nerds", Score: 640 }
      ]
    };
  },
  filters: {
    formatPosition: function(value) {
      return ("0" + value).slice(-2);
    }
  }
};
</script>

<style>
/* @import "../assets/css/leaderboard.css"; */
</style>
