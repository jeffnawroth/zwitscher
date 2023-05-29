import { Post } from "@/interfaces";

export function sortByDateDescending(posts: Post[]) {
  return posts.sort((a: Post, b: Post) => b.date.getTime() - a.date.getTime());
}
