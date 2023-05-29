<template>
  <v-data-table
    :headers="headers"
    :items="store.users"
    :sort-by="[{ key: 'username', order: 'asc' }]"
  >
    <!-- :search="search" -->
    <template #top>
      <v-toolbar flat floating>
        <v-toolbar-title>Benutzerverwaltung</v-toolbar-title>
        <!-- <v-text-field
          v-model="search"
          clearable
          density="compact"
          hide-details="auto"
          placeholder="Suche"
          prepend-inner-icon="mdi-magnify"
          variant="solo"
          flat
        ></v-text-field> -->
        <v-btn variant="tonal" @click="router.push({ name: 'create-user' })"
          >Nutzer erstellen</v-btn
        >
      </v-toolbar>
    </template>
    <template #[`item.role`]="{ item }">
      {{ getUserRole(item.raw.role) }}
    </template>
    <template #[`item.actions`]="{ item }">
      <IconWithTooltip
        text="Profil aufrufen"
        icon="mdi-open-in-app"
        @click="openProfile(item.raw)"
      ></IconWithTooltip>
      <IconWithTooltip
        text="Nutzer bearbeiten"
        icon="mdi-account-edit"
        @click="editUser(item.raw)"
      ></IconWithTooltip>
      <IconWithTooltip
        :text="item.raw.locked ? 'Nutzer entsperren' : 'Nutzer sperren'"
        :icon="item.raw.locked ? 'mdi-account-lock' : 'mdi-account-lock-open'"
        @click="openLockDialog(item.raw)"
      ></IconWithTooltip>
      <IconWithTooltip
        text="Nutzer löschen"
        icon="mdi-delete"
        @click="openDeleteDialog(item.raw)"
      ></IconWithTooltip>
    </template>
  </v-data-table>
  <router-view></router-view>
  <DeleteUserDialog v-model="deleteDialog"></DeleteUserDialog>
  <LockUserDialog v-model="lockDialog"></LockUserDialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import { User } from "@/interfaces";
import DeleteUserDialog from "@/components/DeleteUserDialog.vue";
import LockUserDialog from "@/components/LockUserDialog.vue";
import IconWithTooltip from "@/components/IconWithTooltip.vue";
import { useRouter } from "vue-router";
import { Role } from "@/typescript-axios-generated/api";

const store = useUsersStore();
const router = useRouter();

const deleteDialog = ref(false);
const lockDialog = ref(false);

// const search = ref("");

const headers = [
  { title: "Benutzername", key: "username" },
  { title: "Name", key: "name" },
  { title: "E-Mail", key: "email" },
  { title: "Rolle", key: "role" },
  { title: "", key: "actions" },
];

onMounted(() => {
  store.getUsers();
});

function getUserRole(role: Role) {
  const roleMap = {
    0: "Admin",
    1: "Moderator",
    2: "Nutzer",
  };

  return roleMap[role];
}

function editUser(user: User) {
  store.user = user;
  router.push({ name: "edit-user", params: { id: user.id } });
}

function openDeleteDialog(user?: User) {
  store.user = user;
  deleteDialog.value = true;
}

function openLockDialog(user: User) {
  store.user = user;
  lockDialog.value = true;
}

function openProfile(user: User) {
  router.push({
    name: "profile",
    params: { username: user.username },
  });
}
</script>
