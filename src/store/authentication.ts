import { AuthUser } from "@/interfaces";
import router from "@/router";
import axios, { AxiosError } from "axios";
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import {
  AuthenticationApi,
  TokenRequest,
  UserLoginRequestDto,
  UserRegistrationRequestDto,
} from "@/typescript-axios-generated";
import { showNotification } from "./helpers";

export const useAuthenticationStore = defineStore("authentication", () => {
  const user = ref<AuthUser | null>();
  const loading = ref(false);

  /**
   * Register a new user
   * @param credentials
   */
  async function register(credentials: UserRegistrationRequestDto) {
    try {
      loading.value = true;
      const user =
        await AuthenticationApi.prototype.apiAuthenticationRegisterPost(
          credentials,
        );
      setUserData(user.data);
      showNotification("success", "Registrierung erfolgreich!");
      router.push({ name: "home" });
    } catch {
      showNotification(
        "error",
        "Beim Registrieren ist ein Fehler aufgetreten!",
      );
    } finally {
      loading.value = false;
    }
  }

  /**
   * Log in a user
   * @param credentials
   */
  async function login(credentials: UserLoginRequestDto) {
    try {
      loading.value = true;
      const user = await AuthenticationApi.prototype.apiAuthenticationLoginPost(
        credentials,
      );
      setUserData(user.data);
      router.push({ name: "home" });
    } catch (error: unknown) {
      if ((error as AxiosError).response?.status === 403)
        return Promise.reject(error);
      showNotification("error", "Beim Einloggen ist ein Fehler aufgetreten!");
    } finally {
      loading.value = false;
    }
  }

  /**
   * Log out a user
   */
  function logout() {
    if (user.value != null) {
      user.value = null;
      localStorage.removeItem("user");
      axios.defaults.headers.common["Authorization"] = null;
      // showNotification("success", "Du wurdest erfolgreich ausgeloggt!");
    }
    router.push({ name: "login" });
  }

  /**
   * Set logged in user data
   * @param data
   */
  function setUserData(data: any) {
    user.value = data;
    localStorage.setItem("user", JSON.stringify(data));
    axios.defaults.headers.common["Authorization"] = `Bearer ${data.token}`;
  }

  /**
   * Refresh the access token
   * @param param0
   */
  async function refreshUserToken({ token, refreshToken }: TokenRequest) {
    try {
      const data =
        await AuthenticationApi.prototype.apiAuthenticationRefreshTokenPost({
          token,
          refreshToken,
        });
      setUserData(data.data);
    } catch (error) {
      showNotification(
        "error",
        "Ihre Sitzung wurde beendet. Bitte loggen Sie sich neu ein.",
      );
      logout();
    }
  }

  const loggedIn = computed(() => !!user.value);

  return {
    register,
    user,
    login,
    loggedIn,
    logout,
    setUserData,
    refreshUserToken,
    loading,
  };
});
