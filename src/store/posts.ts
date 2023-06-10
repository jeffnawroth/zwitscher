import { defineStore } from "pinia";
import { ref } from "vue";
import { PostAdd } from "@/interfaces";
import { useAuthenticationStore } from "./authentication";
import { computed } from "vue";
import { showNotification, sortByDateDescending } from "./helpers";
import {
  createNewPost,
  getAllPostsFromUser,
  getAllPublicPosts,
  getPostsFromFollowedUsers,
  getSinglePost,
  modifyPost,
  removePost,
} from "@/dummyApi";
import { Post, PostApi } from "@/typescript-axios-generated";

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<Post[]>([]);
  const postsOfUser = ref<Post[]>([]);
  const postsFollowedUsers = ref<Post[]>([]);
  const post = ref<Post | undefined>();

  async function getAllPosts() {
    try {
      const data = await PostApi.prototype.apiPostGet();
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der öffentlichen Beiträge ist ein Fehler aufgetreten!"
      );
    }
  }

  async function getPostsForUser(id: string) {
    try {
      const data = await getAllPostsFromUser(id);
      postsOfUser.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!"
      );
    }
  }

  async function getFollowedUsersPosts() {
    const authStore = useAuthenticationStore();
    try {
      const data = await getPostsFromFollowedUsers(authStore.user!.following);
      postsFollowedUsers.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!"
      );
    }
  }

  async function getPost(id: string) {
    try {
      const data = await getSinglePost(id);
      post.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

  async function createPost(post: PostAdd) {
    try {
      const data = await createNewPost(post);
      allPosts.value?.push(data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

  async function addComment(comment: PostAdd) {
    try {
      const data = await createNewPost(comment);
      post.value?.comments?.push(data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Kommentars ist ein Fehler aufgetreten!"
      );
    }
  }

  async function deletePost(id: string) {
    try {
      await removePost(id);
      allPosts.value = allPosts.value.filter((post) => post.id !== id);
      showNotification("success", "Der Beitrag wurde erfolgreich gelöscht!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Löschen des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

  async function updatePost(post: Post) {
    try {
      await modifyPost(post);
      const index = allPosts.value.findIndex((x) => x.id === post.id);
      if (index > -1) allPosts.value.splice(index, 1, post);
      showNotification("success", "Der Beitrag wurde erfolgreich bearbeitet!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Bearbeiten des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

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
    updatePost,
  };
});
