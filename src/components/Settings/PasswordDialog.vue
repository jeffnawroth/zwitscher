<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card title="Passwort ändern" :loading="store.crudCardLoading">
      <Form
        v-slot="{ meta }"
        :validation-schema="validationSchema"
        :initial-values="initialValues"
        @submit="changePassword"
      >
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <BasePasswordInput
                name="currentPassword"
                label="Aktuelles Passwort"
              ></BasePasswordInput>
            </v-col>
            <v-col cols="12">
              <BasePasswordInput
                name="newPassword"
                label="Neues Passwort"
              ></BasePasswordInput>
            </v-col>
            <v-col cols="12">
              <BasePasswordInput
                name="newPasswordConfirm"
                label="Neues Passwort bestätigen"
              ></BasePasswordInput>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn variant="tonal" @click="$emit('update:modelValue', false)"
            >Abbrechen</v-btn
          >
          <v-btn
            :disabled="!meta.valid || !meta.dirty"
            variant="tonal"
            type="submit"
            >Speichern</v-btn
          >
        </v-card-actions>
      </Form>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import BasePasswordInput from "../BaseComponents/BasePasswordInput.vue";
import { object, ref, setLocale, string } from "yup";
import { Form } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useUsersStore } from "@/store/users";
import { useAuthenticationStore } from "@/store/authentication";
setLocale(yupLocaleDe);

const emit = defineEmits(["update:modelValue"]);
const store = useUsersStore();
const authStore = useAuthenticationStore();

defineProps({
  modelValue: {
    type: Boolean,
  },
});
const initialValues = {
  currentPassword: "",
  newPassword: "",
  newPasswordConfirm: "",
};

//Validationrules
const validationSchema = object({
  currentPassword: string()
    .required()
    .label("Aktuelles Passwort")
    .oneOf([authStore.user?.password!]),

  newPassword: string()
    .required()
    .label("Neues Passwort")
    .notOneOf(
      [ref("currentPassword")],
      "Aktuelles und neues Passwort dürfen nicht übereinstimmen",
    ),
  newPasswordConfirm: string()
    .required()
    .label("Neues Passwort bestätigen")
    .oneOf([ref("newPassword")], "Passwörter stimmen nicht überein"),
});

/**
 * Change password
 * @param values
 */
async function changePassword(values: any) {
  const { newPassword } = values;
  await store.changePassword(newPassword);
  emit("update:modelValue", false);
}
</script>
