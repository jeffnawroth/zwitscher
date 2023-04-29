import router from "@/router";
import axios from "axios";
import { defineStore } from "pinia";
import { ref, computed } from "vue";

const userData = {
  firstName: "Admin",
  lastName: "Nimda",
  token:
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
};

export const useAuthenticationStore = defineStore("authentication", () => {
  const user = ref(null);

  async function register(credentials: Object) {
    //TO-DO: Register User
    //const userData = await ...

    setUserData(userData);
  }

  async function login(credentials: Object) {
    //TO-DO: Log in user
    //const userData = await ...

    setUserData(userData);
  }

  function logout() {
    user.value = null;
    localStorage.removeItem("user");
    axios.defaults.headers.common["Authorization"] = null;
    router.push({ name: "landing-page" });
  }

  function setUserData(data: any) {
    user.value = data;
    localStorage.setItem("user", JSON.stringify(data));
    axios.defaults.headers.common["Authorization"] = `Bearer ${data.token}`;
  }

  const loggedIn = computed(() => !!user.value);

  return { register, user, login, loggedIn, logout, setUserData };
});
