export interface NotificationAlert {
  id: string;
  type: "error" | "success" | "warning" | "info" | undefined;
  text: string;
}
