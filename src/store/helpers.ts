import { NotificationAlert, Post } from "@/interfaces";
import { v4 as uuidv4 } from "uuid";
import { useNotificationStore } from "./notification";

export function sortByDateDescending(posts: Post[]) {
  return posts.sort((a: Post, b: Post) => {
    const dateA = new Date(a.date);
    const dateB = new Date(b.date);
    return dateB.getTime() - dateA.getTime();
  });
}

export function showNotification(
  type: "error" | "success" | "warning" | "info" | undefined,
  text: string
) {
  const store = useNotificationStore();
  const notification: NotificationAlert = {
    id: uuidv4(),
    type: type,
    text: text,
  };
  store.addNotification(notification);
}
