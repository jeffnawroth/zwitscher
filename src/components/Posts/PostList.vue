<template>
  <div v-for="post in posts" :key="post.id">
    <v-list-item>
      <Post
        :post="post"
        @set-upvotes="(upvotes: number) => (post.upvotes = upvotes)"
        @set-downvotes="(downvotes: number) => (post.downvotes = downvotes)"
      ></Post>
    </v-list-item>
    <v-divider></v-divider>
  </div>
</template>

<script setup lang="ts">
import { Post as IPost } from "@/interfaces";
import { PropType } from "vue";
import Post from "./Post.vue";

defineProps({
  posts: {
    type: Array as PropType<Array<IPost>>,
    default: () => {
      [];
    },
  },
});
</script>
