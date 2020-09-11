<template>
  <div class="about">
    <h1>Login</h1>
    <div class="content overflow-auto">
      <div class="container-fluid">
        <div class="row">
          <div class="col"></div>
          <div class="col-6">
            <form>
              <div class="form-group">
                <label for="exampleInputUsername">Username</label>
                <input
                  type="text"
                  class="form-control"
                  id="exampleInputUsername"
                  v-model="username"
                  required
                  autofocus
                />
              </div>
              <div class="form-group">
                <label for="exampleInputPassword">Password</label>
                <input
                  type="password"
                  class="form-control"
                  id="exampleInputPassword"
                  v-model="password"
                  required
                />
              </div>
              <button type="submit" class="btn btn-primary" @click="handleSubmit">Login</button>
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
      username: "",
      password: "",
    };
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      if (this.password.length > 0) {

        this.$http.post('team/login', {
            teamname: this.username,
            Password: this.password
        })
        .then(response => {
            let is_admin = response.data.isAdmin
            localStorage.setItem('user',JSON.stringify(response.data))
            localStorage.setItem('jwt',response.data.token)

            if (localStorage.getItem('jwt') != null){
                this.$emit('loggedIn')
                if(this.$route.params.nextUrl != null){
                    this.$router.push(this.$route.params.nextUrl)
                }
                else {
                    if(is_admin== 1){
                        this.$router.push('administration')
                    }
                    else {
                        this.$router.push('leaderboard')
                    }
                }
            }
        })
        .catch(function (error) {
            console.error(error.response);
        });
      }
    },
  },
};
</script>
