<template>
  <div id="main">

    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" v-if="loggedIn">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-body">
            <ul class="nav flex-column nav-pills nav-fill">
              <li class="nav-item">
                <router-link to="/leaderboard" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Leaderboard
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/team-console" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Team Console
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/phase-guides" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Phase Guides
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/team-accounts" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Team Accounts
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/team-administration" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Team Administration
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/social-sharing" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Social Sharing
                    </a>
                </router-link>
              </li>
              <li class="nav-item">
                <router-link to="/code-of-conduct" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Code of Conduct
                    </a>
                </router-link>
              </li>
              <li>
                <div class="dropdown-divider"></div>
              </li>
              <li class="nav-item" v-if="isAdmin">
                <router-link to="/administration" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Administration Phases
                    </a>
                </router-link>
              </li>
              <li class="nav-item" v-if="isAdmin">
                <router-link to="/administration-teams-import" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Teams Import
                    </a>
                </router-link>
              </li>
               <li class="nav-item">
                <router-link v-if="loggedIn" to="/logout" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      [Logout {{username}}]
                    </a>
                </router-link>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="content-container">
      <div class="navbar-dark" v-if="loggedIn">
        <button class="navbar-toggler" type="button" data-toggle="modal" data-target="#exampleModal">
          <span class="navbar-toggler-icon"></span>
        </button>
      </div>
      
      <router-view @loggedIn="updateLoginStatus" />
    </div>
  </div>
</template>

<script>
export default {
  name: 'MainLayout',
  data() {
      return {
          loggedIn : false,
          isAdmin : false,
          username: null
      }
  },
  created() {
      this.updateLoginStatus();
  },
  methods: {
    updateLoginStatus() {
      let user = JSON.parse(localStorage.getItem('user'))
      this.isAdmin = user.isAdmin == 1;
      this.username = user.teamname;
      this.loggedIn = localStorage.getItem('jwt') != null;
    }
  },
  components: {

  }
}
</script>

<style>
a {
  color: #366aaf;
}

.nav-pills .nav-link.active, .nav-pills .show>.nav-link {
    background-color: #366aaf;
}
</style>
