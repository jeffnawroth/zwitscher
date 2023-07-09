import { defineStore } from "pinia";
import { ref } from "vue";
import { PostAdd } from "@/interfaces";
import { useAuthenticationStore } from "./authentication";
import { computed } from "vue";
import { showNotification, sortByDateDescending } from "./helpers";
import { PostApi, PostResult } from "@/typescript-axios-generated";
import { filesToBase64 } from "@/helpers";

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<PostResult[]>([]);
  const postsOfUser = ref<PostResult[]>([]);
  const postsFollowedUsers = ref<PostResult[]>([]);
  const post = ref<PostResult | undefined>();
  const loading = ref(false);

  async function getAllPosts() {
    try {
      allPosts.value = [];
      loading.value = true;
      const data = await PostApi.prototype.apiPostGet();
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der öffentlichen Beiträge ist ein Fehler aufgetreten!",
      );
    } finally {
      loading.value = false;
    }
  }

  async function getPostsForUser(id: string) {
    try {
      allPosts.value = [];
      loading.value = true;
      const data = await PostApi.prototype.apiPostUserUserIdGet(id);
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!",
      );
    } finally {
      loading.value = false;
    }
  }

  async function getFollowedUsersPosts() {
    try {
      allPosts.value = [];
      loading.value = true;
      const data = await PostApi.prototype.apiPostFollowingPostsGet();
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!",
      );
    } finally {
      loading.value = false;
    }
  }

  async function getPost(id: string) {
    try {
      const data = await PostApi.prototype.apiPostIdGet(id);
      post.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden des Beitrags ist ein Fehler aufgetreten!",
      );
    }
  }

  async function createPost(post: PostAdd) {
    try {
      if (post.files) post.files = await filesToBase64(post.files);
      const data = await PostApi.prototype.apiPostPost(post);
      allPosts.value?.push(data.data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Beitrags ist ein Fehler aufgetreten!",
      );
      return Promise.reject(error);
    }
  }

  async function addComment(comment: PostAdd) {
    try {
      // const data = await createNewPost(comment);
      // post.value?.comments?.push(data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Kommentars ist ein Fehler aufgetreten!",
      );
    }
  }

  async function deletePost(id: string) {
    try {
      await PostApi.prototype.apiPostIdDelete(id);
      allPosts.value = allPosts.value.filter((post) => post.id !== id);
      showNotification("success", "Der Beitrag wurde erfolgreich gelöscht!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Löschen des Beitrags ist ein Fehler aufgetreten!",
      );
    }
  }

  async function updatePost(postUpdate: PostResult) {
    try {
      //@ts-ignore

      if (postUpdate.files)
        postUpdate.files = await filesToBase64(postUpdate.files);
      await PostApi.prototype.apiPostPut(postUpdate);
      postUpdate.edited = true;
      const index = allPosts.value.findIndex((x) => x.id === postUpdate.id);
      if (index > -1) allPosts.value.splice(index, 1, postUpdate);
      if (post.value?.id == postUpdate.id) post.value = postUpdate;
      showNotification("success", "Der Beitrag wurde erfolgreich bearbeitet!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Bearbeiten des Beitrags ist ein Fehler aufgetreten!",
      );
      return Promise.reject(error);
    }
  }

  async function upvotePost(id: string) {
    try {
      await PostApi.prototype.apiPostPostIdUpvotePost(id);
      const postFound = allPosts.value.find((post) => post.id === id)!;
      const authStore = useAuthenticationStore();
      const likedIndex = postFound?.upVotes?.indexOf(authStore.user!.id);
      const dislikedIndex = postFound?.downVotes?.indexOf(authStore.user!.id);
      if (likedIndex !== undefined && likedIndex !== -1) {
        postFound.upVotes?.splice(likedIndex, 1);
        if (post.value?.id === id) post.value.upVotes?.splice(likedIndex, 1);
      } else {
        postFound.upVotes = postFound.upVotes ? postFound.upVotes : [];
        postFound.upVotes?.push(authStore.user!.id);
        post.value?.upVotes?.push(authStore.user?.id!);
        if (dislikedIndex !== undefined && dislikedIndex !== -1) {
          postFound.downVotes?.splice(dislikedIndex, 1);
          if (post.value?.id === id)
            post.value.downVotes?.splice(dislikedIndex, 1);
        }
      }
    } catch (error) {
      showNotification(
        "error",
        "Beim liken des Beitrags ist ein Fehler aufgetreten!",
      );
    }
  }
  async function downvotePost(id: string) {
    try {
      await PostApi.prototype.apiPostPostIdDownvotePost(id);
      const postFound = allPosts.value.find((post) => post.id === id)!;
      const authStore = useAuthenticationStore();
      const likedIndex = postFound?.upVotes?.indexOf(authStore.user!.id);
      const dislikedIndex = postFound?.downVotes?.indexOf(authStore.user!.id);
      if (dislikedIndex !== undefined && dislikedIndex !== -1) {
        postFound.downVotes?.splice(dislikedIndex, 1);
        if (post.value?.id === id)
          post.value.downVotes?.splice(dislikedIndex, 1);
      } else {
        postFound.downVotes = postFound.downVotes ? postFound.downVotes : [];
        postFound.downVotes?.push(authStore.user!.id);
        post.value?.downVotes?.push(authStore.user?.id!);
        if (likedIndex !== undefined && likedIndex !== -1) {
          postFound.upVotes?.splice(likedIndex, 1);
          if (post.value?.id === id) post.value.upVotes?.splice(likedIndex, 1);
        }
      }
    } catch (error) {
      showNotification(
        "error",
        "Beim disliken des Beitrags ist ein Fehler aufgetreten!",
      );
    }
  }

  const sortedPosts = computed(() => {
    return sortByDateDescending(allPosts.value);
  });

  const sortedUserPosts = computed(() => {
    return sortByDateDescending(allPosts.value);
  });

  const sortedPostsFollowedUsers = computed(() => {
    return sortByDateDescending(allPosts.value);
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
    upvotePost,
    downvotePost,
    loading,
  };
});
