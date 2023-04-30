<template>
  <v-data-table
    :headers="headers"
    :items="store.users"
    :sort-by="[{ key: 'username', order: 'asc' }]"
    class="elevation-1"
  >
    <template #top>
      <v-toolbar flat>
        <v-toolbar-title>Nutzerverwaltung</v-toolbar-title>
        <v-spacer />
        <v-btn variant="tonal" @click="$router.push({ name: 'create-user' })"
          >Nutzer erstellen</v-btn
        >
      </v-toolbar>
    </template>
    <template #[`item.actions`]="{ item }">
      <v-icon class="me-2" @click="editUser(item.raw)"> mdi-pencil</v-icon>
      <v-icon @click="toggleDeleteDialog(item.raw)"> mdi-delete </v-icon>
    </template>
  </v-data-table>
  <router-view></router-view>
  <BaseDeleteDialog
    v-model="deleteDialog"
    @cancel="toggleDeleteDialog"
    @delete="removeUser"
  >
    den Nutzer {{ `'${store.user?.username}'` }}
  </BaseDeleteDialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import router from "@/router";
import { User } from "@/interfaces";
import BaseDeleteDialog from "@/components/BaseDeleteDialog.vue";

const store = useUsersStore();
const deleteDialog = ref(false);

const headers = ref([
  { title: "Username", key: "username" },
  { title: "Vorname", key: "firstName" },
  { title: "Nachname", key: "lastName" },
  { title: "E-Mail", key: "email" },
  { title: "Rolle", key: "role" },
  { key: "actions" },
]);

onMounted(() => {
  store.getUsers();
});

function editUser(user: User) {
  store.user = user;
  router.push({ name: "edit-user", params: { id: user.id } });
}

function toggleDeleteDialog(user?: User) {
  deleteDialog.value = !deleteDialog.value;
  if (!store.user) store.user = user;
  else
    setTimeout(() => {
      store.user = undefined;
    }, 200);
}

function removeUser() {
  store.deleteUser();
  toggleDeleteDialog();
}
</script>
