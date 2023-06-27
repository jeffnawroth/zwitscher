import { defineStore } from "pinia";
import { ref } from "vue";
import { PostAdd } from "@/interfaces";
import { useAuthenticationStore } from "./authentication";
import { computed } from "vue";
import { showNotification, sortByDateDescending } from "./helpers";
import { PostApi, PostResult } from "@/typescript-axios-generated";

export const usePostStore = defineStore("post", () => {
  const allPosts = ref<PostResult[]>([]);
  const postsOfUser = ref<PostResult[]>([]);
  const postsFollowedUsers = ref<PostResult[]>([]);
  const post = ref<PostResult | undefined>();

  async function getAllPosts() {
    try {
      allPosts.value = []
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
      allPosts.value = []
      const data = await PostApi.prototype.apiPostUserUserIdGet(id);
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!"
      );
    } 
  }

  async function getFollowedUsersPosts() {
    try {
      const data = await PostApi.prototype.apiPostFollowingPostsGet();
      allPosts.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Beiträge ist ein Fehler aufgetreten!"
      );
    }
  }

  async function getPost(id: string) {
    try {
      const data = await PostApi.prototype.apiPostIdGet(id);
      //@ts-expect-error
      post.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

  async function createPost(post: PostAdd) {
    try {
      //@ts-ignore
      const data = await PostApi.prototype.apiPostPost(post);
      allPosts.value?.push(data.data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Beitrags ist ein Fehler aufgetreten!"
      );
      return Promise.reject(error)
    }
  }

  async function addComment(comment: PostAdd) {
    try {
      // const data = await createNewPost(comment);
      // post.value?.comments?.push(data);
    } catch (error) {
      showNotification(
        "error",
        "Beim Erstellen des Kommentars ist ein Fehler aufgetreten!"
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
        "Beim Löschen des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }

  async function updatePost(post: PostResult) {
    try {
      //@ts-ignore
      await PostApi.prototype.apiPostPut(post);
      const index = allPosts.value.findIndex((x) => x.id === post.id);
      //@ts-ignore
      if (index > -1) allPosts.value.splice(index, 1, post);
      showNotification("success", "Der Beitrag wurde erfolgreich bearbeitet!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Bearbeiten des Beitrags ist ein Fehler aufgetreten!"
      );
      return Promise.reject(error)

    }
  }

  async function upvotePost(id: string) {
    try {
      await PostApi.prototype.apiPostPostIdUpvotePost(id);
      const post = allPosts.value.find((post) => post.id === id)!;
      const authStore = useAuthenticationStore();
      const likedIndex = post?.upVotes?.indexOf(authStore.user!.id);
      const dislikedIndex = post?.downVotes?.indexOf(authStore.user!.id);
      if (likedIndex !== undefined && likedIndex !== -1) {
        post.upVotes?.splice(likedIndex, 1);
      } else {
        post.upVotes = post.upVotes? post.upVotes : []
        post.upVotes?.push(authStore.user!.id);
        if (dislikedIndex !== undefined && dislikedIndex !== -1) {
          post.downVotes?.splice(dislikedIndex, 1);
        }
      }
    } catch (error) {
      showNotification(
        "error",
        "Beim liken des Beitrags ist ein Fehler aufgetreten!"
      );
    }
  }
  async function downvotePost(id: string) {
    try {
      await PostApi.prototype.apiPostPostIdDownvotePost(id);
      const post = allPosts.value.find((post) => post.id === id)!;
      const authStore = useAuthenticationStore();
      const likedIndex = post?.upVotes?.indexOf(authStore.user!.id);
      const dislikedIndex = post?.downVotes?.indexOf(authStore.user!.id);
      if (dislikedIndex !== undefined && dislikedIndex !== -1) {
        post.downVotes?.splice(dislikedIndex, 1);
      } else {
        post.downVotes = post.downVotes? post.downVotes : []
        post.downVotes?.push(authStore.user!.id);
        if (likedIndex !== undefined && likedIndex !== -1) {
          post.upVotes?.splice(likedIndex, 1);
        }
      }
    } catch (error) {
      showNotification(
        "error",
        "Beim disliken des Beitrags ist ein Fehler aufgetreten!"
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
  };
});
