<template>
  <PageToolbar icon="mdi-post-outline" title="Beitrag" back-button>
  </PageToolbar>
  <Post :post="store.post!"></Post>
  <v-divider></v-divider>
  <CreatePost v-if="authStore.loggedIn"></CreatePost>
  <v-divider></v-divider>
  <PostList
    :posts="store.post?.comments ?? []"
    no-posts-message="Der Beitrag hat noch keine Kommentare."
  ></PostList>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import Post from "@/components/Posts/Post.vue";
import CreatePost from "@/components/Posts/CreatePost.vue";
import PostList from "@/components/Posts/PostList.vue";
import { useAuthenticationStore } from "@/store/authentication";
import PageToolbar from "@/components/PageToolbar.vue";

const store = usePostStore();
const authStore = useAuthenticationStore();
</script>
