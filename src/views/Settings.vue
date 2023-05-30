<template>
  <v-list>
    <v-list-item @click="changePasswordDialog = true"
      >Passwort ändern</v-list-item
    >
    <v-divider></v-divider>
    <v-list-item @click="changeEmailDialog = true">E-Mail ändern</v-list-item>
    <v-divider></v-divider>
    <v-list-item @click="deleteAccountDialog = true">Konto löschen</v-list-item>
  </v-list>

  <PasswordDialog v-model="changePasswordDialog"></PasswordDialog>
  <EmailDialog v-model="changeEmailDialog"></EmailDialog>
  <BaseDeleteDialog
    v-model="deleteAccountDialog"
    @cancel="deleteAccountDialog = false"
    @delete="deleteAccount"
    >diesen Account</BaseDeleteDialog
  >
</template>

<script setup lang="ts">
import { ref } from "vue";
import PasswordDialog from "@/components/Settings/PasswordDialog.vue";
import EmailDialog from "@/components/Settings/EmailDialog.vue";
import BaseDeleteDialog from "@/components/BaseComponents/BaseDeleteDialog.vue";
import { useUsersStore } from "@/store/users";
import { useAuthenticationStore } from "@/store/authentication";

const changePasswordDialog = ref(false);
const changeEmailDialog = ref(false);
const deleteAccountDialog = ref(false);

const usersStore = useUsersStore();
const authStore = useAuthenticationStore();

function deleteAccount() {
  // usersStore.deleteUser();
  authStore.logout();
}
</script>
