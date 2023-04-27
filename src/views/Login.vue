<template>
  <v-container class="fill-height justify-center">
    <v-card width="500" title="Anmelden">
      <v-card-text>
        <v-text-field v-model="email" label="E-Mail"></v-text-field>
        <v-text-field
          v-model="password"
          label="Passwort"
          :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showPassword ? 'text' : 'password'"
          @click:append-inner="showPassword = !showPassword"
        ></v-text-field>
        <span> Du hast noch kein Konto? </span>
        <router-link :to="{ name: 'register' }"> Registrieren </router-link>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn variant="tonal">Anmelden</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";

const email = ref("admin@nimda.de");
const password = ref("Admin1!");
const showPassword = ref(false);
</script>
