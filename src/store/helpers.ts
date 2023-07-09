import { NotificationAlert } from "@/interfaces";
import { v4 as uuidv4 } from "uuid";
import { useNotificationStore } from "./notification";
import { PostResult } from "@/typescript-axios-generated";

/**
 * Sort posts based on their date
 * @param posts
 */
export function sortByDateDescending(posts: PostResult[]) {
  return posts.sort((a: PostResult, b: PostResult) => {
    const dateA = new Date(a.date!);
    const dateB = new Date(b.date!);
    return dateB.getTime() - dateA.getTime();
  });
}

/**
 * Create a new notification
 * @param type
 * @param text
 */
export function showNotification(
  type: "error" | "success" | "warning" | "info" | undefined,
  text: string,
) {
  const store = useNotificationStore();
  const notification: NotificationAlert = {
    id: uuidv4(),
    type: type,
    text: text,
  };
  store.addNotification(notification);
}
