<script setup lang="ts">
import { onMounted } from 'vue'
import { generateFileURL } from '@/helpers'
import { useUsersStore } from '@/store/users'

const store = useUsersStore()

onMounted(() => {
  store.fetchFollowedUsers()
})
</script>

<template>
  <v-list nav>
    <v-list-item
      v-if="store.loadingFollowedUsers"
      class="d-flex justify-center"
    >
      <v-progress-circular
        :indeterminate="store.loadingFollowedUsers"
      />
    </v-list-item>
    <v-list-item
      v-for="user in store.sortedFollowedUsers"
      :key="user.id!"
      :title="user.name!"
      :subtitle="`@${user.username!}`"
      :to="`/${user.username}`"
    >
      <template #prepend>
        <v-avatar v-if="!user.avatar" color="grey">
          <v-icon icon="mdi-account-circle" size="x-large" />
        </v-avatar>
        <v-avatar v-else :image="generateFileURL(user?.avatar)" />
      </template>
    </v-list-item>
  </v-list>
</template>
