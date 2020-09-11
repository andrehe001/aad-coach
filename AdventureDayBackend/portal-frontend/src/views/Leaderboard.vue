<template>
  <div class="leaderboard">
    <h1>The Leaderboard</h1>
    <div class="content overflow-auto">
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col" class="min rank">Rank</th>
            <th scope="col">Team</th>
            <th scope="col" class="num">Score</th> <!-- Wins + Profit - Errors -->
            <th scope="col" class="num">Wins</th>
            <th scope="col" class="num">Loses</th>
            <th scope="col" class="num">Errors</th> <!-- Errors: Exceptions, Network, Timeouts, Hacker Attacks ... -->
            <th scope="col" class="num">Profit</th> <!-- Profit = Income - Costs -->
            <!-- Income : Won games -->
            <!-- Costs = Lost Games + Azure Costs -->
          </tr>
        </thead>
        <tbody>
          <tr v-for="team in Teams" v-bind:key="team.Name">
            <td class="min rank">
              <span>{{ team.Position | formatPosition }}</span>
            </td>
            <th scope="row">{{ team.Name }}</th>
            <td class="num highlight">{{ team.Score }}</td>
            <td class="num">{{ team.Wins }}</td>
            <td class="num">{{ team.Loses }}</td>
            <td class="num">{{ team.Errors }}</td>
            <td class="num">{{ team.Profit }}</td>
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
        { Position: 1, Name: "Teddybären", Score: 1100, Wins: 840, Loses: 340, Errors: 5, Profit: '$ 500k' },
        { Position: 2, Name: "Hackers"   , Score: 980 , Wins: 640, Loses: 240, Errors: 35, Profit: '$ 33k' },
        { Position: 3, Name: "Sharks"    , Score: 650 , Wins: 440, Loses: 140, Errors: 55, Profit: '$ 20k' },
        { Position: 4, Name: "Nerds"     , Score: 640 , Wins: 140, Loses: 40, Errors: 135, Profit: '$ 10k' }
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
</style>