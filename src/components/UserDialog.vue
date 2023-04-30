<template>
  <v-dialog v-model="dialog" max-width="500" persistent>
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      :initial-values="initialValues"
      @submit="submit"
    >
      <v-card width="500" title="Nutzer erstellen">
        <v-row class="justify-center mb-2">
          <v-menu open-on-hover>
            <template #activator="{ props }">
              <v-btn v-bind="props" icon size="100">
                <v-avatar size="100">
                  <img
                    alt="user"
                    src="https://cdn.pixabay.com/photo/2020/06/24/19/12/cabbage-5337431_1280.jpg"
                  />
                </v-avatar>
              </v-btn>
            </template>
            <v-list>
              <v-list-item>
                <v-btn variant="text"> Profilbild ändern </v-btn>
              </v-list-item>
            </v-list>
          </v-menu>
        </v-row>
        <v-card-text>
          <v-row>
            <v-col>
              <BaseSelectWithValidation
                name="role"
                label="Rolle"
                :items="roles"
              ></BaseSelectWithValidation>
            </v-col>
          </v-row>
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
              <BaseSelectWithValidation
                name="gender"
                label="Geschlecht"
                :items="gender"
              ></BaseSelectWithValidation>
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
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn variant="tonal" @click="cancel(meta.dirty)">Abbrechen</v-btn>
          <v-btn :disabled="!meta.valid" type="submit" variant="tonal"
            >Speichern</v-btn
          >
        </v-card-actions>
      </v-card>
    </Form>
  </v-dialog>

  <BaseDiscardDialog
    v-model="discardDialog"
    @cancel="discardDialog = false"
    @discard="cancel"
  ></BaseDiscardDialog>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import BaseInputWithValidation from "@/components/BaseInputWithValidation.vue";
import BaseSelectWithValidation from "./BaseSelectWithValidation.vue";
import BaseDiscardDialog from "./BaseDiscardDialog.vue";
import BasePasswordInput from "./BasePasswordInput.vue";
import router from "@/router";
import { Form } from "vee-validate";
import { object, string, ref as yupRef, setLocale } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useUsersStore } from "@/store/users";

setLocale(yupLocaleDe);

const usersStore = useUsersStore();
const dialog = ref(true);
const discardDialog = ref(false);

const validationSchema = object({
  role: string().required().label("Rolle"),
  username: string().required().label("Username"),
  firstName: string().required().label("Vorname"),
  lastName: string().required().label("Nachname"),
  gender: string().label("Geschlecht").nullable(),
  email: string().required().email().label("E-Mail"),
  password: string().required().label("Passwort"),
  passwordConfirm: string()
    .required()
    .oneOf([yupRef("password")])
    .label("Passwörter"),
});

const initialValues = {
  role: null,
  username: "",
  firstName: "",
  lastName: "",
  gender: null,
  email: "",
  password: "",
  passwordConfirm: "",
};

const roles = ref(["Admin", "Moderator", "Nutzer"]);
const gender = ref(["männlich", "weiblich", "divers"]);

function cancel(dirty?: boolean) {
  if (dirty) {
    discardDialog.value = true;
  } else {
    close();
  }
}

function close() {
  router.push({ name: "users" });
}

function submit(values: Object) {
  usersStore.createUser(values);
  close();
}
</script>
