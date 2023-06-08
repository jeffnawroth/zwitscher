<template>
  <div class="notification-container">
    <NotificationAlert
      v-for="notification in store.notifications"
      :key="notification.id"
      :notification="notification"
      class="my-1"
    />
  </div>
</template>

<script lang="ts" setup>
import { useNotificationStore } from "@/store/notification";
import NotificationAlert from "./NotificationAlert.vue";

const store = useNotificationStore();
</script>

<style>
.notification-container {
  top: 0;
  right: 0;
  position: absolute;
  margin: 12px;
}
</style>
