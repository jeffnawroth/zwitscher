<script setup lang="ts">
import type { User } from '@/typescript-axios-generated/api'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useTheme } from 'vuetify'
import DeleteUserDialog from '@/components/DeleteUserDialog.vue'
import IconWithTooltip from '@/components/IconWithTooltip.vue'
import LockUserDialog from '@/components/LockUserDialog.vue'
import { useAuthenticationStore } from '@/store/authentication'
import { useUsersStore } from '@/store/users'
// eslint-disable-next-line ts/consistent-type-imports
import { Role } from '@/typescript-axios-generated'

const store = useUsersStore()
const router = useRouter()
const authStore = useAuthenticationStore()
const theme = useTheme()

const deleteDialog = ref(false)
const lockDialog = ref(false)

const search = ref('')

const headers = [
  { title: 'Benutzername', key: 'username' },
  { title: 'Name', key: 'name' },
  { title: 'E-Mail', key: 'email' },
  { title: 'Rolle', key: 'role' },
  { title: '', key: 'actions' },
]

onMounted(() => {
  store.getUsers()
})

/**
 * Open user in dialog for editing
 * @param role
 * @returns role as string
 */
function getUserRole(role: Role) {
  const roleMap = {
    0: 'Admin',
    1: 'Moderator',
    2: 'Nutzer',
  }

  return roleMap[role]
}

/**
 * Open user in dialog for editing
 * @param user
 */
function editUser(user: User) {
  store.user = user
  router.push({ name: 'edit-user', params: { username: user.username } })
}
/**
 * Open dialog to delete the user
 * @param user
 */
function openDeleteDialog(user?: User) {
  store.user = user
  deleteDialog.value = true
}

/**
 * Open dialog to lock the user
 * @param user
 */
function openLockDialog(user: User) {
  store.user = user
  lockDialog.value = true
}

/**
 * Open profile
 * @param user
 */
function openProfile(user: User) {
  router.push({
    name: 'profile',
    params: { username: user.username },
  })
}
</script>

<template>
  <v-data-table
    :headers="headers"
    :items="store.users"
    :sort-by="[{ key: 'username', order: 'asc' }]"
    :loading="store.loading"
    :search="search"
  >
    <template #top>
      <v-toolbar
        flat
        :color="theme.current.value.dark ? 'grey-darken-4' : 'white'"
      >
        <v-text-field
          v-model="search"
          density="compact"
          hide-details
          variant="solo-filled"
          flat
          style="max-width: 300px"
          placeholder="Suche..."
        />
        <v-spacer />
        <v-btn
          icon="mdi-account-plus"
          variant="tonal"
          @click="router.push({ name: 'create-user' })"
        />
      </v-toolbar>
    </template>

    <template #[`item.role`]="{ item }">
      {{ getUserRole(item.role as Role) }}
    </template>
    <template #[`item.actions`]="{ item }">
      <IconWithTooltip
        text="Profil aufrufen"
        icon="mdi-open-in-app"
        @click="openProfile(item)"
      />
      <IconWithTooltip
        v-if="authStore.user?.role === Role.NUMBER_0"
        text="Nutzer bearbeiten"
        icon="mdi-account-edit"
        @click="editUser(item)"
      />
      <IconWithTooltip
        v-if="
          authStore.user?.role === Role.NUMBER_0
            || (item.role === Role.NUMBER_2
              && authStore.user?.role === Role.NUMBER_1)
        "
        :text="item.locked ? 'Nutzer entsperren' : 'Nutzer sperren'"
        :icon="item.locked ? 'mdi-account-lock' : 'mdi-account-lock-open'"
        @click="openLockDialog(item)"
      />
      <IconWithTooltip
        v-if="
          authStore.user?.role === Role.NUMBER_0
            || (item.role === Role.NUMBER_2
              && authStore.user?.role === Role.NUMBER_1)
        "
        text="Nutzer löschen"
        icon="mdi-account-remove"
        @click="openDeleteDialog(item)"
      />
    </template>
  </v-data-table>
  <router-view />
  <DeleteUserDialog v-model="deleteDialog" />
  <LockUserDialog v-model="lockDialog" />
</template>
