<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card title="Passwort ändern">
      <Form
        :validation-schema="validationSchema"
        :initial-values="initialValues"
        :on-submit="changePassword"
        autocomplete="off"
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
          <v-btn variant="tonal" type="submit">Speichern</v-btn>
        </v-card-actions>
      </Form>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import BasePasswordInput from "./BaseComponents/BasePasswordInput.vue";
import { object, ref, setLocale, string } from "yup";
import { Form } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
setLocale(yupLocaleDe);

defineEmits(["update:modelValue"]);

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
const validationSchema = object({
  currentPassword: string().required().label("Aktuelles Passwort"),
  newPassword: string()
    .required()
    .label("Neues Passwort")
    .notOneOf(
      [ref("currentPassword")],
      "Altes und neues Passwört dürfen nicht übereinstimmen"
    ),
  newPasswordConfirm: string()
    .required()
    .oneOf([ref("newPassword")], "Passwörter stimmen nicht überein"),
});

function changePassword(values: any) {
  console.log(values);
}
</script>
