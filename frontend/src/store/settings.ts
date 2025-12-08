import { defineStore } from "pinia";
import { useTheme } from "vuetify";

export const useSettingsStore = defineStore("settings", () => {
  const theme = useTheme();

  /**
   * Toggle theme
   */
  function toggleTheme() {
    theme.global.name.value = theme.global.current.value.dark
      ? "light"
      : "dark";
    localStorage.setItem("theme", theme.global.name.value);
  }

  /**
   * Set selected theme
   * @param newTheme
   */
  function setTheme(newTheme: string) {
    theme.global.name.value = newTheme;
  }

  return { toggleTheme, theme, setTheme };
});
