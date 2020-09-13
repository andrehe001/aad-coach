<template>
  <div class="team-accounts">
    <h1>Team Accounts</h1>
    <div class="content overflow-auto">
      <p class="text">Distribute the following accounts across your team and use them to log into the <br><a href="https://portal.azure.com/">Azure Portal</a>.</p>
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col">Username</th>
            <th scope="col">Password</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in Users" v-bind:key="user.Username">
            <th scope="row">
              {{ user.Username }}
            </th>
            <td>
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
        { Username: "AADTeam1A1@asmw13.onmicrosoft.com", Password: "InitialPa55word" },
        { Username: "AADTeam1A2@asmw13.onmicrosoft.com", Password: "InitialPa55word" },
        { Username: "AADTeam1A3@asmw13.onmicrosoft.com", Password: "InitialPa55word" },
        { Username: "AADTeam1A4@asmw13.onmicrosoft.com", Password: "InitialPa55word" }
      ]
    };
  }
};
</script>

<style>
.team-accounts p.text {
  text-align: center;
  margin: 20px 0 10px;
}

.team-accounts .table {
  font-size: 1.2rem;
}
</style>