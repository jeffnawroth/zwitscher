import { defineStore } from "pinia";
import { ref } from "vue";
import { users as dummyUsers } from "@/dummyData";
import { User } from "@/interfaces";

let id = 4;

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>(dummyUsers);
  const user = ref<User>();

  function createUser(user: User) {
    user.id = ++id;
    users.value.push(user);
  }

  function getUsers() {
    users.value = dummyUsers;
  }

  function getUser(id: number) {
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
    users.value.splice(userIndex, 1, userEdit);
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
