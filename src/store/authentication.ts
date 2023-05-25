import { AuthUser, LoginDto, RegisterDto } from "@/interfaces";
import router from "@/router";
import axios from "axios";
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { userData } from "@/dummyData";

export const useAuthenticationStore = defineStore("authentication", () => {
  const user = ref<AuthUser | null>(userData);

  async function register(credentials: RegisterDto) {
    //TO-DO: Register User
    //const userData = await ...

    setUserData(userData);
  }

  async function login(credentials: LoginDto) {
    //TO-DO: Log in user
    //const userData = await ...

    setUserData(userData);
  }

  function logout() {
    user.value = null;
    localStorage.removeItem("user");
    axios.defaults.headers.common["Authorization"] = null;
    router.push({ name: "login" });
  }

  function setUserData(data: any) {
    user.value = data;
    localStorage.setItem("user", JSON.stringify(data));
    axios.defaults.headers.common["Authorization"] = `Bearer ${data.token}`;
  }

  const loggedIn = computed(() => !!user.value);

  return { register, user, login, loggedIn, logout, setUserData };
});
