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
    <template #[`item.actions`]="{}">
      <v-icon class="me-2"> mdi-pencil</v-icon>
      <v-icon> mdi-delete </v-icon>
    </template>
  </v-data-table>
  <router-view></router-view>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useUsersStore } from "@/store/users";

const store = useUsersStore();

const headers = ref([
  { title: "Username", key: "username" },
  { title: "Vorname", key: "firstName" },
  { title: "Nachname", key: "lastName" },
  { title: "E-Mail", key: "email" },
  { title: "Rolle", key: "role" },
  { key: "actions" },
]);
</script>
