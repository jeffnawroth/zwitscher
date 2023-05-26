<template>
  <v-data-table
    :headers="headers"
    :items="store.users"
    :sort-by="[{ key: 'username', order: 'asc' }]"
    class="elevation-1"
  >
    <template #top>
      <v-toolbar flat>
        <v-toolbar-title>Benutzerverwaltung</v-toolbar-title>
        <v-spacer />
        <v-btn variant="tonal" @click="$router.push({ name: 'create-user' })"
          >Nutzer erstellen</v-btn
        >
      </v-toolbar>
    </template>
    <template #[`item.actions`]="{ item }">
      <v-icon class="me-2" @click="editUser(item.raw)"> mdi-pencil</v-icon>
      <v-icon @click="openDeleteDialog(item.raw)"> mdi-delete </v-icon>
    </template>
  </v-data-table>
  <router-view></router-view>
  <DeleteUserDialog v-model="deleteDialog"></DeleteUserDialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import router from "@/router";
import { User } from "@/interfaces";
import DeleteUserDialog from "@/components/DeleteUserDialog.vue";

const store = useUsersStore();
const deleteDialog = ref(false);

const headers = [
  { title: "Username", key: "username" },
  { title: "Vorname", key: "firstName" },
  { title: "Nachname", key: "lastName" },
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
</script>
