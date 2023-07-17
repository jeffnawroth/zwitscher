import { NotificationAlert } from "@/interfaces";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useNotificationStore = defineStore("notification", () => {
  const notifications = ref<NotificationAlert[]>([]);

  /**
   * Add new notification to other notifications
   * @param notification
   */
  function addNotification(notification: NotificationAlert) {
    notifications.value.push(notification);
  }

  /**
   * Delete a notification
   * @param notification
   */
  function deleteNotification(notification: NotificationAlert) {
    const index = notifications.value.indexOf(notification);
    if (index > -1) notifications.value.splice(index, 1);
  }

  return { notifications, addNotification, deleteNotification };
});
