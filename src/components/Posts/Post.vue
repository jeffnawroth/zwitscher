<template>
  <v-card
    v-if="!editMode"
    :subtitle="`@${post.username} • ${formattedDate}  ${
      post.edited ? '• bearbeitet' : ''
    }`"
    density="compact"
    rounded="lg"
    flat
    @click="openPost"
  >
    <template #prepend>
      <v-btn icon variant="text" @click.stop="openProfile">
        <v-avatar v-if="!post.avatar" color="grey">
          <v-icon icon="mdi-account-circle" size="x-large"></v-icon>
        </v-avatar>
        <v-img v-else>
          <v-avatar :image="generateFileURL(post.avatar as unknown as File)">
          </v-avatar>
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
        v-if="post.files"
        :class="post.text === '' ? '' : 'mt-1'"
        :files="post.files"
      ></FileLayout>
    </template>
    <v-card-actions>
      <v-btn :prepend-icon="thumbUp" @click.stop="likePost">{{
        post.upVotes?.length ?? 0
      }}</v-btn>
      <v-btn :prepend-icon="thumbDown" @click.stop="dislikePost">{{
        post.downVotes?.length ?? 0
      }}</v-btn>
      <v-btn prepend-icon="mdi-comment-outline">{{
        //@ts-expect-error
        post.comments?.length ?? 0
      }}</v-btn>

      <v-spacer></v-spacer>
      <v-btn
        v-if="
          authStore.loggedIn &&
          (post.userId === authStore.user?.id ||
            authStore.user?.role == Role.NUMBER_0)
        "
        icon="mdi-pencil-outline"
        @click.stop="editMode = true"
      ></v-btn>
      <v-btn
        v-if="
          authStore.loggedIn &&
          (post.userId === authStore.user?.id ||
            authStore.user?.role == Role.NUMBER_0 ||
            (authStore.user?.role == Role.NUMBER_1 &&
              post.userRole == Role.NUMBER_2))
        "
        icon="mdi-delete-outline"
        @click.stop="deleteDialog = true"
      ></v-btn>
    </v-card-actions>
  </v-card>

  <CreatePost
    v-else
    :post="post"
    :edit-mode="editMode"
    @set-edit-mode="(value) => (editMode = value)"
  ></CreatePost>

  <BaseDeleteDialog
    v-model="deleteDialog"
    :loading="store.crudCardLoading"
    @delete="deleteUserPost"
    @cancel="deleteDialog = false"
    >den Beitrag</BaseDeleteDialog
  >
</template>

<script setup lang="ts">
import { PostResult } from "@/typescript-axios-generated";
import { useAuthenticationStore } from "@/store/authentication";
import BaseDeleteDialog from "../BaseComponents/BaseDeleteDialog.vue";
import { usePostStore } from "@/store/posts";
import { PropType, computed, ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import FileLayout from "./FileLayout.vue";
import { generateFileURL } from "@/helpers";
import { Role } from "@/typescript-axios-generated";
import CreatePost from "./CreatePost.vue";

const props = defineProps({
  post: {
    type: Object as PropType<PostResult>,
    required: true,
  },
});

const store = usePostStore();
const authStore = useAuthenticationStore();
const router = useRouter();
const route = useRoute();
const deleteDialog = ref(false);
const editMode = ref(false);

const thumbUp = computed(() => {
  return props.post.upVotes?.includes(authStore.user?.id!)
    ? "mdi-thumb-up"
    : "mdi-thumb-up-outline";
});

const thumbDown = computed(() => {
  return props.post.downVotes?.includes(authStore.user?.id!)
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
  store.upvotePost(props.post.id!);
}

function dislikePost() {
  if (!authStore.loggedIn) {
    router.push({ name: "login" });
    return;
  }
  store.downvotePost(props.post.id!);
}

async function deleteUserPost() {
  await store.deletePost(props.post.id!);
  deleteDialog.value = false;
}

function openPost() {
  store.post = props.post;
  router.push({
    name: "post",
    params: { username: props.post.username, postId: props.post.id },
  });
}

const formattedDate = computed(() => {
  const date = new Date(props.post.date!);
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
