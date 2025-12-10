<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import PageToolbar from '@/components/PageToolbar.vue'
import CreatePost from '@/components/Posts/CreatePost.vue'
import PostList from '@/components/Posts/PostList.vue'
import { useAuthenticationStore } from '@/store/authentication'
import { usePostStore } from '@/store/posts'

const store = usePostStore()
const authStore = useAuthenticationStore()
// Test
const tab = ref('one')

onMounted(() => {
  store.getAllPosts()
})

watch(
  () => authStore.loggedIn,
  (newVal) => {
    if (!newVal)
      tab.value = 'one'
  },
)

/**
 * Load posts based on tab (public/for you) selection
 */
watch(tab, (newVal) => {
  if (newVal === 'two') {
    store.getFollowedUsersPosts()
  }
  else {
    store.getAllPosts()
  }
})
</script>

<template>
  <PageToolbar icon="mdi-home" title="Startseite" />
  <v-tabs
    v-if="authStore.loggedIn"
    v-model="tab"
    align-tabs="center"
    fixed-tabs
  >
    <v-tab value="one">
      Öffentlich
    </v-tab>
    <v-tab value="two">
      Folge ich
    </v-tab>
  </v-tabs>
  <v-list rounded="lg">
    <div v-if="authStore.loggedIn">
      <v-list-item>
        <CreatePost />
      </v-list-item>
      <v-divider />
    </div>

    <v-window v-model="tab">
      <v-window-item value="one">
        <PostList :posts="store.sortedPosts" />
      </v-window-item>
      <v-window-item value="two">
        <PostList :posts="store.sortedPostsFollowedUsers" />
      </v-window-item>
    </v-window>
  </v-list>
  <router-view />
</template>
