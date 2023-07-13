<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card title="E-Mail ändern" :loading="store.crudCardLoading">
      <Form
        v-slot="{ meta }"
        :validation-schema="validationSchema"
        :initial-values="initialValues"
        @submit="changeMail"
      >
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <BaseInputWithValidation
                name="currentMail"
                label="Aktuelle E-Mail"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
            <v-col cols="12">
              <BaseInputWithValidation
                name="newMail"
                label="Neue E-Mail"
                type="text"
              ></BaseInputWithValidation>
            </v-col>
            <v-col cols="12">
              <BaseInputWithValidation
                name="newMailConfirm"
                label="Neue E-Mail bestätigen"
                type="text"
              ></BaseInputWithValidation>
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
import { object, ref, setLocale, string } from "yup";
import { Form } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import BaseInputWithValidation from "../BaseComponents/BaseInputWithValidation.vue";
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
  currentMail: "",
  newMail: "",
  newMailConfirm: "",
};
const validationSchema = object({
  currentMail: string()
    .required()
    .label("Aktuelle E-Mail")
    .email()
    .oneOf([authStore.user?.email!]),
  newMail: string()
    .required()
    .label("Neue E-Mail")
    .notOneOf(
      [ref("currentMail")],
      "Aktuelle und neue E-mail dürfen nicht übereinstimmen",
    ),
  newMailConfirm: string()
    .required()
    .oneOf([ref("newMail")], "E-Mails stimmen nicht überein"),
});

async function changeMail(values: any) {
  const { newMail } = values;
  await store.changeEmail(newMail);
  emit("update:modelValue", false);
}
</script>
