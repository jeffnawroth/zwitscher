<template>
  <PageToolbar icon="mdi-home" title="Startseite"></PageToolbar>
  <v-tabs
    v-if="authStore.loggedIn"
    v-model="tab"
    align-tabs="center"
    fixed-tabs
  >
    <v-tab value="one">Öffentlich</v-tab>
    <v-tab value="two">Folge ich</v-tab>
  </v-tabs>
  <v-list rounded="lg">
    <div v-if="authStore.loggedIn">
      <v-list-item>
        <CreatePost></CreatePost>
      </v-list-item>
      <v-divider></v-divider>
    </div>

    <v-window v-model="tab">
      <v-window-item value="one">
        <PostList :posts="store.sortedPosts"></PostList>
      </v-window-item>
      <v-window-item value="two">
        <PostList :posts="store.sortedPostsFollowedUsers"></PostList>
      </v-window-item>
    </v-window>
  </v-list>
  <router-view></router-view>
</template>

<script setup lang="ts">
import CreatePost from "@/components/Posts/CreatePost.vue";
import { usePostStore } from "@/store/posts";
import { onMounted, ref, watch } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import PostList from "@/components/Posts/PostList.vue";
import PageToolbar from "@/components/PageToolbar.vue";

const store = usePostStore();
const authStore = useAuthenticationStore();
//Test
const tab = ref("one");

onMounted(() => {
  store.getAllPosts();
  if (authStore.loggedIn) {
    store.getFollowedUsersPosts();
  }
});

watch(
  () => authStore.loggedIn,
  (newVal) => {
    if (!newVal) tab.value = "one";
  }
);

watch(tab, (newVal) => {
  if (newVal === "two") {
    if (authStore.loggedIn) {
      store.getFollowedUsersPosts();
    }
  }
});
</script>
