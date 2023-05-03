import { Post } from "@/interfaces";

export function sortByDateDescending(posts: Post[]) {
  return posts.sort((a: Post, b: Post) => {
    // Convert dates to UTC and compare them in reverse order
    return (
      Date.UTC(
        b.date.getFullYear(),
        b.date.getMonth(),
        b.date.getDate(),
        b.date.getHours(),
        b.date.getMinutes()
      ) -
      Date.UTC(
        a.date.getFullYear(),
        a.date.getMonth(),
        a.date.getDate(),
        a.date.getHours(),
        a.date.getMinutes()
      )
    );
  });
}
