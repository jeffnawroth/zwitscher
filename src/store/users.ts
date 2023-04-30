import { defineStore } from "pinia";
import { ref } from "vue";
import dummyUsers from "@/views/dummyUsers";
import { User } from "@/interfaces";

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>([]);
  const user = ref<User>();

  function createUser(user: User) {
    users.value.push(user);
  }

  function getUsers() {
    users.value = dummyUsers;
  }

  function deleteUser() {
    const index = users.value.findIndex(
      (userArray) => userArray.id === user.value?.id
    );
    users.value.splice(index, 1);
  }

  return { user, users, createUser, getUsers, deleteUser };
});
