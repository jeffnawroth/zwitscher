import { Post } from "@/interfaces";

export function sortByDateDescending(posts: Post[]) {
  return posts.sort((a: Post, b: Post) => {
    const dateA = new Date(a.date);
    const dateB = new Date(b.date);
    return dateB.getTime() - dateA.getTime();
  });
}
