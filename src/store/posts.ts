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
  const post = ref<Post | undefined>();

  function getAllPosts() {
    allPosts.value = posts;
  }

  function getPostsForUser(id: number) {
    postsOfUser.value = posts.filter((post) => post.userId === id);
  }

  function getPost(id: number) {
    post.value = allPosts.value.find((post) => post.id == id);
  }

  function createPost(postAdd: AddPost) {
    //todo

    const post: Post = {
      ...postAdd,

      id: 0,
      upvotes: 0,
      downvotes: 0,
      firstName: "Admin",
      lastName: "Nimda",
      username: "ANimda",
      date: new Date(),
      avatar: authStore.user?.avatar,
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
    getPost,
    post,
  };
});
