import { AuthUser } from "@/interfaces";
import router from "@/router";
import axios from "axios";
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { userData } from "@/dummyData";
import {
  AuthenticationApi,
  UserLoginRequestDto,
  UserRegistrationRequestDto,
} from "@/typescript-axios-generated";

export const useAuthenticationStore = defineStore("authentication", () => {
  const user = ref<AuthUser | null>();

  async function register(credentials: UserRegistrationRequestDto) {
    const user =
      await AuthenticationApi.prototype.apiAuthenticationRegisterPost(
        credentials
      );
    setUserData(user.data);
    // setUserData(userData);
  }

  async function login(credentials: UserLoginRequestDto) {
    const user = await AuthenticationApi.prototype.apiAuthenticationLoginPost(
      credentials
    );
    setUserData(user.data);
    // setUserData(userData);
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
