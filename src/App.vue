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
        <template #append> </template>
        <div v-for="item in items" :key="item.title">
          <v-list-item
            v-if="store.loggedIn || item.title === 'Startseite'"
            :to="item.route"
            :title="item.title"
            :prepend-icon="item.icon"
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

const store = useAuthenticationStore();
const { mdAndDown } = useDisplay();

const items = [
  {
    title: "Startseite",
    icon: "mdi-home",
    route: "/",
  },
  {
    title: "Profil",
    icon: "mdi-account",
    route: "",
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

const showMenuIcon = computed(() => {
  return mdAndDown.value;
});

onMounted(() => {
  const userString = localStorage.getItem("user");
  if (userString) {
    const userData = JSON.parse(userString);
    store.setUserData(userData);

    items[1].route = `/${store.user?.username}`;
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
