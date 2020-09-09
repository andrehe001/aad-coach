<template>
  <div id="main">

    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
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
                <router-link to="/code-of-conduct" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Code of Conduct
                    </a>
                </router-link>
              </li>
              <li>
                <div class="dropdown-divider"></div>
              </li>
              <li class="nav-item">
                <router-link to="/team-console" v-slot="{ href, route, navigate, isActive, isExactActive }" >
                    <a :href="href" @click="navigate" :class="['nav-link', isExactActive && 'active']" data-dismiss="modal">
                      Management
                    </a>
                </router-link>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="content-container">
      <div class="navbar-dark">
        <button class="navbar-toggler" type="button" data-toggle="modal" data-target="#exampleModal">
          <span class="navbar-toggler-icon"></span>
        </button>
      </div>
      
      <router-view />
    </div>
  </div>
</template>

<script>
export default {
  name: 'MainLayout',
  components: {
  }
}
</script>

<style>

</style>
