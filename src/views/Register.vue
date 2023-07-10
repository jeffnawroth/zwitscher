<template>
  <v-dialog v-model="dialog" width="500" persistent>
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      :initial-values="initialValues"
      @submit="submit"
    >
      <v-card title="Registrieren" :loading="store.loading">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="username"
                label="Benutzername"
                type="text"
                prefix="@"
                @keydown.space.prevent
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="name"
                label="Name"
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
import { ref } from "vue";
import { useAuthenticationStore } from "@/store/authentication";
import BaseInputWithValidation from "@/components/BaseComponents/BaseInputWithValidation.vue";
import BasePasswordInput from "@/components/BaseComponents/BasePasswordInput.vue";
import { Form } from "vee-validate";
import { object, string, ref as yupRef, setLocale } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useRouter } from "vue-router";

setLocale(yupLocaleDe);

const store = useAuthenticationStore();
const router = useRouter();
const dialog = ref(true);

const initialValues = {
  username: "",
  name: "",
  email: "",
  password: "",
  passwordConfirm: "",
};

const validationSchema = object({
  username: string().required().label("Benutzername"),
  name: string().required().label("Name"),
  email: string().required().email().label("E-Mail"),
  password: string().required().label("Passwort"),
  passwordConfirm: string()
    .required()
    .oneOf([yupRef("password")])
    .label("Passwörter"),
});

/**
 * Register user with passed values
 * @param values
 */
async function submit(values: any) {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const { passwordConfirm, ...credentials } = values;
  await store.register(credentials);
}
</script>
