<template>
  <v-list>
    <v-list-item @click="changePasswordDialog = true"
      >Passwort ändern</v-list-item
    >
    <v-divider></v-divider>
    <v-list-item>E-Mail-Adresse ändern</v-list-item>
    <v-divider></v-divider>
    <v-list-item>Konto löschen</v-list-item>
  </v-list>

  <PasswordDialog v-model="changePasswordDialog"></PasswordDialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import PasswordDialog from "@/components/PasswordDialog.vue";
import BaseDeleteDialog from "@/components/BaseComponents/BaseDeleteDialog.vue";

const changePasswordDialog = ref(false);
const changeEmailDialog = ref(false);
const deleteAccountDialog = ref(false);
</script>
