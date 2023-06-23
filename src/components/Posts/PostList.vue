<template>
  <v-card variant="flat" :loading="store.loading">
    <div v-for="(post, index) in posts" :key="post.id!">
      <v-list-item @click="openPost(post)">
        <Post :post="post"></Post>
      </v-list-item>
      <v-divider v-if="index !== posts.length - 1"></v-divider>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { PropType } from "vue";
import Post from "./Post.vue";
import { useRouter } from "vue-router";
import { usePostStore } from "@/store/posts";
import { PostResult } from "@/typescript-axios-generated";

defineProps({
  posts: {
    type: Array as PropType<Array<PostResult>>,
    default: () => {
      [];
    },
  },
});

const store = usePostStore();
const router = useRouter();

function openPost(post: PostResult) {
  store.post = post;
  router.push({
    name: "post",
    params: { username: post.username, postId: post.id },
  });
}
</script>
