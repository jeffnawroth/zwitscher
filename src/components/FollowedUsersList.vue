<template>
  <v-list nav>
    <v-list-item
      v-for="user in store.followedUsers"
      :key="user.id!"
      :title="user.name!"
      :subtitle="`@${user.username!}`"
      :to="`/${user.username}`"
    >
      <template #prepend>
        <v-avatar v-if="!user.avatar" color="grey">
          <v-icon icon="mdi-account-circle" size="x-large"></v-icon>
        </v-avatar>
        <v-avatar v-else :image="generateFileURL(user?.avatar)"> </v-avatar>
      </template>
    </v-list-item>
  </v-list>
</template>

<script setup lang="ts">
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import { generateFileURL } from "@/helpers";

const store = useUsersStore();

onMounted(() => {
  store.fetchFollowedUsers();
});
</script>
