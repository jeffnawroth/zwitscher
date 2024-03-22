<template>
  <v-dialog
    :model-value="modelValue"
    width="auto"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <v-card width="500">
      <v-card-title>Passwort zurücksetzen</v-card-title>
      <v-card-subtitle
        >Wenn ein Konto mit dieser E-Mail-Adresse existiert, dann erhalten Sie
        <br />
        in kürze eine E-Mail zum zurücksetzen Ihres Passworts.</v-card-subtitle
      >
      <Form
        v-slot="{ meta }"
        :validation-schema="validationSchema"
        :initial-values="initialValues"
        :on-submit="submit"
      >
        <v-card-text>
          <BaseInputWithValidation
            name="email"
            label="E-Mail"
            type="text"
          ></BaseInputWithValidation>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn variant="tonal" @click="$emit('close')">Schließen</v-btn>
          <v-btn variant="tonal" type="submit" :disabled="!meta.valid"
            >Senden</v-btn
          >
        </v-card-actions>
      </Form>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { object, string } from "yup";
import BaseInputWithValidation from "./BaseComponents/BaseInputWithValidation.vue";
import { Form } from "vee-validate";
import { ref } from "vue";
import { showNotification } from "@/store/helpers";

defineProps({
  modelValue: {
    type: Boolean,
  },
});

const emit = defineEmits(["close", "update:model-value"]);

const validationSchema = object({
  email: string().required().email().label("E-Mail"),
});

const initialValues = ref({
  email: "",
});

//Show success notifaction
function submit() {
  showNotification(
    "success",
    "Eine E-Mail zum Ändern des Passworts wurde versendet!",
  );
  emit("close");
}
</script>
