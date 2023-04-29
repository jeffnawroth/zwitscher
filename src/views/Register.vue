<template>
  <v-container class="fill-height justify-center">
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      @submit="submit"
    >
      <v-card width="500" title="Registrieren">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="firstName"
                label="Vorname"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
            <v-col>
              <BaseInputWithValidation
                name="lastName"
                label="Nachname"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <BaseInputWithValidation
            name="email"
            label="E-Mail"
            type="text"
          ></BaseInputWithValidation>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="password"
                label="Passwort"
                :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showPassword ? 'text' : 'password'"
                @click:append-inner="showPassword = !showPassword"
              ></BaseInputWithValidation>
            </v-col>
            <v-col>
              <BaseInputWithValidation
                name="passwordConfirm"
                label="Passwort bestätigen"
                :append-inner-icon="
                  showPasswordConfirm ? 'mdi-eye' : 'mdi-eye-off'
                "
                :type="showPasswordConfirm ? 'text' : 'password'"
                @click:append-inner="showPasswordConfirm = !showPasswordConfirm"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>

          <!--   <div class="ml-4 mb-6">
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
          </div> -->
          <span> Du hast bereits ein Konto? </span>
          <router-link :to="{ name: 'login' }"> Anmelden </router-link>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn :disabled="!meta.valid" type="submit" variant="tonal"
            >Registrieren</v-btn
          >
        </v-card-actions>
      </v-card>
    </Form>
  </v-container>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import BaseInputWithValidation from "@/components/BaseInputWithValidation.vue";
import router from "@/router";
import { Form } from "vee-validate";
import { object, string, ref as yupRef, setLocale } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";

setLocale(yupLocaleDe);

const store = useAuthenticationStore();
const showPassword = ref(false);
const showPasswordConfirm = ref(false);

const validationSchema = object({
  firstName: string().required().label("Vorname"),
  lastName: string().required().label("Nachname"),
  email: string().required().email().label("E-Mail"),
  password: string().required().label("Passwort"),
  passwordConfirm: string()
    .required()
    .oneOf([yupRef("password")])
    .label("Passwörter"),
});

function submit(values: Object) {
  store.register(values);
  router.push({ name: "home" });
}

/* const isMinLengthValid = computed(() => {
  return user.value.password.length >= 6;
});
const hasDigit = computed(() => {
  return /\d/.test(user.value.password);
});
const hasLowercase = computed(() => {
  return /[a-z]/.test(user.value.password);
});
const hasNonAlphanumeric = computed(() => {
  return /\W/.test(user.value.password);
});
const hasUppercase = computed(() => {
  return /[A-Z]/.test(user.value.password);
}); */
</script>
