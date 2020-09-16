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
                <label for="inputEnvUrl">GameEngine URL</label>
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
              <div v-if="saved" class="alert alert-success" role="alert">Saved!</div>
              <div v-if="errorMessage" class="alert alert-danger" role="alert">{{errorMessage}}</div>
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
      saved: false,
      errorMessage: null,
    };
  },
  mounted() {
    this.$http
      .get("Team/current")
      .then((response) => {
        this.enableTeamChange = response.data.name.startsWith("Team");
        this.teamName = response.data.name;
        this.environmentUrl = response.data.gameEngineUri;
      })
      .catch((error) => {
        console.error(error.response);
      });
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      if (this.teamName.length > 0) {
        this.$http
          .post("Team/current", {
            newName: this.teamName,
            newGameEngineUri: this.environmentUrl,
          })
          .then((response) => {
            this.enableTeamChange = response.data.name.startsWith("Team");
            this.teamName = response.data.name;
            this.environmentUrl = response.data.gameEngineUri;
            this.saved = true;
          })
          .catch(function (error) {
            if (error.response.status == 400 && error.response.data.message) {
              this.errorMessage = error.response.data.message;
            } else {
              this.errorMessage =
                "Internal error occured - please contact admin.";
              console.error(error.response);
            }
          });
      }
    },
  },
};
</script>