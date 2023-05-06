<template>
  <v-list>
    <v-list-item>
      <Post :post="store.post!"></Post>
    </v-list-item>
    <v-divider></v-divider>
    <v-list-item>
      <CreatePost></CreatePost>
    </v-list-item>
    <v-divider></v-divider>
    <PostList :posts="store.post?.comments"></PostList>
  </v-list>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import Post from "@/components/Posts/Post.vue";
import CreatePost from "@/components/Posts/CreatePost.vue";
import PostList from "@/components/Posts/PostList.vue";

const store = usePostStore();
</script>
