<template>
  <div class="Leaderboard">
    <div class="index-content">
      <img class="lb-astronaut" src="../assets/images/png/astronaut-bg.png" />
      <img class="lb-spaceship" src="../assets/images/png/spaceship-bg.png" />
      <div class="leaderboard-container">
        <h1 class="leaderboard-title">THE LEADERBOARD</h1>
        <h2 class="leaderboard-subtitle">Azure Game Day Experience</h2>
        <p class="info-contest">
          Next scenario will be presented in 59 minutes.
        </p>
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
        <!-- <div class="leaderboard-powered">
          <p>
            Powered by
            <a href="https://playfab.com/" target="_blank">Playfab</a>
          </p>
        </div>
        <div class="leaderboard-buttons">
          <div class="custom-button">
            <a class="custom-button-link" href="/">play again</a>
          </div>
        </div> -->
      </div>
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
@import "../assets/css/leaderboard.css";
</style>
