<template>
  <v-data-table
    :headers="headers"
    :items="store.allPosts"
    :sort-by="[{ key: 'date', order: 'desc' }]"
    :loading="store.loading"
    :search="search"
  >
    <template #top>
      <v-toolbar :color="theme.current.value.dark ? 'grey-darken-4' : 'white'">
        <v-text-field
          v-model="search"
          density="compact"
          hide-details
          variant="solo-filled"
          flat
          style="max-width: 300px"
          placeholder="Suche..."
        ></v-text-field>
      </v-toolbar>
    </template>

    <template #[`item.upVotes`]="{ item }">
      {{ item.upVotes?.length }}
    </template>
    <template #[`item.downVotes`]="{ item }">
      {{ item.downVotes?.length }}
    </template>
    <template #[`item.edited`]="{ item }">
      {{ item.edited ? "Ja" : "Nein" }}
    </template>
    <template #[`item.date`]="{ item }">
      {{ toLocaleDate(item.date as string) }}
    </template>
    <template #[`item.files`]="{ item }">
      {{ item.files?.length }}
    </template>
    <template #[`item.actions`]="{ item }">
      <IconWithTooltip
        text="Beitrag Detailansicht"
        icon="mdi-open-in-app"
        @click="openPostDetails(item)"
      ></IconWithTooltip>

      <IconWithTooltip
        text="Beitrag löschen"
        icon="mdi-delete"
        @click="openDeleteDialog(item)"
      ></IconWithTooltip>
    </template>
  </v-data-table>
  <router-view v-if="route.name === 'edit-post'"></router-view>
  <BaseDeleteDialog
    v-model="deleteDialog"
    @delete="deletePost"
    @cancel="deleteDialog = false"
    >den Beitrag</BaseDeleteDialog
  >
</template>

<script setup lang="ts">
import { ref } from "vue";
import { onMounted } from "vue";
import IconWithTooltip from "@/components/IconWithTooltip.vue";
import { useRouter, useRoute } from "vue-router";
import { PostResult } from "@/typescript-axios-generated/api";
import { usePostStore } from "@/store/posts";
import BaseDeleteDialog from "./BaseComponents/BaseDeleteDialog.vue";
import { useTheme } from "vuetify";

const store = usePostStore();
const router = useRouter();
const route = useRoute();
const theme = useTheme();
const deleteDialog = ref(false);
const search = ref("");
const headers = [
  { title: "Erstellungsdatum", key: "date" },
  { title: "Benutzername", key: "username" },
  { title: "Likes", key: "upVotes" },
  { title: "Dislikes", key: "downVotes" },
  { title: "Dateien", key: "files" },
  { title: "Bearbeitet", key: "edited" },
  { title: "", key: "actions" },
];

onMounted(() => {
  store.getAllPosts();
});

function toLocaleDate(date: string) {
  return new Date(date).toLocaleDateString("de");
}

/**
 * Open dialog to delete the post
 * @param user
 */
function openDeleteDialog(post?: PostResult) {
  store.post = post;
  deleteDialog.value = true;
}

/**
 * Open post details
 * @param user
 */
function openPostDetails(post: PostResult) {
  router.push({
    name: "post",
    params: { username: post.username, postId: post.id },
  });
}

async function deletePost() {
  await store.deletePost(store.post?.id!);
  deleteDialog.value = false;
}
</script>
