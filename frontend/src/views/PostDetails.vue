<script setup lang="ts">
import PageToolbar from '@/components/PageToolbar.vue'
import CreatePost from '@/components/Posts/CreatePost.vue'
import Post from '@/components/Posts/Post.vue'
import PostList from '@/components/Posts/PostList.vue'
import { useAuthenticationStore } from '@/store/authentication'
import { usePostStore } from '@/store/posts'

const store = usePostStore()
const authStore = useAuthenticationStore()
</script>

<template>
  <PageToolbar icon="mdi-post-outline" title="Beitrag" back-button />
  <Post :post="store.post!" />
  <v-divider />
  <CreatePost v-if="authStore.loggedIn" />
  <v-divider />
  <PostList
    :posts="store.post?.comments ?? []"
    no-posts-message="Der Beitrag hat noch keine Kommentare."
  />
</template>
