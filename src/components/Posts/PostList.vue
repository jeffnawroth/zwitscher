<template>
  <div v-for="(post, index) in posts" :key="post.id">
    <v-list-item @click="openPost(post)">
      <Post
        :post="post"
        @set-upvotes="(upvotes: number) => (post.upVotes = upvotes)"
        @set-downvotes="(downvotes: number) => (post.downVotes = downvotes)"
      ></Post>
    </v-list-item>
    <v-divider v-if="index !== posts.length - 1"></v-divider>
  </div>
</template>

<script setup lang="ts">
import { Post as IPost } from "@/interfaces";
import { PropType } from "vue";
import Post from "./Post.vue";
import { useRouter } from "vue-router";
import { usePostStore } from "@/store/posts";

defineProps({
  posts: {
    type: Array as PropType<Array<IPost>>,
    default: () => {
      [];
    },
  },
});

const store = usePostStore();
const router = useRouter();

function openPost(post: IPost) {
  store.post = post;
  router.push({
    name: "post",
    params: { username: post.username, postId: post.id },
  });
}
</script>
