<template>
  <v-app id="inspire">
    <v-app-bar height="70" class="px-3" color="white" flat density="compact">
      <v-spacer></v-spacer>
      <v-text-field
        class="mt-5"
        variant="outlined"
        placeholder="Suche..."
        density="compact"
        flat
      ></v-text-field>
      <v-spacer></v-spacer>
      <v-tooltip :text="store.loggedIn ? 'Abmelden' : 'Anmelden'">
        <template #activator="{ props }">
          <v-btn v-bind="props" :icon="authIcon" @click="store.logout"></v-btn>
        </template>
      </v-tooltip>
    </v-app-bar>

    <v-main class="bg-grey-lighten-3">
      <v-container fluid>
        <v-row>
          <v-col cols="3">
            <v-sheet rounded="lg" class="pa-2">
              <v-list>
                <div v-for="item in items" :key="item.title">
                  <v-list-item
                    v-if="store.loggedIn || item.title === 'Startseite'"
                    :to="item.route"
                    :title="item.title"
                    :prepend-icon="item.icon"
                    rounded="lg"
                  >
                  </v-list-item>
                </div>
              </v-list>
            </v-sheet>
          </v-col>

          <v-col cols="6">
            <v-sheet rounded="lg">
              <router-view></router-view>
            </v-sheet>
          </v-col>

          <v-col cols="3">
            <v-sheet rounded="lg" min-height="268">
              <!--  -->
            </v-sheet>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { computed, onMounted } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import axios from "axios";

const store = useAuthenticationStore();
const items = [
  {
    title: "Startseite",
    icon: "mdi-home",
    route: "/",
  },
  {
    title: "Profil",
    icon: "mdi-account",
    route: `/${store.user?.username}/profile`,
  },
  {
    title: "Benutzerverwaltung",
    icon: "mdi-account-group",
    route: "/users",
  },
  {
    title: "Dashboard",
    icon: "mdi-view-dashboard",
    route: "/dashboard",
  },

  {
    title: "Einstellungen",
    icon: "mdi-cog",
    route: "/settings",
  },
];

const authIcon = computed(() => {
  return store.loggedIn ? "mdi-logout" : "mdi-login";
});

onMounted(() => {
  const userString = localStorage.getItem("user");
  if (userString) {
    const userData = JSON.parse(userString);
    store.setUserData(userData);
  }
  /*  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response.status === 401) {
        store.logout();
      }
      return Promise.reject(error);
    }
  ); */
});
</script>
