<template>
  <v-card :subtitle="`@${post.username} • ${formattedDate}`" :text="post.text">
    <template #prepend>
      <v-img>
        <v-avatar
          class="avatar"
          :image="post?.avatar"
          @click.stop="openProfile"
        ></v-avatar>
      </v-img>
    </template>
    <template #title>
      <span class="title" @click.stop="openProfile">{{
        `${post.firstName} ${post.lastName}`
      }}</span>
    </template>
    <v-card-actions>
      <v-btn :prepend-icon="thumbUp" @click.stop="likePost">{{
        post.upvotes
      }}</v-btn>
      <v-btn :prepend-icon="thumbDown" @click.stop="dislikePost">{{
        post.downvotes
      }}</v-btn>
      <v-btn prepend-icon="mdi-comment-outline">3</v-btn>

      <v-spacer></v-spacer>
      <v-btn
        v-if="post.userId === user?.id"
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
import router from "@/router";

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
const { user } = useAuthenticationStore();
const deleteDialog = ref(false);

const thumbUp = computed(() => {
  return user?.liked.includes(props.post.id)
    ? "mdi-thumb-up"
    : "mdi-thumb-up-outline";
});

const thumbDown = computed(() => {
  return user?.disliked.includes(props.post.id)
    ? "mdi-thumb-down"
    : "mdi-thumb-down-outline";
});

function openProfile() {
  router.push({ name: "profile", params: { username: props.post.username } });
}

function likePost() {
  const likedIndex = user?.liked.indexOf(props.post.id);
  const dislikedIndex = user?.disliked.indexOf(props.post.id);

  if (likedIndex != undefined && likedIndex !== -1) {
    user?.liked.splice(likedIndex, 1);
    emit("set-upvotes", props.post.upvotes - 1);
  } else {
    user?.liked.push(props.post.id);
    emit("set-upvotes", props.post.upvotes + 1);
    if (dislikedIndex != undefined && dislikedIndex !== -1) {
      user?.disliked.splice(dislikedIndex, 1);
      emit("set-downvotes", props.post.downvotes - 1);
    }
  }
}

function dislikePost() {
  const likedIndex = user?.liked.indexOf(props.post.id);
  const dislikedIndex = user?.disliked.indexOf(props.post.id);

  if (dislikedIndex != undefined && dislikedIndex !== -1) {
    user?.disliked.splice(dislikedIndex, 1);
    emit("set-downvotes", props.post.downvotes - 1);
  } else {
    user?.disliked.push(props.post.id);
    emit("set-downvotes", props.post.downvotes + 1);
    if (likedIndex != undefined && likedIndex !== -1) {
      user?.liked.splice(likedIndex, 1);
      emit("set-upvotes", props.post.upvotes - 1);
    }
  }
}

function deleteUserPost() {
  store.deletePost(props.post.id);
}

const formattedDate = computed(() => {
  const { date } = props.post;
  const now = new Date();
  const diff = now.getTime() - date.getTime();
  const diffInSeconds = Math.round(diff / 1000);
  const diffInMinutes = Math.round(diff / (1000 * 60));
  const diffInHours = Math.round(diff / (1000 * 60 * 60));

  if (diffInSeconds < 60) {
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

<style>
.title:hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>

<style>
.avatar:hover {
  cursor: pointer;
}
</style>
