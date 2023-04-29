<template>
  <v-btn @click="logoutUser">Logout</v-btn>
  <span class="text-h1">Home Sweet Home</span>
</template>

<script lang="ts" setup>
import { useAuthenticationStore } from "@/store/authentication";

const store = useAuthenticationStore();

function logoutUser() {
  store.logout();
}
</script>
