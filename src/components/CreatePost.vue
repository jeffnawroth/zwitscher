<template>
  <v-card
    :prepend-avatar="authStore.user?.avatar"
    :title="`${authStore.user?.firstName} ${authStore.user?.lastName}`"
    :subtitle="`@${authStore.user?.username}`"
  >
    <v-card-text>
      <v-textarea
        v-model="postText"
        placeholder="Was gibt's neues?"
        variant="outlined"
        clearable
        counter="281"
        hide-details="auto"
        :rows="3"
        auto-grow
        persistent-counter
      ></v-textarea>
    </v-card-text>
    <v-card-actions>
      <v-btn icon="mdi-image-outline"></v-btn>
      <v-btn icon="mdi-file-gif-box"></v-btn>
      <v-btn icon="mdi-emoticon-happy-outline"></v-btn>
      <v-spacer></v-spacer>
      <v-btn variant="tonal" @click="submit">Veröffentlichen</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { AddPost, Post } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { ref } from "vue";

const authStore = useAuthenticationStore();
const postsStore = usePostStore();

const postText = ref("");

function submit() {
  const post: AddPost = {
    avatar: authStore.user?.avatar!,
    firstName: authStore.user!.firstName,
    lastName: authStore.user!.firstName,
    username: authStore.user!.username,
    text: postText.value,
  };
  postsStore.createPost(post);
}
</script>
