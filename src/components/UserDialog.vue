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
              <v-btn :disabled="userLocked" v-bind="props" icon size="100">
                <Field v-slot="{ field }" name="avatar">
                  <v-img>
                    <v-avatar
                      :size="100"
                      class="avatar"
                      :image="field.value"
                    ></v-avatar>
                  </v-img>
                </Field>
              </v-btn>
            </template>
            <v-list>
              <v-list-item>
                <v-btn @click="fileInput?.click()">Profilbild ändern</v-btn>
                <input ref="fileInput" hidden type="file" />
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
                :disabled="userLocked"
              ></BaseSelectWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="username"
                label="Username"
                type="text"
                :disabled="userLocked"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="firstName"
                label="Vorname"
                type="text"
                :disabled="userLocked"
              ></BaseInputWithValidation>
            </v-col>
            <v-col>
              <BaseInputWithValidation
                name="lastName"
                label="Nachname"
                type="text"
                :disabled="userLocked"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="email"
                label="E-Mail"
                type="text"
                :disabled="userLocked"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BasePasswordInput
                name="password"
                label="Passwort"
                :disabled="userLocked"
              ></BasePasswordInput>
            </v-col>
            <v-col>
              <BasePasswordInput
                name="passwordConfirm"
                label="Passwort bestätigen"
                :disabled="userLocked"
              ></BasePasswordInput>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="birthdate"
                label="Geburtsdatum"
                type="date"
                :clearable="false"
                :max="dateToday"
                :disabled="userLocked"
              ></BaseInputWithValidation>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseSelectWithValidation
                name="gender"
                label="Geschlecht"
                :items="gender"
                :disabled="userLocked"
              ></BaseSelectWithValidation>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <BaseTextarea
                label="Bio"
                type="text"
                name="bio"
                auto-grow
                :disabled="userLocked"
              ></BaseTextarea>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseCombobox
                name="interests"
                label="Interessen"
                :items="interests"
                :disabled="userLocked"
              ></BaseCombobox>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn variant="tonal" @click="cancel(meta.dirty)">Abbrechen</v-btn>

          <LockButton
            v-if="!createUserRoute"
            @click="lockDialog = true"
          ></LockButton>

          <v-btn
            v-if="!createUserRoute"
            prepend-icon="mdi-delete-outline"
            variant="tonal"
            color="red"
            @click="deleteDialog = true"
            >Löschen</v-btn
          >
          <v-btn
            type="submit"
            :disabled="!meta.valid || !meta.dirty"
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

  <DeleteUserDialog v-model="deleteDialog"></DeleteUserDialog>
  <LockUserDialog v-model="lockDialog"></LockUserDialog>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import BaseInputWithValidation from "./BaseComponents/BaseInputWithValidation.vue";
import BaseSelectWithValidation from "./BaseComponents/BaseSelectWithValidation.vue";
import BaseDiscardDialog from "./BaseComponents/BaseDiscardDialog.vue";
import BasePasswordInput from "./BaseComponents/BasePasswordInput.vue";
import BaseCombobox from "./BaseComponents/BaseCombobox.vue";
import BaseTextarea from "./BaseComponents/BaseTextarea.vue";
import router from "@/router";
import { Form, Field } from "vee-validate";
import {
  object,
  string,
  number,
  ref as yupRef,
  setLocale,
  array,
  date,
} from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import { UserEdit } from "@/interfaces";
import DeleteUserDialog from "./DeleteUserDialog.vue";
import LockButton from "./LockButton.vue";
import LockUserDialog from "./LockUserDialog.vue";

setLocale(yupLocaleDe);

const store = useUsersStore();
const dialog = ref(true);
const lockDialog = ref(false);
const discardDialog = ref(false);
const deleteDialog = ref(false);

const fileInput = ref<HTMLInputElement | null>(null);

const initialValues = ref({
  avatar: null,
  role: null,
  username: "",
  firstName: "",
  lastName: "",
  email: "",
  gender: null,
  password: null,
  passwordConfirm: null,
  birthdate: null,
  interests: null,
  bio: null,
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
  interests: array().label("Interessen").nullable(),
  email: string().required().email().label("E-Mail"),
  birthdate: string().nullable(),
  bio: string().nullable(),
  password: string().nullable().label("Passwort"),
  passwordConfirm: string()
    .nullable()
    .oneOf([yupRef("password")], "Passwörter stimmen nicht überein")
    .label("Passwort bestätigen"),
  avatar: string().nullable(),
});

function cancel(dirty?: boolean) {
  if (dirty) {
    discardDialog.value = true;
  } else {
    close();
  }
}

const profileSettings = computed(() => {
  return router.currentRoute.value.name === "profile-settings";
});

const createUserRoute = computed(() => {
  return router.currentRoute.value.name === "create-user";
});

const dateToday = computed(() => {
  return new Date().toISOString().slice(0, 10);
});

const userLocked = computed(() => {
  return store.user?.locked;
});

onMounted(() => {
  if (
    store.user &&
    (router.currentRoute.value.name === "edit-user" ||
      router.currentRoute.value.name === "profile-settings")
  ) {
    const { gender, interests, birthdate, ...rest } = JSON.parse(
      JSON.stringify(store.user)
    );
    let initialValues = {
      ...rest,
      password: "",
      passwordConfirm: "",
      gender,
      interests,
      birthdate,
    };

    form.value?.resetForm({
      values: initialValues,
    });
  }
});

function close() {
  profileSettings.value
    ? router.push({ name: "profile" })
    : router.push({ name: "users" });
}

function submit(values: any) {
  const { passwordConfirm, ...rest } = values;
  let updatedValues: UserEdit = { ...rest };

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
