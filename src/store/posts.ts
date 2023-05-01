import { defineStore } from "pinia";
import { allPosts as posts } from "@/dummyData";
import { ref } from "vue";
import { AddPost, Post } from "@/interfaces";

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<Post[]>();

  function getAllPosts() {
    allPosts.value = posts;
  }

  function getPostsForUser() {
    //Todo
  }

  function createPost(postAdd: AddPost) {
    //todo

    const post: Post = {
      ...postAdd,
      id: 0,
      upvotes: 0,
      downvotes: 0,
    };
    allPosts.value?.push(post);
  }

  function deletePost() {
    //todo
  }

  return { getAllPosts, posts, createPost };
});
