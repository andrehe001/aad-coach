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
          <br>
          <p class="status-text">
            <span>Wins:</span>
            <span>840 (70,9%)</span>
          </p>
          <p class="status-text">
            <span>Loses:</span>
            <span>340 (28,6%)</span>
          </p>
          <p class="status-text">
            <span>Errors:</span>
            <span>5 (0.5%)</span>
          </p>
          <br>
          <p class="status-text">
            <span>Profit:</span>
            <span>1000</span>
          </p>
          <p class="status-text">
            <span>Income:</span>
            <span>2000</span>
          </p>
          <p class="status-text">
            <span>Costs:</span>
            <span>1000</span>
          </p>
          <br>
          <p class="status-text">
            <span>Current<br/>Phase:</span>
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
              <tr>
                <td class="min">5ms ago</td>
                <td class="min">7.3345</td>
                <td class="min highlight">SUCCESS</td>
                <td></td>
              </tr>
              <tr>
                <td class="min">25ms ago</td>
                <td class="min">3.3345</td>
                <td class="min highlight">SUCCESS</td>
                <td>Lorem ipsum ...</td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
              </tr>
              <tr>
                <td class="min">10s ago</td>
                <td class="min">207.3345</td>
                <td class="min highlight">FAILED</td>
                <td class="cell-collapse"><span>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.</span></td>
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
.team-console .table {
  font-size: 1em;
}

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

.row {
  margin: 0;
  height: 100%;
}
</style>
