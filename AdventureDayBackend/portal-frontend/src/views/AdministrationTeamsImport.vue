<template>
  <div class="team-accounts">
    <h1>Administration Teams Import</h1>
    <div class="content overflow-auto">
      <p class="text">Teams can be imported here by uploading an excel (xlsx) file in the format that is outlined in the docs. Be aware that importing via this way will <b><em>DELETE ALL EXISTING SCORES AND TEAMS!</em></b></p>
      <p><input
        type="file"
        accept=".xlsx"
        @change="onFileChange"/></p>
      <p> <button class="btn btn-primary" @click="onUploadFile">Start file upload...</button></p>
      <p style="color:#e83e8c;font-weight:900" ref="errorText"></p>
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col">TeamName</th>
            <th scope="col">Status</th>
            <th scope="col">Subscription Id</th>
            <th scope="col">Tenant Id</th>
            <th scope="col">Team Password</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="team in teams" v-bind:key="team.id">
            <th scope="row">
              {{ team.name }}
            </th>
            <td :class="team.status.toLowerCase() == 'ok' ? 'ok' : 'nok'">
              {{ team.status }}
            </td>
            <td>
              {{ team.subscriptionId }}
            </td>
            <td>
              {{ team.tenantId }}
            </td>
            <td>
              {{ team.teamPassword }}
            </td>
          </tr>
        </tbody>
      </table>
      <!--TODO: Make some kind of tab so that we can switch between the team and member views here -->
      <table class="table table-hover">
        <thead>
          <tr>
            <th scope="col">Username</th>
            <th scope="col">Status</th>
            <th scope="col">Team</th>
            <th scope="col">Password</th>
          </tr>
        </thead>
        <tbody>
          <template v-for="team in teams">
            <template v-for="member in team.members">
              <tr v-bind:key="member.id">
                <th scope="row">
                  {{ member.username }}
                </th>
                <td :class="member.status.toLowerCase() == 'ok' ? 'ok' : 'nok'">
                  {{ member.status }}
                </td>
                <td>
                  {{ team.name }}
                </td>
                <td>
                  {{ member.password }}
                </td>
              </tr>
            </template>
          </template>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: "AdministrationTeamsImport",
  data() {
    return {
      teams: [],
      selectedFile: ""
    };
  },
  created() {
    this.fetchTeamsWithMembers();
  },
  methods: {
    onFileChange(e) {
      const selectedFile = e.target.files[0]; // accessing file
      this.selectedFile = selectedFile;
    },
    onUploadFile() {
      const formData = new FormData();
      formData.append("files", this.selectedFile);  // appending file

      // sending file to the backend
      this.$http
        .post("Team/importxlsx", formData)
        .then(response => {
          console.log(response);
          this.$refs.errorText.innerText = "";
          this.fetchTeamsWithMembers();
        }, err => {
          if (err.response.status == 400) {
            this.$refs.errorText.innerText = err.response.data;
          }
          else {
            this.$refs.errorText.innerText = err;
          }
        });
    },
    fetchTeamsWithMembers() {
      this.$http
        .get("Team/allwithmembers")
        .then((response) => {
          this.teams = response.data;
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

.text em {
  color: #e83e8c;
}

.ok {
  color:lightgreen;
  font-weight: bolder;
}

.nok {
  color: #e83e8c;
  font-weight: bolder;
}

</style>