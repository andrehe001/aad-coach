<template>
  <div class="Leaderboard">
    <h1>Team Accounts</h1>
    <div class="content overflow-auto">
      <table class="leaderboard">
        <thead>
          <tr>
            <th class="lb-user">Username</th>
            <th class="lb-user">Password</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in Users" v-bind:key="user.Username">
            <td class="lb-user">
              {{ user.Username }}
            </td>
            <td class="lb-user">
              {{ user.Password }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: "TeamAccounts",
  data() {
    return {
      Users: [
        { Username: "Teddybären", Password: "Secret" },
        { Username: "Hackers", Password: "Secret" },
        { Username: "Sharks", Password: "Secret" },
        { Username: "Nerds", Password: "Secret" }
      ]
    };
  }
};
</script>

<style>
/* @import "../assets/css/leaderboard.css"; */
</style>
