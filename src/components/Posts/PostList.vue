<template>
  <div v-for="(post, index) in posts" :key="post.id!">
    <v-list-item>
      <Post :post="post"></Post>
    </v-list-item>
    <v-divider v-if="index !== posts.length - 1"></v-divider>
  </div>
</template>

<script setup lang="ts">
import { PropType } from "vue";
import Post from "./Post.vue";
import { PostResult } from "@/typescript-axios-generated";

defineProps({
  posts: {
    type: Array as PropType<Array<PostResult>>,
    default: () => {
      [];
    },
  },
});
</script>
