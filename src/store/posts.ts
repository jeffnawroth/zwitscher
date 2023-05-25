import { defineStore } from "pinia";
import {
  allPosts as posts,
  followedUsersPosts as postsOfFollowedUsers,
} from "@/dummyData";
import { ref } from "vue";
import { AddPost, Post } from "@/interfaces";
import { useAuthenticationStore } from "./authentication";
import { computed } from "vue";
import { sortByDateDescending } from "./helpers";

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<Post[]>(posts);
  const postsOfUser = ref<Post[]>([]);
  const postsFollowedUsers = ref<Post[]>([]);
  const post = ref<Post | undefined>();

  function getAllPosts() {
    allPosts.value = posts;
  }

  function getPostsForUser(id: number) {
    postsOfUser.value = posts.filter((post) => post.userId === id);
  }

  function getFollowedUsersPosts(id: number) {
    postsFollowedUsers.value = postsOfFollowedUsers;
  }

  function getPost(id: number) {
    post.value = allPosts.value.find((post) => post.id == id);
  }

  function createPost(postAdd: AddPost) {
    const authStore = useAuthenticationStore();
    //todo

    const post: Post = {
      id: 10,
      userId: postAdd.userId,
      upvotes: 0,
      downvotes: 0,
      firstName: "Admin",
      lastName: "Nimda",
      username: "ANimda",
      date: new Date(),
      avatar: authStore.user?.avatar,
      comments: [],
      files: postAdd.files ?? [],
      text: postAdd.text ?? "",
    };
    allPosts.value?.push(post);
  }

  function addComment(comment: AddPost) {
    const authStore = useAuthenticationStore();

    const postAdd: Post = {
      id: 11,
      userId: comment.userId,
      upvotes: 0,
      downvotes: 0,
      firstName: "Admin",
      lastName: "Nimda",
      username: "ANimda",
      date: new Date(),
      avatar: authStore.user?.avatar,
      comments: [],
      files: comment.files ?? [],
      text: comment.text ?? "",
    };

    post.value?.comments?.push(postAdd);
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

  const sortedPostsFollowedUsers = computed(() => {
    return sortByDateDescending(postsFollowedUsers.value);
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
    addComment,
    getFollowedUsersPosts,
    sortedPostsFollowedUsers,
  };
});
