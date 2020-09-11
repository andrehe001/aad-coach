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
                <small id="emailHelp" class="form-text text-muted">Public endpoint of your environment, where the requests are send to.</small>
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
    };
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      if (this.teamName.length > 0) {
        
        

        // this.$http.get('https://www.google.com/', { // TODO
        //     username: this.username,
        //     password: this.password
        // })
        // .then(response => {

        //     // TODO
        //     response.data = {token: 'blub', user: { is_admin : true}};

        //     let is_admin = response.data.user.is_admin
        //     localStorage.setItem('user',JSON.stringify(response.data.user))
        //     localStorage.setItem('jwt',response.data.token)

        //     if (localStorage.getItem('jwt') != null){
        //         this.$emit('loggedIn')
        //         if(this.$route.params.nextUrl != null){
        //             this.$router.push(this.$route.params.nextUrl)
        //         }
        //         else {
        //             if(is_admin== 1){
        //                 this.$router.push('management')
        //             }
        //             else {
        //                 this.$router.push('leaderboard')
        //             }
        //         }
        //     }
        // })
        // .catch(function (error) {
        //     console.error(error.response);
        // });
      }
    },
  },
};
</script>