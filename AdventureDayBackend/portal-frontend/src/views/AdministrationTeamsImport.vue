<template>
  <div class="team-accounts">
    <h1>Administration Teams Import</h1>
    <div class="content overflow-auto">
      <p class="text">This will have the excel upload button.</p>
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col">TeamName</th>
            <th scope="col">Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" v-bind:key="user.Username">
            <th scope="row">
              {{ user.username }}
            </th>
            <td>
              {{ user.password }}
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
      users: []
    };
  },
  created() {
    this.fetchTeamAccounts();
  },
  methods: {
    fetchTeamAccounts() {
      this.$http
        .get("Team/members/current")
        .then((response) => {
          this.users = response.data;
        })
        .catch(function (error) {
          console.error(error.response);
        });
    }
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