<template>
  <div class="team-console">
    <h1>Team Console</h1>
    <div class="container-fluid content">
      <div class="row">
        <div id="team-status" class="col-3">
          <p class="status-text">
            <span>Rank:</span>
            <span>01</span>
          </p>
          <p class="status-text">
            <span>Score:</span>
            <span>1100</span>
          </p>
          <p class="status-text">
            <span>Games per Second:</span>
            <span>33</span>
          </p>
          <p class="status-text">
            <span>Win Rate:</span>
            <span>20%</span>
          </p>
          <p class="status-text">
            <span>Error Rate:</span>
            <span>0.4%</span>
          </p>
          <p class="status-text">
            <span>Profit:</span>
            <span>1000</span>
          </p><br>
          <p class="status-text">
            <span>Current<br/>Phase:</span>
            <span>2. Change Management</span>
          </p>
        </div>
        <div id="team-logs" class="col overflow-auto">
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
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TeamConsole",
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

#team-status {
  background: #19304F;
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

#team-logs .leaderboard {
  width: 100%;
}

#team-logs .leaderboard thead, #team-logs .leaderboard tbody {
  display: table-header-group;
}

.row {
  margin: 0;
  height: 100%;
}

.leaderboard {
  font-size: 1em;
}

.leaderboard tr .lb-metrics {
  width: 100px;
  text-align: center;
  justify-self: center;
}
</style>
