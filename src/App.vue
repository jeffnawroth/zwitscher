<template>
  <v-app id="inspire">
    <v-app-bar height="70" class="px-3" color="white" flat density="compact">
      <v-spacer></v-spacer>
      <v-text-field
        class="mt-5"
        variant="solo"
        placeholder="Suche..."
        density="compact"
      ></v-text-field>
      <v-spacer></v-spacer>
      <v-btn icon="mdi-logout" @click="store.logout"></v-btn>
    </v-app-bar>

    <v-main class="bg-grey-lighten-3">
      <v-container>
        <v-row>
          <v-col cols="3">
            <v-sheet rounded="lg" min-height="268" class="pa-2">
              <v-list>
                <v-list-item
                  v-for="item in items"
                  :key="item.title"
                  :to="item.route"
                  :title="item.title"
                  :prepend-icon="item.icon"
                  rounded="lg"
                >
                </v-list-item>
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
import { onMounted } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import axios from "axios";

const items = [
  {
    title: "Startseite",
    icon: "mdi-home",
    route: "/",
  },
  {
    title: "Profil",
    icon: "mdi-account",
    route: "/profile",
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

const store = useAuthenticationStore();

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
