<template>
  <v-dialog v-model="dialog" max-width="500" persistent>
    <Form
      ref="form"
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      :initial-values="initialValues"
      @submit="submit"
    >
      <v-card width="500" :title="cardTitle">
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
                <v-btn>Profilbild ändern</v-btn>
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
          <v-btn
            :disabled="!meta.valid || !meta.dirty"
            type="submit"
            variant="tonal"
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
import { onMounted } from "vue";

setLocale(yupLocaleDe);

const store = useUsersStore();
const dialog = ref(true);
const discardDialog = ref(false);

const initialValues = ref({
  role: null,
  username: "",
  firstName: "",
  lastName: "",
  email: "",
  gender: null,
  password: "",
  passwordConfirm: "",
});

const form = ref<InstanceType<typeof Form> | null>(null);

const roles = ref(["Admin", "Moderator", "Nutzer"]);
const gender = ref(["männlich", "weiblich", "divers"]);

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
    .oneOf([yupRef("password")], "Passwörter stimmen nicht überein")
    .label("Passwort bestätigen"),
});

function cancel(dirty?: boolean) {
  if (dirty) {
    discardDialog.value = true;
  } else {
    close();
  }
}

onMounted(() => {
  if (store.user && router.currentRoute.value.name == "edit-user") {
    let initialValues = JSON.parse(JSON.stringify(store.user));
    initialValues.passwordConfirm = initialValues.password;
    form.value?.resetForm({
      values: initialValues,
    });
  }
});

function close() {
  router.push({ name: "users" });
}

function submit(values: any) {
  delete values.passwordConfirm;

  if (router.currentRoute.value.name == "create-user") {
    store.createUser(values);
  } else {
    store.updateUser(values);
  }
  close();
}

const cardTitle = computed(() => {
  return router.currentRoute.value.name == "create-user"
    ? "Nutzer erstellen"
    : "Nutzer bearbeiten";
});
</script>
