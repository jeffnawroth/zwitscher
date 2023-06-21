import { defineStore } from "pinia";
import { ref } from "vue";
import { User, UserAdd, UserLight } from "@/interfaces";
import { showNotification } from "./helpers";
import { UserApi } from "@/typescript-axios-generated";
import { getFollowedUsers } from "@/dummyApi";

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>([]);
  const user = ref<User>();
  const followedUsers = ref<UserLight[]>();

  async function createUser(user: UserAdd) {
    try {
      const data = await UserApi.prototype.apiUserPost(user);
      users.value.push(data.data as User);
      showNotification("success", "Der Nutzer wurde erfolgreich erstellt!");
    } catch {
      showNotification(
        "error",
        "Beim Erstellen des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function getUsers() {
    try {
      const data = await UserApi.prototype.apiUserGet();
      users.value = data.data as User[];
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der Nutzer ist ein Fehler aufgetreten"
      );
    }
  }

  async function fetchFollowedUsers(id: string) {
    try {
      const data = await getFollowedUsers(id);
      followedUsers.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden der gefolgten Nutzer ist ein Fehler aufgetreten"
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
        username
      );
      user.value = data.data as User;
    } catch (error) {
      showNotification(
        "error",
        "Beim Laden des Nutzers ist ein Fehler aufgetreten"
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
        "Beim Löschen des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function updateUser(userEdit: User, notification = true) {
    try {
      await UserApi.prototype.apiUserIdPut(userEdit.id, userEdit);
      user.value = userEdit;
      const index = users.value.findIndex((user) => user.id === userEdit.id);
      if (index > -1) users.value.splice(index, 1, userEdit);

      if (notification)
        showNotification("success", "Die Änderungen wurden gespeichert!");
    } catch {
      showNotification(
        "error",
        "Beim Bearbeiten des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function followUser(id: string) {
    try {
        await UserApi.prototype.apiUserIdFollowPost(id)
    } catch (error) {
      showNotification(
        "error",
        "Beim Folgen des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }
  async function unfollowUser(id: string) {
    try {
        await UserApi.prototype.apiUserIdUnfollowPost(id)
    } catch (error) {
      showNotification(
        "error",
        "Beim Entfolgen des Nutzers ist ein Fehler aufgetreten"
      );
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
    unfollowUser
  };
});
