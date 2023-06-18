<template>
  <v-list nav>
    <v-list-item
      v-for="user in store.followedUsers"
      :key="user.id"
      :title="user.name"
      :subtitle="user.username"
      :to="`/${user.username}`"
    >
      <template #prepend>
        <v-avatar v-if="!user.avatar" size="40" color="grey">
          <v-icon icon="mdi-account-circle" size="40"></v-icon>
        </v-avatar>
        <v-img v-else>
          <!-- <v-avatar :image="generateFileURL(post?.avatar)"> </v-avatar> -->
        </v-img>
      </template>
      <!-- :prepend-avatar="user.avatar ? user.avatar : 'mdi-account-circle'" -->
    </v-list-item>
  </v-list>
</template>

<script setup lang="ts">
import { useAuthenticationStore } from "@/store/authentication";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";

const store = useUsersStore();
const authStore = useAuthenticationStore();

onMounted(() => {
  store.fetchFollowedUsers("jkldsjfklsjfkljsdlkjfsdklf");
});
</script>
