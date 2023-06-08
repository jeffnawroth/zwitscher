import { defineStore } from "pinia";
import { useTheme } from "vuetify/lib/framework.mjs";

export const useSettingsStore = defineStore("settings", () => {
  const theme = useTheme();

  function toggleTheme() {
    theme.global.name.value = theme.global.current.value.dark
      ? "light"
      : "dark";
    localStorage.setItem("theme", theme.global.name.value);
  }

  function setTheme(newTheme: string) {
    theme.global.name.value = newTheme;
  }

  return { toggleTheme, theme, setTheme };
});
