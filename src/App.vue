<template>
  <v-app>
    <v-main><router-view></router-view></v-main>
  </v-app>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import axios from "axios";

const store = useAuthenticationStore();

onMounted(() => {
  const userString = localStorage.getItem("user");
  if (userString) {
    const userData = JSON.parse(userString);
    store.setUserData(userData);
  }
  /*  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response.status === 401) {
        store.logout();
      }
      return Promise.reject(error);
    }
  ); */
});
</script>
