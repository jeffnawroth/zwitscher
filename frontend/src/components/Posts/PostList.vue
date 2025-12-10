<script setup lang="ts">
import type { PropType } from 'vue'
import type { PostResult } from '@/typescript-axios-generated'
import { useRoute } from 'vue-router'
import { usePostStore } from '@/store/posts'
import Post from './Post.vue'

defineProps({
  posts: {
    type: Array as PropType<Array<PostResult>>,
    default: () => [],
  },
  noPostsMessage: {
    type: String,
    default: 'Es wurden noch keine Beiträge veröffentlicht.',
  },
})
const store = usePostStore()
const route = useRoute()
</script>

<template>
  <v-card :loading="store.loading" flat>
    <v-card-text v-if="posts.length !== 0">
      <template v-for="(post, index) in posts" :key="post.id!">
        <Post :post="post" />
        <template v-if="route.name === 'profile'">
          <template v-for="comment in post.comments" :key="comment.id!">
            <v-timeline truncate-line="end">
              <v-timeline-item size="small" width="750">
                <Post :post="comment" />
              </v-timeline-item>
            </v-timeline>
          </template>
        </template>
        <v-divider v-if="index !== posts.length - 1" />
      </template>
    </v-card-text>
    <v-card-text v-else class="d-flex justify-center">
      {{ noPostsMessage }}
    </v-card-text>
  </v-card>
</template>

<style>
.vl {
  border-left: 6px solid green;
  height: 500px;
  left: 50%;
  margin-left: -3px;
  top: 0;
}
</style>
