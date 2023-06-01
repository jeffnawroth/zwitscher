import { NotificationAlert } from "@/interfaces";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useNotificationStore = defineStore("notification", () => {
  const notifications = ref<NotificationAlert[]>([]);

  function addNotification(notification: NotificationAlert) {
    notifications.value.push(notification);
  }

  function deleteNotification(notification: NotificationAlert) {
    const index = notifications.value.indexOf(notification);
    if (index > -1) notifications.value.splice(index, 1);
  }

  return { notifications, addNotification, deleteNotification };
});
