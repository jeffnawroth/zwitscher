<template>
  <router-view></router-view>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import { useUsersStore } from "@/store/users";
import { onBeforeRouteUpdate } from "vue-router";

const usersStore = useUsersStore();
const store = usePostStore();

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.username !== from.params.username) {
    usersStore.getUserByUsername(to.params.username as string);
    store.getPostsForUser(usersStore.user!.id);
  }
});
</script>
