import { defineStore } from "pinia";
import { allPosts as posts, userPosts } from "@/dummyData";
import { ref } from "vue";
import { AddPost, Post } from "@/interfaces";
import { useAuthenticationStore } from "./authentication";
import { computed } from "vue";
import { sortByDateDescending } from "./helpers";

const authStore = useAuthenticationStore();

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<Post[]>([]);
  const postsOfUser = ref<Post[]>([]);

  function getAllPosts() {
    allPosts.value = posts;
  }

  function getPostsForUser(id: number) {
    postsOfUser.value = userPosts;
  }

  function createPost(postAdd: AddPost) {
    //todo

    const post: Post = {
      ...postAdd,
      id: 0,
      upvotes: 0,
      downvotes: 0,
      userId: authStore.user!.id,
    };
    allPosts.value?.push(post);
  }

  function deletePost(id: number) {
    const userPostIndex = postsOfUser.value.findIndex((post) => post.id === id);
    const allPostsIndex = allPosts.value.findIndex((post) => post.id === id);
    postsOfUser.value.splice(userPostIndex, 1);
    allPosts.value.splice(allPostsIndex, 1);
  }

  function updatePost(post: Post) {}

  const sortedPosts = computed(() => {
    return sortByDateDescending(allPosts.value);
  });

  const sortedUserPosts = computed(() => {
    return sortByDateDescending(postsOfUser.value);
  });

  return {
    getAllPosts,
    allPosts,
    createPost,
    getPostsForUser,
    postsOfUser,
    deletePost,
    sortedUserPosts,
    sortedPosts,
  };
});
