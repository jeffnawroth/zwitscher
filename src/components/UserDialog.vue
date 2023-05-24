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
          <BaseBirthdateInput
            name-day="day"
            name-month="month"
            name-year="year"
          ></BaseBirthdateInput>
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
          <v-row>
            <v-col>
              <BaseCombobox
                name="interests"
                label="Interessen"
                :items="interests"
              ></BaseCombobox>
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
import BaseInputWithValidation from "./BaseComponents/BaseInputWithValidation.vue";
import BaseSelectWithValidation from "./BaseComponents/BaseSelectWithValidation.vue";
import BaseDiscardDialog from "./BaseComponents/BaseDiscardDialog.vue";
import BasePasswordInput from "./BaseComponents/BasePasswordInput.vue";
import BaseBirthdateInput from "./BaseComponents/BaseBirthdateInput.vue";
import BaseCombobox from "./BaseComponents/BaseCombobox.vue";
import router from "@/router";
import { Form, Field } from "vee-validate";
import { object, string, number, ref as yupRef, setLocale, array } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import { UserEdit } from "@/interfaces";

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
  day: null,
  month: null,
  year: null,
  interests: null,
});

const form = ref<InstanceType<typeof Form> | null>(null);

const roles = ["Admin", "Moderator", "Nutzer"];
const gender = ["männlich", "weiblich", "divers"];
const interests = [
  "Sport",
  "Musik",
  "Lesen",
  "Kunst",
  "Reisen",
  "Kochen",
  "Filme",
  "Gaming",
  "Mode",
  "Tiere",
  "Natur",
  "Technologie",
  "Geschichte",
  "Politik",
  "Wissenschaft",
  "Fotografie",
  "Fitness",
  "Yoga",
  "Schreiben",
  "Tanzen",
];

const validationSchema = object({
  role: string().required().label("Rolle"),
  username: string().required().label("Username"),
  firstName: string().required().label("Vorname"),
  lastName: string().required().label("Nachname"),
  gender: string().label("Geschlecht").nullable(),
  interests: array().label("Interessen").required(),
  email: string().required().email().label("E-Mail"),
  day: number().nullable(),
  month: number().nullable(),
  year: number().nullable(),
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
    const { birthdate, password, ...rest } = JSON.parse(
      JSON.stringify(store.user)
    );
    let initialValues = { ...rest, password, passwordConfirm: password };
    if (birthdate) {
      let date = new Date(birthdate);
      initialValues = {
        ...initialValues,
        day: date.getDate(),
        month: date.getMonth() + 1,
        year: date.getFullYear(),
      };
    }
    form.value?.resetForm({
      values: initialValues,
    });
  }
});

function close() {
  router.push({ name: "users" });
}

function submit(values: any) {
  const { day, month, year, passwordConfirm, ...rest } = values;
  let updatedValues: UserEdit = { ...rest };

  if (day && month && year) {
    updatedValues = { ...rest, birthdate: [day, month, year] };
  }

  if (router.currentRoute.value.name == "create-user") {
    store.createUser(updatedValues);
  } else {
    store.updateUser(updatedValues);
  }
  close();
}

const cardTitle = computed(() => {
  return router.currentRoute.value.name == "create-user"
    ? "Nutzer erstellen"
    : "Nutzer bearbeiten";
});
</script>
