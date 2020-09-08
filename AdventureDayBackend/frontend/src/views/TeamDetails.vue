<template>
  <div class="team-details">
    <div class="console-screen stats">
      <p class="console-text">Stats</p>
      <div class="user-picks">
        <div class="user-picks-container">
          <div class="pick selectable">
            <p class="pick-text">
              Rank:
              <span>01</span>
            </p>
            <p class="pick-text">
              Score:
              <span>1100</span>
            </p>
            <p class="pick-text">
              Games per Second:
              <span>33</span>
            </p>
            <p class="pick-text">
              Win Rate:
              <span>20%</span>
            </p>
            <p class="pick-text">
              Error Rate:
              <span>0.4%</span>
            </p>
            <p class="pick-text">
              Profit:
              <span>1000</span>
            </p><br><br>
            <p class="pick-text">
              Current Phase:
              <span>2. Change Management</span>
            </p>
          </div>
        </div>
      </div>
    </div>
    <div class="console-screen log">
      <p class="console-text">Log</p>
      <div class="challenger-picks">
        <table class="leaderboard">
          <thead>
            <tr>
              <th class="lb-metrics">Timestamp</th>
              <th class="lb-metrics">Response Time</th>
              <th class="lb-metrics">Status</th>
              <th class="lb-reason">Reason</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td class="lb-metrics">5ms ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">10ms ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">2s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">20s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">30s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">31s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">5ms ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">10ms ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">2s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">20s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">30s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">31s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">2s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
            <tr>
              <td class="lb-metrics">20s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">30s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">SUCCESS</td>
              <td class="lb-reason"></td>
            </tr>
            <tr>
              <td class="lb-metrics">31s ago</td>
              <td class="lb-metrics">7.3345</td>
              <td class="lb-metrics">FAILURE</td>
              <td class="lb-reason">Lorem ipsum ...</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="cam-content">
        <div class="cam-button">
          <a class="cam-link" href="/cambattle">autoscroll: on</a>
        </div>
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
