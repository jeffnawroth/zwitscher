import { defineStore } from "pinia";
import { ref } from "vue";
import { User, UserAdd } from "@/interfaces";
import { showNotification } from "./helpers";
import {
  createNewUser,
  fetchUserByUsername,
  getAllUsers,
  getUserById,
  modifyUser,
  removeUser,
} from "@/dummyApi";

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>([]);
  const user = ref<User>();

  async function createUser(user: UserAdd) {
    try {
      await createNewUser(user);
      showNotification("success", "Der Nutzer wurde erfolgreich erstellt!");
    } catch {
      showNotification(
        "error",
        "Beim erstellen des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function getUsers() {
    try {
      const data = await getAllUsers();
      users.value = data as User[];
    } catch (error) {
      showNotification(
        "error",
        "Beim laden der Nutzer ist ein Fehler aufgetreten"
      );
    }
  }

  async function getUser(id: string) {
    try {
      const data = await getUserById(id);
      user.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim laden des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function getUserByUsername(username: string) {
    try {
      const data = await fetchUserByUsername(username);
      user.value = data;
    } catch (error) {
      showNotification(
        "error",
        "Beim laden des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function deleteUser() {
    try {
      await removeUser(user.value!.id);
      showNotification("success", "Der Nutzer wurde erfolgreich gelöscht!");
    } catch {
      showNotification(
        "error",
        "Beim löschen des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  async function updateUser(userEdit: User) {
    try {
      await modifyUser(userEdit);
      const store = useUsersStore();
      store.user = userEdit;
      showNotification("success", "Die Änderungen wurden gespeichert!");
    } catch {
      showNotification(
        "error",
        "Beim Bearbeiten des Nutzers ist ein Fehler aufgetreten"
      );
    }
  }

  return {
    user,
    users,
    createUser,
    getUsers,
    deleteUser,
    getUser,
    updateUser,
    getUserByUsername,
  };
});
