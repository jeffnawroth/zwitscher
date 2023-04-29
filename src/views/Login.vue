<template>
  <v-container class="fill-height justify-center">
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      @submit="submit"
    >
      <v-card width="500" title="Anmelden">
        <v-card-text>
          <BaseInputWithValidation
            name="email"
            label="E-Mail"
            type="email"
          ></BaseInputWithValidation>
          <BaseInputWithValidation
            name="password"
            label="Passwort"
            :type="showPassword ? 'text' : 'password'"
            :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
            @click:append-inner="showPassword = !showPassword"
          ></BaseInputWithValidation>
          <span> Du hast noch kein Konto? </span>
          <router-link :to="{ name: 'register' }"> Registrieren </router-link>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn :disabled="!meta.valid" type="submit" variant="tonal"
            >Anmelden</v-btn
          >
        </v-card-actions>
      </v-card>
    </Form>
  </v-container>
</template>

<script setup lang="ts">
import { Form } from "vee-validate";
import { object, string, setLocale } from "yup";
import { useAuthenticationStore } from "@/store/authentication";
import BaseInputWithValidation from "@/components/BaseInputWithValidation.vue";
import router from "@/router";
import { ref } from "vue";
import yupLocaleDe from "@/plugins/yupLocaleDe";

setLocale(yupLocaleDe);

const store = useAuthenticationStore();

const validationSchema = object({
  email: string().required().email().label("E-Mail"),
  password: string().required().label("Passwort"),
});

const showPassword = ref(false);

function submit(values: Object) {
  store.login(values);
  router.push({ name: "home" });
}
</script>
