<script setup lang="ts">
import { storeToRefs } from 'pinia'
import { computed, onMounted, ref, watch } from 'vue'
import { useDisplay } from 'vuetify'
import { useAuthenticationStore } from '@/store/authentication'
import FollowedUsersList from './components/FollowedUsersList.vue'
import IconWithTooltip from './components/IconWithTooltip.vue'
import NotificationContainer from './components/Notification/NotificationContainer.vue'
import CreatePostDialog from './components/Posts/CreatePostDialog.vue'
import { generateFileURL } from './helpers'
import { useSettingsStore } from './store/settings'
import { useUsersStore } from './store/users'
import { Role } from './typescript-axios-generated'

const store = useAuthenticationStore()
const settingsStore = useSettingsStore()
const { mdAndDown } = useDisplay()
const { loggedIn, user } = storeToRefs(store)
const usersStore = useUsersStore()

const showDialog = ref(false)
const search = ref('')

const authIcon = computed(() => {
  return loggedIn ? 'mdi-logout' : 'mdi-login'
})

watch(search, (val) => {
  usersStore.searchUser(val)
})

onMounted(() => {
  // Load and set user-data from local storage if exists
  const userString = localStorage.getItem('user')
  if (userString) {
    const userData = JSON.parse(userString)
    store.setUserData(userData)
  }

  // Load and set theme from local storage if exists
  const theme = localStorage.getItem('theme')
  if (theme)
    settingsStore.setTheme(theme)
})
</script>

<template>
  <v-app>
    <v-app-bar flat density="compact" border>
      <v-app-bar-title>Zwitscher</v-app-bar-title>

      <v-autocomplete
        id="search"
        v-model:search="search"
        :items="usersStore.searchResult"
        item-value="id"
        item-title="name"
        variant="solo-filled"
        placeholder="Suche..."
        density="compact"
        flat
        hide-details
        hide-no-data
        style="max-width: 300px"
        clearable
        menu-icon=""
        :loading="usersStore.searching"
      >
        <template #item="{ props, item }">
          <v-list-item
            v-bind="props"
            :title="item?.raw?.name!"
            :subtitle="`@${item?.raw?.userName!}`"
            :to="`/${item.raw?.userName}`"
          >
            <template #prepend>
              <v-avatar v-if="!item.raw.avatar" color="grey">
                <v-icon icon="mdi-account-circle" size="x-large" />
              </v-avatar>
              <v-avatar v-else :image="generateFileURL(item.raw?.avatar)" />
            </template>
          </v-list-item>
        </template>
      </v-autocomplete>

      <IconWithTooltip
        class="mx-2"
        icon="mdi-theme-light-dark"
        :text="
          settingsStore.theme.global.current.dark
            ? 'Dark Mode deaktivieren'
            : 'Dark Mode aktivieren'
        "
        @click="settingsStore.toggleTheme"
      />

      <IconWithTooltip
        class="mx-2"
        :icon="authIcon"
        :text="loggedIn ? 'Abmelden' : 'Anmelden'"
        @click="store.logout"
      />
    </v-app-bar>

    <v-navigation-drawer
      width="300"
      floating
      location="left"
      :rail="mdAndDown"
      permanent
    >
      <v-list nav>
        <v-list-item
          to="/"
          title="Startseite"
          prepend-icon="mdi-home"
          rounded="lg"
        />
        <div v-if="loggedIn">
          <v-list-item
            :to="`/${user?.username}`"
            title="Profil"
            prepend-icon="mdi-account"
            rounded="lg"
          />
          <div v-if="user?.role !== Role.NUMBER_2">
            <v-list-item
              to="/dashboard"
              title="Dashboard"
              prepend-icon="mdi-view-dashboard"
              rounded="lg"
            />
            <v-list-item
              to="/data-management"
              title="Datenverwaltung"
              prepend-icon="mdi-database"
              rounded="lg"
            />
          </div>
          <v-list-item
            to="/settings"
            title="Einstellungen"
            prepend-icon="mdi-cog"
            rounded="lg"
          />

          <v-list-item
            v-if="mdAndDown"
            variant="tonal"
            rounded="lg"
            prepend-icon="mdi-alpha-z"
            @click="showDialog = true"
          />
          <v-list-item v-else>
            <v-btn block variant="tonal" @click="showDialog = true">
              Zwitschern
            </v-btn>
          </v-list-item>
        </div>
      </v-list>
      <div v-if="loggedIn" />
    </v-navigation-drawer>

    <v-navigation-drawer location="right">
      <FollowedUsersList v-if="loggedIn" />
    </v-navigation-drawer>

    <NotificationContainer />

    <v-main>
      <v-container fluid style="max-width: 980px">
        <v-card>
          <CreatePostDialog v-model="showDialog" />
          <router-view />
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<style>
.hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>
