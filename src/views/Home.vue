<template>
  <v-list rounded="lg">
    <v-list-item>
      <CreatePost></CreatePost>
    </v-list-item>
    <v-divider></v-divider>

    <div v-for="post in store.posts" :key="post.id">
      <v-list-item>
        <Post :post="post"></Post>
      </v-list-item>
      <v-divider></v-divider>
    </div>
  </v-list>
</template>

<script setup lang="ts">
import Post from "@/components/Post.vue";
import CreatePost from "@/components/CreatePost.vue";
import { usePostStore } from "@/store/posts";
import { onMounted } from "vue";

const store = usePostStore();

onMounted(() => {
  store.getAllPosts();
});
</script>
