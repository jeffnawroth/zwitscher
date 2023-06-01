<template>
  <v-card :subtitle="`@${post.username} • ${formattedDate}`">
    <template #prepend>
      <v-btn icon variant="text" @click.stop="openProfile">
        <v-avatar v-if="!post.avatar" size="40" color="grey">
          <v-icon icon="mdi-account-circle" size="40"></v-icon>
        </v-avatar>
        <v-img v-else>
          <v-avatar :image="generateFileURL(post?.avatar)"> </v-avatar>
        </v-img>
      </v-btn>
    </template>
    <template #title>
      <span
        :class="{ title: route.name != 'profile' }"
        @click.stop="openProfile"
        >{{ `${post.name}` }}</span
      >
    </template>
    <template #text>
      {{ post.text }}
      <FileLayout
        :class="post.text === '' ? '' : 'mt-1'"
        :files="post.files"
      ></FileLayout>
    </template>
    <v-card-actions>
      <v-btn :prepend-icon="thumbUp" @click.stop="likePost">{{
        post.upvotes
      }}</v-btn>
      <v-btn :prepend-icon="thumbDown" @click.stop="dislikePost">{{
        post.downvotes
      }}</v-btn>
      <v-btn prepend-icon="mdi-comment-outline">{{
        post.comments?.length ?? 0
      }}</v-btn>

      <v-spacer></v-spacer>
      <v-btn
        v-if="
          authStore.loggedIn &&
          (post.userId === authStore.user?.id ||
            authStore.user?.role == Role.NUMBER_0 ||
            authStore.user?.role == Role.NUMBER_1)
        "
        icon="mdi-delete-outline"
        @click.stop="deleteDialog = true"
      ></v-btn>
    </v-card-actions>
  </v-card>

  <BaseDeleteDialog
    v-model="deleteDialog"
    @delete="deleteUserPost"
    @cancel="deleteDialog = false"
    >den Beitrag</BaseDeleteDialog
  >
</template>

<script setup lang="ts">
import { Post } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import BaseDeleteDialog from "../BaseComponents/BaseDeleteDialog.vue";
import { usePostStore } from "@/store/posts";
import { PropType, computed, ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import FileLayout from "./FileLayout.vue";
import { generateFileURL } from "@/helpers";
import { Role } from "@/typescript-axios-generated";

const emit = defineEmits<{
  (e: "set-upvotes", upvotes: number): void;
  (e: "set-downvotes", downvotes: number): void;
}>();

const props = defineProps({
  post: {
    type: Object as PropType<Post>,
    required: true,
  },
});

const store = usePostStore();
const authStore = useAuthenticationStore();
const router = useRouter();
const route = useRoute();
const deleteDialog = ref(false);

const thumbUp = computed(() => {
  return authStore.user?.likedPosts?.includes(props.post.id)
    ? "mdi-thumb-up"
    : "mdi-thumb-up-outline";
});

const thumbDown = computed(() => {
  return authStore.user?.dislikedPosts?.includes(props.post.id)
    ? "mdi-thumb-down"
    : "mdi-thumb-down-outline";
});

function openProfile() {
  router.push({ name: "profile", params: { username: props.post.username } });
}

function likePost() {
  if (!authStore.loggedIn) {
    router.push({ name: "login" });
    return;
  }
  const likedIndex = authStore.user?.likedPosts?.indexOf(props.post.id);
  const dislikedIndex = authStore.user?.dislikedPosts?.indexOf(props.post.id);

  if (likedIndex != undefined && likedIndex !== -1) {
    authStore.user?.likedPosts?.splice(likedIndex, 1);
    emit("set-upvotes", props.post.upvotes - 1);
  } else {
    if (!authStore.user?.likedPosts) authStore.user!.likedPosts = [];
    authStore.user?.likedPosts?.push(props.post.id);
    emit("set-upvotes", props.post.upvotes + 1);
    if (dislikedIndex != undefined && dislikedIndex !== -1) {
      authStore.user?.dislikedPosts?.splice(dislikedIndex, 1);
      emit("set-downvotes", props.post.downvotes - 1);
    }
  }
}

function dislikePost() {
  if (!authStore.loggedIn) {
    router.push({ name: "login" });
    return;
  }
  const likedIndex = authStore.user?.likedPosts?.indexOf(props.post.id);
  const dislikedIndex = authStore.user?.dislikedPosts?.indexOf(props.post.id);

  if (dislikedIndex != undefined && dislikedIndex !== -1) {
    authStore.user?.dislikedPosts?.splice(dislikedIndex, 1);
    emit("set-downvotes", props.post.downvotes - 1);
  } else {
    if (!authStore.user?.dislikedPosts) authStore.user!.dislikedPosts = [];
    authStore.user?.dislikedPosts?.push(props.post.id);
    emit("set-downvotes", props.post.downvotes + 1);
    if (likedIndex != undefined && likedIndex !== -1) {
      authStore.user?.likedPosts?.splice(likedIndex, 1);
      emit("set-upvotes", props.post.upvotes - 1);
    }
  }
}

function deleteUserPost() {
  store.deletePost(props.post.id);
}

const formattedDate = computed(() => {
  const date = new Date(props.post.date);
  const now = new Date();
  const diff = now.getTime() - date.getTime();
  const diffInSeconds = Math.round(diff / 1000);
  const diffInMinutes = Math.round(diff / (1000 * 60));
  const diffInHours = Math.round(diff / (1000 * 60 * 60));

  if (diffInSeconds == 0) {
    return "Jetzt";
  } else if (diffInSeconds < 60) {
    return `${diffInSeconds}s`;
  } else if (diffInMinutes < 60) {
    return `${diffInMinutes}m`;
  } else if (diffInHours < 24) {
    return `${Math.round(diffInHours)}h`;
  } else {
    const year = date.getFullYear();
    const month = date.toLocaleString("default", { month: "long" });
    const day = date.getDate();
    return year !== now.getFullYear()
      ? `${day} ${month} ${year}`
      : `${day} ${month}`;
  }
});
</script>

<style scoped>
.title:hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>
