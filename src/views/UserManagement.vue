<template>
  <v-data-table
    :headers="headers"
    :items="store.users"
    :sort-by="[{ key: 'username', order: 'asc' }]"
    class="elevation-1"
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
        <v-btn variant="tonal" @click="$router.push({ name: 'create-user' })"
          >Nutzer erstellen</v-btn
        >
      </v-toolbar>
    </template>
    <template #[`item.actions`]="{ item }">
      <v-icon class="me-2" @click="openProfile(item.raw)"> mdi-account</v-icon>
      <v-icon class="me-2" @click="editUser(item.raw)"> mdi-pencil</v-icon>
      <v-icon class="me-2" @click="openLockDialog(item.raw)">
        {{ item.raw.locked ? "mdi-lock" : "mdi-lock-open" }}</v-icon
      >
      <v-icon @click="openDeleteDialog(item.raw)"> mdi-delete </v-icon>
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
import router from "@/router";
import { User } from "@/interfaces";
import DeleteUserDialog from "@/components/DeleteUserDialog.vue";
import LockUserDialog from "@/components/LockUserDialog.vue";

const store = useUsersStore();
const deleteDialog = ref(false);
const lockDialog = ref(false);

// const search = ref("");

const headers = [
  { title: "Username", key: "username" },
  { title: "Name", key: "name" },
  { title: "E-Mail", key: "email" },
  { title: "Rolle", key: "role" },
  { title: "", key: "actions" },
];

onMounted(() => {
  store.getUsers();
});

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
