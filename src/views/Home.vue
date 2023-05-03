<template>
  <v-list rounded="lg">
    <v-list-item v-if="authStore.loggedIn">
      <CreatePost></CreatePost>
    </v-list-item>
    <v-divider></v-divider>

    <div v-for="post in store.sortedPosts" :key="post.id">
      <v-list-item>
        <Post
          :post="post"
          @set-upvotes="(upvotes) => (post.upvotes = upvotes)"
          @set-downvotes="(downvotes) => (post.downvotes = downvotes)"
        ></Post>
      </v-list-item>
      <v-divider></v-divider>
    </div>
  </v-list>
  <router-view></router-view>
</template>

<script setup lang="ts">
import Post from "@/components/Post.vue";
import CreatePost from "@/components/CreatePost.vue";
import { usePostStore } from "@/store/posts";
import { onMounted } from "vue";
import { useAuthenticationStore } from "@/store/authentication";

const store = usePostStore();
const authStore = useAuthenticationStore();

onMounted(() => {
  store.getAllPosts();
});
</script>
