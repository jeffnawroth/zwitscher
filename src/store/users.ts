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

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>([]);
  const user = ref<User>();
  const followedUsers = ref<UserLight[]>([]);
  const loading = ref(false);

  async function createUser(user: UserAdd) {
    try {
      if (user.avatar) user.avatar = await toBase64(user.avatar);
      const data = await UserApi.prototype.apiUserPost(user);
      users.value.push(data.data as User);
      showNotification("success", "Der Nutzer wurde erfolgreich erstellt!");
    } catch {
      showNotification(
        "error",
        "Beim Erstellen des Nutzers ist ein Fehler aufgetreten",
      );
    }
  }

  async function getUsers() {
    try {
      users.value = [];
      loading.value = true;
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

  async function fetchFollowedUsers() {
    try {
      const data = await UserApi.prototype.apiUserFollowedUsersLightGet();
      followedUsers.value = data.data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der gefolgten Nutzer ist ein Fehler aufgetreten",
      );
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

  async function deleteUser(id: string) {
    try {
      await UserApi.prototype.apiUserIdDelete(id);
      users.value = users.value.filter((user) => user.id !== id);
      showNotification("success", "Der Nutzer wurde erfolgreich gelöscht!");
    } catch {
      showNotification(
        "error",
        "Beim Löschen des Nutzers ist ein Fehler aufgetreten",
      );
    }
  }

  async function updateUser(userEdit: UserEdit, notification = true) {
    try {
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
    }
  }

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
  };
});
