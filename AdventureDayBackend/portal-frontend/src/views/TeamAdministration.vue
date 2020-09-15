<template>
  <div class="about">
    <h1>Team Administration</h1>
    <div class="content overflow-auto">
      <div class="container-fluid">
        <div class="row">
          <div class="col"></div>
          <div class="col-10">
            <form>
              <div class="form-group">
                <label for="inputTeamName">Team Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="inputTeamName"
                  v-model="teamName"
                  :disabled="!enableTeamChange"
                  required
                  autofocus
                />
                <small id="emailHelp" class="form-text text-muted">This can only be changed once!</small>
              </div>
              <div class="form-group">
                <label for="inputEnvUrl">Environment URL</label>
                <input
                  type="text"
                  class="form-control"
                  id="inputEnvUrl"
                  v-model="environmentUrl"
                  required
                />
                <small
                  id="emailHelp"
                  class="form-text text-muted"
                >Public endpoint of your environment, where the requests are send to.</small>
              </div>
              <button type="submit" class="btn btn-primary" @click="handleSubmit">Save</button>
            </form>
          </div>
          <div class="col"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      teamName: "",
      environmentUrl: "",
      enableTeamChange: true,
    };
  },
  mounted() {
    this.$http
      .get("Team/current")
      .then(response => {
        this.enableTeamChange = response.data.name.startsWith("Team");
        this.teamName = response.data.name;
        this.environmentUrl = response.data.gameEngineUri;
      })
      .catch(error => {
        console.error(error.response);
      });
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      if (this.teamName.length > 0) {
        this.$http.post('Team/current', {
            newName: this.teamName,
            newGameEngineUri: this.environmentUrl
        })
        .then(response => {
          this.enableTeamChange = response.data.name.startsWith("Team");
          this.teamName = response.data.name;
          this.environmentUrl = response.data.gameEngineUri;
        })
        .catch(function (error) {
            console.error(error.response);
        });
      }
    },
  },
};
</script>