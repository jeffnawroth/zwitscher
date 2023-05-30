<template>
  <v-app>
    <v-app-bar color="white" flat density="compact" border>
      <v-app-bar-title>Zwitscher</v-app-bar-title>
      <v-spacer></v-spacer>
      <v-spacer></v-spacer>
      <v-spacer></v-spacer>
      <v-text-field
        bg-color="grey-lighten-2"
        variant="solo"
        placeholder="Suche..."
        density="compact"
        flat
        hide-details="auto"
      ></v-text-field>
      <v-tooltip :text="store.loggedIn ? 'Abmelden' : 'Anmelden'">
        <template #activator="{ props }">
          <v-btn v-bind="props" :icon="authIcon" @click="store.logout"></v-btn>
        </template>
      </v-tooltip>
    </v-app-bar>

    <v-navigation-drawer location="left" :rail="mdAndDown" permanent>
      <v-list nav>
        <v-list-item
          to="/"
          title="Startseite"
          prepend-icon="mdi-home"
          rounded="lg"
        >
        </v-list-item>
        <div v-if="store.loggedIn">
          <v-list-item
            :to="`/${store.user?.username}`"
            title="Profil"
            prepend-icon="mdi-account"
            rounded="lg"
          >
          </v-list-item>
          <div v-if="store.user?.role != Role.NUMBER_2">
            <v-list-item
              to="/users"
              title="Benutzerverwaltung"
              prepend-icon="mdi-account-group"
              rounded="lg"
            >
            </v-list-item>
            <v-list-item
              to="/dashboard"
              title="Dashboard"
              prepend-icon="mdi-view-dashboard"
              rounded="lg"
            >
            </v-list-item>
          </div>
          <v-list-item
            to="/settings"
            title="Einstellungen"
            prepend-icon="mdi-cog"
            rounded="lg"
          >
          </v-list-item>
        </div> </v-list
    ></v-navigation-drawer>

    <v-navigation-drawer location="right"> </v-navigation-drawer>

    <v-main>
      <v-container fluid style="max-width: 980px">
        <v-card>
          <router-view></router-view>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import axios from "axios";
import { useDisplay } from "vuetify/lib/framework.mjs";
import { Role } from "./typescript-axios-generated";
import { useRoute, useRouter } from "vue-router";

const store = useAuthenticationStore();
const route = useRoute();
const router = useRouter();
const { mdAndDown } = useDisplay();

const authIcon = computed(() => {
  return store.loggedIn ? "mdi-logout" : "mdi-login";
});

const title = computed(() => {
  const routeNameMapper = {
    home: "Startseite",
    profile: "Profil",
    users: "Benutzerverwaltung",
    dashboard: "Dashboard",
    settings: "Einstellungen",
  };
  //@ts-expect-error
  return routeNameMapper[route.name];
});

onMounted(() => {
  const userString = localStorage.getItem("user");
  if (userString) {
    const userData = JSON.parse(userString);
    store.setUserData(userData);

    console.log(store.user?.username);
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
