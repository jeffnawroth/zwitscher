<template>
  <PageToolbar icon="mdi-post-outline" title="Beitrag" back-button>
  </PageToolbar>
  <v-list rounded="lg">
    <v-list-item>
      <Post
        :post="store.post!"
        @set-upvotes="(upvotes: number) => (store.post!.upVotes = upvotes)"
        @set-downvotes="(downvotes: number) => (store.post!.downVotes = downvotes)"
      ></Post>
    </v-list-item>
    <v-divider></v-divider>
    <v-list-item v-if="authStore.loggedIn">
      <CreatePost></CreatePost>
    </v-list-item>
    <v-divider></v-divider>
    <PostList
      v-if="store.post?.comments?.length && store.post?.comments?.length > 0"
      :posts="store.post?.comments"
    ></PostList>
    <v-list-item v-else class="d-flex justify-center"
      >Der Beitrag hat noch keine Kommentare.</v-list-item
    >
  </v-list>
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
