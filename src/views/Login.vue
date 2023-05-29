<template>
  <v-dialog v-model="dialog" width="500">
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      :initial-values="initialValues"
      @submit="submit"
    >
      <v-card title="Anmelden">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="email"
                label="E-Mail"
                type="email"
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
          </v-row>
          <v-row>
            <v-col>
              <span> Du hast noch kein Konto? </span>
              <router-link :to="{ name: 'register' }">
                Registrieren
              </router-link>
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
            >Anmelden</v-btn
          >
        </v-card-actions>
      </v-card>
    </Form>
  </v-dialog>
</template>

<script setup lang="ts">
import { Form } from "vee-validate";
import { object, string, setLocale } from "yup";
import { useAuthenticationStore } from "@/store/authentication";
import BaseInputWithValidation from "@/components/BaseComponents/BaseInputWithValidation.vue";
import BasePasswordInput from "@/components/BaseComponents/BasePasswordInput.vue";
import { useRouter } from "vue-router";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { ref } from "vue";

setLocale(yupLocaleDe);

const initialValues = {
  email: "jeff@zwitscher.de",
  password: "Jeff123!",
};

const store = useAuthenticationStore();
const dialog = ref(true);
const router = useRouter();

const validationSchema = object({
  email: string().required().email().label("E-Mail"),
  password: string().required().label("Passwort"),
});

function submit(values: any) {
  store.login(values);
  router.push({ name: "home" });
}
</script>
