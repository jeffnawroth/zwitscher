<template>
  <v-card
    :prepend-avatar="post.avatar"
    :title="`${post.firstName} ${post.lastName}`"
    :subtitle="`@${post.username}`"
    :text="post.text"
  >
    <v-card-actions>
      <v-btn prepend-icon="mdi-thumb-up-outline">{{ post.upvotes }}</v-btn>
      <v-btn prepend-icon="mdi-thumb-down-outline">{{ post.downvotes }}</v-btn>
      <v-btn prepend-icon="mdi-comment-outline">3</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { Post } from "@/interfaces";
import { PropType } from "vue";

defineProps({
  post: {
    type: Object as PropType<Post>,
    required: true,
  },
});
</script>
