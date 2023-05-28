<template>
  <v-dialog v-model="dialog" persistent width="500">
    <v-card :title="cardTitle">
      <Form
        ref="form"
        v-slot="{ meta }"
        :validation-schema="validationSchema"
        :initial-values="initialValues"
        @submit="submit"
      >
        <v-row class="justify-center mb-2">
          <Field
            v-slot="{ handleChange, handleBlur }"
            v-model="file"
            name="avatar"
          >
            <input
              ref="fileInput"
              hidden
              type="file"
              accept="image/*, video/*"
              @change="handleChange($event), onFileChange($event)"
              @blur="handleBlur"
            />
          </Field>
          <Avatar :file="file" @click="fileInput?.click()"></Avatar>
        </v-row>
        <v-tabs v-model="tab" fixed-tabs>
          <v-tab :value="1">Profil</v-tab>
          <v-tab :value="2">Konto</v-tab>
        </v-tabs>
        <v-window v-model="tab">
          <v-window-item :value="1">
            <v-card-text>
              <v-row>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="name"
                    label="Name"
                    type="text"
                    :disabled="userLocked"
                  ></BaseInputWithValidation>
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="birthdate"
                    label="Geburtsdatum"
                    type="date"
                    :clearable="false"
                    :max="dateToday"
                    :disabled="userLocked"
                  ></BaseInputWithValidation>
                </v-col>
                <v-col cols="12">
                  <BaseSelectWithValidation
                    name="gender"
                    label="Geschlecht"
                    :items="gender"
                    :disabled="userLocked"
                  ></BaseSelectWithValidation>
                </v-col>
                <v-col cols="12">
                  <BaseCombobox
                    name="interests"
                    label="Interessen"
                    :items="interests"
                    :disabled="userLocked"
                  ></BaseCombobox>
                </v-col>
                <v-col cols="12">
                  <BaseTextarea
                    label="Bio"
                    type="text"
                    name="bio"
                    auto-grow
                    :disabled="userLocked"
                  ></BaseTextarea>
                </v-col>
              </v-row>
            </v-card-text>
          </v-window-item>
          <v-window-item :value="2">
            <v-card-text>
              <v-row>
                <v-col cols="12">
                  <BaseSelectWithValidation
                    name="role"
                    label="Rolle"
                    :items="roles"
                    :disabled="userLocked"
                  ></BaseSelectWithValidation>
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="username"
                    label="Benutzername"
                    type="text"
                    :disabled="userLocked"
                    prefix="@"
                    @keydown.space.prevent
                  ></BaseInputWithValidation>
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="email"
                    label="E-Mail"
                    type="text"
                    :disabled="userLocked"
                  ></BaseInputWithValidation>
                </v-col>
                <v-col cols="12" sm="6">
                  <BasePasswordInput
                    name="password"
                    label="Passwort"
                    :disabled="userLocked"
                  ></BasePasswordInput>
                </v-col>
                <v-col cols="12" sm="6">
                  <BasePasswordInput
                    name="passwordConfirm"
                    label="Passwort bestätigen"
                    :disabled="userLocked"
                  ></BasePasswordInput>
                </v-col>
              </v-row>
            </v-card-text>
          </v-window-item>
        </v-window>

        <v-card-actions>
          <v-btn
            v-if="tab > 1"
            variant="plain"
            icon="mdi-chevron-left"
            @click="tab--"
          ></v-btn>
          <v-spacer></v-spacer>
          <v-btn variant="tonal" @click="cancel(meta.dirty)">Abbrechen</v-btn>

          <v-btn
            type="submit"
            :disabled="!meta.valid || !meta.dirty"
            variant="tonal"
            >Speichern</v-btn
          >
          <v-btn
            v-if="tab < 2"
            variant="plain"
            icon="mdi-chevron-right"
            @click="tab++"
          ></v-btn>
        </v-card-actions>
      </Form>
    </v-card>
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
import BaseCombobox from "./BaseComponents/BaseCombobox.vue";
import BaseTextarea from "./BaseComponents/BaseTextarea.vue";
import { useRoute, useRouter } from "vue-router";
import { Form, Field } from "vee-validate";
import { object, string, ref as yupRef, setLocale, array, mixed } from "yup";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useUsersStore } from "@/store/users";
import { onMounted } from "vue";
import { UserEdit } from "@/interfaces";
import Avatar from "./Avatar.vue";

setLocale(yupLocaleDe);

const store = useUsersStore();
const dialog = ref(true);
const discardDialog = ref(false);
const route = useRoute();
const router = useRouter();

const fileInput = ref<HTMLInputElement | null>(null);
const file = ref<File>();

const tab = ref(1);

const initialValues = ref({
  avatar: null,
  role: null,
  username: "",
  name: "",
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
  username: string()
    .required()
    .label("Benutzername")
    .matches(
      /^[a-zA-Z0-9_-]+$/,
      "Der Benutzername darf nur Buchstaben, Zahlen, Bindestriche und Unterstriche enthalten"
    ),
  name: string().required().label("Name"),
  gender: string().label("Geschlecht").nullable(),
  interests: array().label("Interessen").nullable(),
  email: string().required().email().label("E-Mail"),
  birthdate: string().nullable(),
  bio: string().nullable(),
  password:
    route.name === "create-user"
      ? string().required().label("Passwort")
      : string().nullable().label("Passwort"),
  passwordConfirm:
    route.name === "create-user"
      ? string()
          .required()
          .oneOf([yupRef("password")], "Passwörter stimmen nicht überein")
      : string().nullable().label("Passwort bestätigen"),
  avatar: mixed().nullable(),
});

const profileSettings = computed(() => {
  return route.name === "profile-settings";
});

const dateToday = computed(() => {
  return new Date().toISOString().slice(0, 10);
});

const userLocked = computed(() => {
  return store.user?.locked;
});

const cardTitle = computed(() => {
  return route.name == "create-user" ? "Nutzer erstellen" : "Nutzer bearbeiten";
});

onMounted(() => {
  if (
    store.user &&
    (route.name === "edit-user" || route.name === "profile-settings")
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
      avatar: store.user.avatar,
    };

    form.value?.resetForm({
      values: initialValues,
    });
  }
});

function cancel(dirty?: boolean) {
  if (dirty) {
    discardDialog.value = true;
  } else {
    close();
  }
}

function onFileChange(e: any) {
  var files = e.target.files || e.dataTransfer.files;
  if (!files.length) return;
  file.value = e.target.files[0];
}

function close() {
  profileSettings.value
    ? router.push({ name: "profile" })
    : router.push({ name: "users" });
}

function submit(values: any) {
  const { passwordConfirm, ...rest } = values;
  let updatedValues: UserEdit = { ...rest };

  if (route.name == "create-user") {
    store.createUser(updatedValues);
  } else {
    store.updateUser(updatedValues);
  }
  close();
}
</script>
