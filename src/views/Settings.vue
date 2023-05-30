<template>
  <v-list>
    <v-list-item @click="changePasswordDialog = true"
      >Passwort ändern</v-list-item
    >
    <v-divider></v-divider>
    <v-list-item @click="changeEmailDialog = true">E-Mail ändern</v-list-item>
    <v-divider></v-divider>
    <v-list-item>Konto löschen</v-list-item>
  </v-list>

  <PasswordDialog v-model="changePasswordDialog"></PasswordDialog>
  <EmailDialog v-model="changeEmailDialog"></EmailDialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import PasswordDialog from "@/components/Settings/PasswordDialog.vue";
import EmailDialog from "@/components/Settings/EmailDialog.vue";

const changePasswordDialog = ref(false);
const changeEmailDialog = ref(false);
const deleteAccountDialog = ref(false);
</script>
