<template>
  <v-list rounded="lg">
    <v-list-item>
      <v-card :prepend-avatar="authStore.user?.avatar">
        <template #title>
          {{ `${authStore.user?.firstName} ${authStore.user?.lastName}` }}
        </template>
        <template #subtitle>
          <p>{{ `@${authStore.user?.username}` }}</p>
          <p class="text-bold">
            <span class="font-weight-black">
              {{ `${authStore.user?.follower}` }}
            </span>
            Abonnenten

            <span class="font-weight-black">{{
              `${authStore.user?.following}`
            }}</span>
            Folge ich
          </p>
        </template>
        <template #text>
          {{ authStore.user?.bio }}
        </template>
      </v-card>
    </v-list-item>
    <v-divider></v-divider>

    <div v-for="post in store.sortedUserPosts" :key="post.id">
      <v-list-item>
        <Post
          :post="post"
          @set-upvotes="(upvotes) => (post.upvotes = upvotes)"
          @set-downvotes="(downvotes) => (post.downvotes = downvotes)"
        ></Post>
      </v-list-item>
      <v-divider></v-divider>
    </div>
  </v-list>
</template>

<script setup lang="ts">
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { onMounted } from "vue";
import Post from "@/components/Post.vue";

const store = usePostStore();
const authStore = useAuthenticationStore();

onMounted(() => {
  store.getPostsForUser(authStore.user!.id);
});
</script>
