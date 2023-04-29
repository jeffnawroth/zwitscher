<template>
  <v-container class="fill-height justify-center">
    <v-row>
      <v-col class="d-flex justify-center">
        <span class="text-h2 font-weight-bold"
          >Willkommen beim Mikroblogging-Dienst!</span
        >
      </v-col>
      <v-col
        v-if="!store.loggedIn"
        class="d-flex justify-center text-center"
        cols="12"
      >
        <span class="text-h5">
          Um den Mikroblogging-Dienst zu nutzen, müssen Sie sich
          <router-link :to="{ name: 'login' }"> Anmelden </router-link>
          oder
          <router-link :to="{ name: 'register' }"> Registrieren </router-link>
        </span>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useAuthenticationStore } from "@/store/authentication";

const store = useAuthenticationStore();
</script>
