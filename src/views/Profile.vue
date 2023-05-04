<template>
  <v-list rounded="lg">
    <v-list-item>
      <v-card :prepend-avatar="usersStore.user?.avatar">
        <template #title>
          {{ `${usersStore.user?.firstName} ${usersStore.user?.lastName}` }}
        </template>
        <template #subtitle>
          <p>{{ `@${usersStore.user?.username}` }}</p>
          <p class="text-bold">
            <span class="font-weight-black">
              {{ `${usersStore.user?.follower}` }}
            </span>
            Abonnenten

            <span class="font-weight-black">{{
              `${usersStore.user?.following}`
            }}</span>
            Folge ich
          </p>
        </template>
        <template #text>
          {{ usersStore.user?.bio }}
        </template>
      </v-card>
    </v-list-item>
    <v-divider></v-divider>

    <PostList :posts="store.sortedUserPosts"></PostList>
  </v-list>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import { onMounted } from "vue";
import Post from "@/components/Posts/Post.vue";
import { useUsersStore } from "@/store/users";
import { onBeforeRouteUpdate } from "vue-router";
import PostList from "@/components/Posts/PostList.vue";

const store = usePostStore();
const usersStore = useUsersStore();

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.id !== from.params.id) {
    usersStore.getUser(Number(to.params.id));
    store.getPostsForUser(usersStore.user!.id);
  }
});

onMounted(() => {
  store.getPostsForUser(usersStore.user!.id);
});
</script>
