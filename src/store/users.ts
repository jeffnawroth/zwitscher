import { defineStore } from "pinia";
import { ref } from "vue";
import { users as dummyUsers } from "@/dummyData";
import { User, UserAdd, UserEdit } from "@/interfaces";
import { v4 as uuidv4 } from "uuid";

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>(dummyUsers);
  const user = ref<User>();

  function createUser(user: UserAdd) {
    const addedUser: User = {
      ...user,
      id: uuidv4(),
      followers: [],
      following: [],
      createdAt: new Date().toUTCString(),
      locked: false,
    };

    users.value.push(addedUser);
  }

  function getUsers() {
    users.value = dummyUsers;
  }

  function getUser(id: string) {
    const userFound = users.value.find((user) => user.id == id);
    user.value = userFound;
  }

  function getUserByUsername(username: string) {
    const userFound = users.value.find((user) => user.username == username);
    user.value = userFound;
  }

  function deleteUser() {
    const userIndex = users.value.findIndex(
      (userArray) => userArray.id === user.value?.id
    );
    users.value.splice(userIndex, 1);
  }

  function updateUser(userEdit: User) {
    const userIndex = users.value.findIndex((user) => user.id === userEdit.id);

    const store = useUsersStore();
    users.value.splice(userIndex, 1, userEdit);
    store.user = userEdit;
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
