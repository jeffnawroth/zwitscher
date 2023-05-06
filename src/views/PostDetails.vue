<template>
  <v-list>
    <v-list-item>
      <Post :post="store.post!"></Post>
    </v-list-item>
  </v-list>
  <v-divider></v-divider>
  <v-list-item>
    <CreatePost></CreatePost>
  </v-list-item>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import Post from "@/components/Posts/Post.vue";
import CreatePost from "@/components/Posts/CreatePost.vue";

const store = usePostStore();
</script>
