import { defineStore } from "pinia";
import { ref } from "vue";
import { users as dummyUsers } from "@/dummyData";
import { User, UserAdd, UserEdit } from "@/interfaces";

let id = 4;

export const useUsersStore = defineStore("users", () => {
  const users = ref<User[]>(dummyUsers);
  const user = ref<User>();

  function createUser(user: UserAdd) {
    const { birthdate, ...rest } = user;
    let addedUser: User = {
      ...rest,
      id: ++id,
      follower: [],
      following: [],
      liked: [],
      disliked: [],
      createdAt: new Date(),
    };

    if (user.birthdate) {
      const newBirthdate = new Date(
        user.birthdate[2],
        user.birthdate[1] - 1,
        user.birthdate[0]
      );
      addedUser = { ...addedUser, birthdate: newBirthdate };
    }

    users.value.push(addedUser);
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

  function updateUser(userEdit: UserEdit) {
    const userIndex = users.value.findIndex((user) => user.id === userEdit.id);
    const { birthdate, ...rest } = userEdit;
    let user: User = { ...rest };

    if (birthdate) {
      const newBirthdate = new Date(
        birthdate[2],
        birthdate[1] - 1,
        birthdate[0]
      );
      user = { ...user, birthdate: newBirthdate };
    }

    users.value.splice(userIndex, 1, user);
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
