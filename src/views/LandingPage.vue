<template>
  <v-dialog v-model="dialog">
    <v-card class="mx-auto">
      <v-card-title class="text-h4"> </v-card-title>
      <v-card-text>
        <p class="text-h2 mb-4">Willkommen beim Mikroblogging-Dienst!</p>
        <p v-if="!store.loggedIn" class="text-h5">
          Um den Mikroblogging-Dienst zu nutzen, müssen Sie sich
          <router-link :to="{ name: 'login' }"> Anmelden </router-link>
          oder
          <router-link :to="{ name: 'register' }"> Registrieren </router-link>
        </p>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn variant="tonal" @click="dialog = false">Schließen</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { useAuthenticationStore } from "@/store/authentication";
import { ref } from "vue";

const store = useAuthenticationStore();
const dialog = ref(true);
</script>
