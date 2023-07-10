<template>
  <PageToolbar icon="mdi-cog" title="Einstellungen"></PageToolbar>
  <v-list>
    <v-list-item
      prepend-icon="mdi-email-edit-outline"
      title="E-Mail ändern"
      @click="changeEmailDialog = true"
    ></v-list-item>
    <v-divider></v-divider>
    <v-list-item
      prepend-icon="mdi-lock-reset"
      title="Passwort ändern"
      @click="changePasswordDialog = true"
    >
    </v-list-item>
    <v-divider></v-divider>
    <v-list-item @click="deleteAccountDialog = true">
      <template #prepend>
        <v-icon color="red">mdi-account-remove-outline</v-icon>
      </template>
      <template #title>
        <span class="text-red">Konto löschen</span>
      </template></v-list-item
    >
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
import PageToolbar from "@/components/PageToolbar.vue";

const changePasswordDialog = ref(false);
const changeEmailDialog = ref(false);
const deleteAccountDialog = ref(false);

const usersStore = useUsersStore();
const authStore = useAuthenticationStore();

/**
 * Delete account and push to login
 */
async function deleteAccount() {
  await usersStore.deleteUser(authStore.user?.id!);
  authStore.logout();
}
</script>
