<template>
  <div class="notification-container">
    <NotificationAlert
      v-for="notification in store.notifications"
      :key="notification.id"
      :notification="notification"
    />
  </div>
</template>

<script lang="ts" setup>
import { useNotificationStore } from "@/store/notification";
import NotificationAlert from "./NotificationAlert.vue";

const store = useNotificationStore();
</script>
