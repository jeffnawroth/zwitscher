import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import dummyUsers from "@/views/dummyUsers";

export const useUsersStore = defineStore("users", () => {
  const users = ref(dummyUsers);

  function createUser(user: Object) {
    //@ts-expect-error
    users.value.push(user);
  }

  return { users, createUser };
});
