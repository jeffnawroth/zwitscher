<template>
  <v-card :loading="store.loading" flat>
    <v-card-text v-if="posts.length !== 0">
      <template v-for="(post, index) in posts" :key="post.id!">
        <Post :post="post"></Post>
        <v-divider v-if="index !== posts.length - 1"></v-divider>
      </template>
    </v-card-text>
    <v-card-text v-else class="d-flex justify-center">
      {{ noPostsMessage }}
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { PropType } from "vue";
import Post from "./Post.vue";
import { PostResult } from "@/typescript-axios-generated";
import { usePostStore } from "@/store/posts";

const store = usePostStore();

defineProps({
  posts: {
    type: Array as PropType<Array<PostResult>>,
    default: () => {
      [];
    },
  },
  noPostsMessage: {
    type: String,
    default: "Es wurden noch keine Beiträge veröffentlicht.",
  },
});
</script>
