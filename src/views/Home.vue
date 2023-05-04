<template>
  <v-list rounded="lg">
    <v-list-item v-if="authStore.loggedIn">
      <CreatePost></CreatePost>
    </v-list-item>
    <v-divider></v-divider>

    <PostList :posts="store.sortedPosts"></PostList>
  </v-list>
  <router-view></router-view>
</template>

<script setup lang="ts">
import CreatePost from "@/components/Posts/CreatePost.vue";
import { usePostStore } from "@/store/posts";
import { onMounted } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import PostList from "@/components/Posts/PostList.vue";

const store = usePostStore();
const authStore = useAuthenticationStore();

onMounted(() => {
  store.getAllPosts();
});
</script>
