import { defineStore } from "pinia";
import { ref } from "vue";
import { showNotification } from "./helpers";
import {
  User,
  UserAdd,
  UserApi,
  UserEdit,
  UserLight,
} from "@/typescript-axios-generated";
import { toBase64 } from "@/helpers";
import { computed } from "vue";

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>([]);
  const user = ref<User>();
  const followedUsers = ref<UserLight[]>([]);
  const loading = ref(false);
  const crudCardLoading = ref(false);
  const loadingFollowedUsers = ref(false);

  /**
   * Creates a new user
   * @param user
   */
  async function createUser(user: UserAdd) {
    try {
      crudCardLoading.value = true;
      if (user.avatar) user.avatar = await toBase64(user.avatar);
      const data = await UserApi.prototype.apiUserPost(user);
      users.value.push(data.data as User);
      showNotification("success", "Der Nutzer wurde erfolgreich erstellt!");
    } catch {
      showNotification(
        "error",
        "Beim Erstellen des Nutzers ist ein Fehler aufgetreten",
      );
    } finally {
      crudCardLoading.value = false;
    }
  }

  /**
   * Get a list of all users
   */
  async function getUsers() {
    try {
      if (users.value.length === 0) loading.value = true;
      // users.value = [];
      // loading.value = true;
      const data = await UserApi.prototype.apiUserGet();
      users.value = data.data as User[];
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Nutzer ist ein Fehler aufgetreten",
      );
    } finally {
      loading.value = false;
    }
  }

  /**
   * Gets a list of all followed users with specified attributes
   */
  async function fetchFollowedUsers() {
    try {
      loadingFollowedUsers.value = true;
      const data = await UserApi.prototype.apiUserFollowedUsersLightGet();
      followedUsers.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der gefolgten Nutzer ist ein Fehler aufgetreten",
      );
    } finally {
      loadingFollowedUsers.value = false;
    }
  }

  // async function getUser(id: string) {
  //   try {
  //     const data = await UserApi.prototype.apiUserIdGet(id);

  //     user.value = data.data as User;
  //   } catch (error) {
  //     showNotification(
  //       "error",
  //       "Beim Laden des Nutzers ist ein Fehler aufgetreten"
  //     );
  //   }
  // }

  /**
   * Gets a user with the username
   * @param username
   */
  async function getUserByUsername(username: string) {
    try {
      const data = await UserApi.prototype.apiUserGetByUsernameUsernameGet(
        username,
      );
      user.value = data.data as User;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden des Nutzers ist ein Fehler aufgetreten",
      );
    }
  }

  /**
   * Deletes a user with the specified ID
   * @param id
   */
  async function deleteUser(id: string) {
    try {
      crudCardLoading.value = true;
      await UserApi.prototype.apiUserIdDelete(id);
      users.value = users.value.filter((user) => user.id !== id);
      showNotification("success", "Der Nutzer wurde erfolgreich gelöscht!");
    } catch {
      showNotification(
        "error",
        "Beim Löschen des Nutzers ist ein Fehler aufgetreten",
      );
    } finally {
      crudCardLoading.value = false;
    }
  }

  /**
   * Updates an existing user
   * @param userEdit
   * @param notification
   */
  async function updateUser(userEdit: UserEdit, notification = true) {
    try {
      crudCardLoading.value = true;
      if (userEdit.avatar) userEdit.avatar = await toBase64(userEdit.avatar);
      await UserApi.prototype.apiUserIdPut(userEdit.id!, userEdit);
      user.value = userEdit;
      const index = users.value.findIndex((user) => user.id === userEdit.id);
      if (index > -1) users.value.splice(index, 1, userEdit);

      if (notification)
        showNotification("success", "Die Änderungen wurden gespeichert!");
    } catch {
      showNotification(
        "error",
        "Beim Bearbeiten des Nutzers ist ein Fehler aufgetreten",
      );
    } finally {
      crudCardLoading.value = false;
    }
  }

  /**
   * Allows a user to follow another user
   * @param id
   */
  async function followUser(id: string) {
    try {
      await UserApi.prototype.apiUserIdFollowPost(id);
      if (user.value) {
        const { id, name, username, avatar } = user.value;
        followedUsers.value.push({ id, avatar, username, name });
      }
    } catch (error) {
      showNotification(
        "error",
        "Beim Folgen des Nutzers ist ein Fehler aufgetreten",
      );
    }
  }

  /**
   * Allows a user to unfollow another user
   * @param id
   */
  async function unfollowUser(id: string) {
    try {
      await UserApi.prototype.apiUserIdUnfollowPost(id);
      const index = followedUsers.value.findIndex((user) => user.id == id);
      if (index > -1) followedUsers.value?.splice(index, 1);
    } catch (error) {
      showNotification(
        "error",
        "Beim Entfolgen des Nutzers ist ein Fehler aufgetreten",
      );
    }
  }

  /**
   * Updates a users password
   * @param password
   */
  async function changePassword(password: string) {
    try {
      await UserApi.prototype.apiUserPasswordChangePut(password);
      showNotification("success", "Das Passwort wurde erfolgreich geändert!");
    } catch (error) {
      showNotification(
        "error",
        "Beim ändern des Passworts ist ein Fehler aufgetreten!",
      );
      return Promise.reject(error);
    }
  }

  /**
   * Updates a users email
   * @param email
   */
  async function changeEmail(email: string) {
    try {
      await UserApi.prototype.apiUserEmailChangePut(email);
      showNotification("success", "Die E-Mail wurde erfolgreich geändert!");
    } catch (error) {
      showNotification(
        "error",
        "Beim Ändern der E-Mail ist ein Fehler aufgetreten!",
      );
      return Promise.reject(error);
    }
  }

  const sortedFollowedUsers = computed(() => {
    return followedUsers.value.sort((a, b) => {
      const usernameA = a.username!.toLocaleUpperCase();
      const usernameB = b.username!.toLocaleUpperCase();

      return usernameA > usernameB;
    });
  });

  return {
    user,
    users,
    createUser,
    getUsers,
    deleteUser,
    // getUser,
    updateUser,
    getUserByUsername,
    fetchFollowedUsers,
    followedUsers,
    followUser,
    unfollowUser,
    changePassword,
    changeEmail,
    loading,
    crudCardLoading,
    sortedFollowedUsers,
    loadingFollowedUsers,
  };
});
