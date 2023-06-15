<template>
  <v-app>
    <v-app-bar flat density="compact" border>
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

      <IconWithTooltip
        class="mx-2"
        icon="mdi-theme-light-dark"
        :text="
          settingsStore.theme.global.current.dark
            ? 'Dark Mode deaktivieren'
            : 'Dark Mode aktivieren'
        "
        @click="settingsStore.toggleTheme"
      ></IconWithTooltip>

      <IconWithTooltip
        class="mx-2"
        :icon="authIcon"
        :text="store.loggedIn ? 'Abmelden' : 'Anmelden'"
        @click="store.logout"
      >
      </IconWithTooltip>
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
        </div>
      </v-list>
      <div v-if="store.loggedIn" class="pa-5">
        <v-btn block variant="tonal" @click="showDialog = true"
          >Zwitschern</v-btn
        >
      </div>
    </v-navigation-drawer>

    <v-navigation-drawer location="right"> </v-navigation-drawer>

    <NotificationContainer></NotificationContainer>

    <v-main>
      <v-container fluid style="max-width: 980px">
        <v-card>
          <CreatePostDialog v-model="showDialog"></CreatePostDialog>
          <router-view></router-view>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import { useDisplay } from "vuetify/lib/framework.mjs";
import { Role } from "./typescript-axios-generated";
import { useSettingsStore } from "./store/settings";
import IconWithTooltip from "./components/IconWithTooltip.vue";
import NotificationContainer from "./components/Notification/NotificationContainer.vue";
import CreatePostDialog from "./components/Posts/CreatePostDialog.vue";

const store = useAuthenticationStore();
const settingsStore = useSettingsStore();
const { mdAndDown } = useDisplay();

const showDialog = ref(false);

const authIcon = computed(() => {
  return store.loggedIn ? "mdi-logout" : "mdi-login";
});

onMounted(() => {
  const userString = localStorage.getItem("user");
  if (userString) {
    const userData = JSON.parse(userString);
    store.setUserData(userData);
  }

  const theme = localStorage.getItem("theme");
  if (theme) settingsStore.setTheme(theme);
});
</script>
