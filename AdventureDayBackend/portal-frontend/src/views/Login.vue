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
        // TODO
        var response = { data: null };
        response.data = {
          token: "blub",
          user: { is_admin: this.username == "root" ? true : false },
        };

        let is_admin = response.data.user.is_admin;
        localStorage.setItem("user", JSON.stringify(response.data.user));
        localStorage.setItem("jwt", response.data.token);

        if (localStorage.getItem("jwt") != null) {
          this.$emit("loggedIn");
          if (this.$route.params.nextUrl != null) {
            this.$router.push(this.$route.params.nextUrl);
          } else {
            if (is_admin == 1) {
              this.$router.push("administration");
            } else {
              this.$router.push("leaderboard");
            }
          }
        }

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

<style>
.btn-primary {
  color: #fff;
  background-color: #366aaf;
  border-color: #fff;
}
.btn-primary:hover {
  color: #fff;
  background-color: #19304f;
  border-color: #fff;
}
</style>