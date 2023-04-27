<template>
  <v-container class="fill-height justify-center">
    <v-card width="500" title="Registrieren">
      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="user.firstName"
              label="Vorname"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="user.lastName"
              label="Nachname"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-text-field v-model="user.email" label="E-Mail"></v-text-field>
        <v-row>
          <v-col>
            <v-text-field
              v-model="user.password"
              label="Passwort"
              :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showPassword ? 'text' : 'password'"
              @click:append-inner="showPassword = !showPassword"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="passwordConfirm"
              label="Passwort bestätigen"
              :append-inner-icon="
                showPasswordConfirm ? 'mdi-eye' : 'mdi-eye-off'
              "
              :type="showPasswordConfirm ? 'text' : 'password'"
              @click:append-inner="showPasswordConfirm = !showPasswordConfirm"
            ></v-text-field
          ></v-col>
        </v-row>

        <div class="ml-4 mb-6">
          <ul>
            <li :class="{ 'text-success': isMinLengthValid }">
              mindestens 6 Zeichen
            </li>
            <li :class="{ 'text-success': hasDigit }">
              mindestens eine Ziffer ('0'-'9')
            </li>
            <li :class="{ 'text-success': hasLowercase }">
              mindestens einen Kleinbuchstaben ('a'-'z')
            </li>
            <li :class="{ 'text-success': hasNonAlphanumeric }">
              mindestens ein nicht alphanumerisches Zeichen
            </li>
            <li :class="{ 'text-success': hasUppercase }">
              mindestens einen Großbuchstaben ('A'-'Z')
            </li>
          </ul>
        </div>
        <span> Du hast bereits ein Konto? </span>
        <router-link :to="{ name: 'login' }"> Anmelden </router-link>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn variant="tonal">Registrieren</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from "vue";

const user = reactive({ firstName: "", lastName: "", email: "", password: "" });
const passwordConfirm = ref("");
const showPassword = ref(false);
const showPasswordConfirm = ref(false);

const isMinLengthValid = computed(() => {
  return user.password.length >= 6;
});
const hasDigit = computed(() => {
  return /\d/.test(user.password);
});
const hasLowercase = computed(() => {
  return /[a-z]/.test(user.password);
});
const hasNonAlphanumeric = computed(() => {
  return /\W/.test(user.password);
});
const hasUppercase = computed(() => {
  return /[A-Z]/.test(user.password);
});
</script>
