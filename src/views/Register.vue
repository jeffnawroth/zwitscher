<template>
  <v-dialog v-model="dialog" width="500">
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      @submit="submit"
    >
      <v-card title="Registrieren">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="username"
                label="Username"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
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
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="email"
                label="E-Mail"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BasePasswordInput
                name="password"
                label="Passwort"
              ></BasePasswordInput>
            </v-col>
            <v-col>
              <BasePasswordInput
                name="passwordConfirm"
                label="Passwort bestätigen"
              ></BasePasswordInput>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <span> Du hast bereits ein Konto? </span>
              <router-link :to="{ name: 'login' }"> Anmelden </router-link>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            type="submit"
            variant="tonal"
            @click="router.push({ name: 'home' })"
            >Schließen</v-btn
          >
          <v-btn :disabled="!meta.valid" type="submit" variant="tonal"
            >Registrieren</v-btn
          >
        </v-card-actions>
      </v-card>
    </Form>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import BaseInputWithValidation from "@/components/BaseInputWithValidation.vue";
import BasePasswordInput from "@/components/BasePasswordInput.vue";
import router from "@/router";
import { Form } from "vee-validate";
import { object, string, ref as yupRef, setLocale } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";

setLocale(yupLocaleDe);

const store = useAuthenticationStore();
const dialog = ref(true);

const validationSchema = object({
  username: string().required().label("Username"),
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
</script>
