<script setup lang="ts">
import type { AxiosError } from 'axios'
import { Form } from 'vee-validate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { object, setLocale, string } from 'yup'
import BaseInputWithValidation from '@/components/BaseComponents/BaseInputWithValidation.vue'
import BasePasswordInput from '@/components/BaseComponents/BasePasswordInput.vue'
import ResetPasswordDialog from '@/components/ResetPasswordDialog.vue'
import UserLockedDialog from '@/components/UserLockedDialog.vue'
import yupLocaleDe from '@/plugins/yupLocaleDe'
import { useAuthenticationStore } from '@/store/authentication'

setLocale(yupLocaleDe)

const initialValues = {
  email: '',
  password: '',
}

const store = useAuthenticationStore()
const dialog = ref(true)
const router = useRouter()

const showLockedDialog = ref(false)
const showResetPasswordDialog = ref(false)

const validationSchema = object({
  email: string().required().email().label('E-Mail'),
  password: string().required().label('Passwort'),
})

/**
 * Try to login a user with passed values if not logged. If user is locken then open locked-dialog
 * @param values
 */
async function submit(values: any) {
  try {
    await store.login(values)
  }
  catch (error: unknown) {
    if ((error as AxiosError).response?.status === 403) {
      showLockedDialog.value = true
      dialog.value = false
    }
  }
}
</script>

<template>
  <v-dialog v-model="dialog" width="500" persistent>
    <Form
      v-slot="{ meta }"
      :validation-schema="validationSchema"
      :initial-values="initialValues"
      @submit="submit"
    >
      <v-card title="Anmelden" :loading="store.loading">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="email"
                label="E-Mail"
                type="email"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BasePasswordInput
                name="password"
                label="Passwort"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12">
              <v-hover>
                <template #default="{ isHovering, props }">
                  <p
                    v-bind="props"
                    class="text-indigo-darken-2"
                    :class="isHovering ? 'hover' : undefined"
                    @click="
                      showResetPasswordDialog = true;
                      dialog = false;
                    "
                  >
                    Passwort vergessen?
                  </p>
                </template>
              </v-hover>
            </v-col>
            <v-col cols="12">
              <span>Du hast noch kein Konto?</span>
              <v-hover>
                <template #default="{ isHovering, props }">
                  <span
                    v-bind="props"
                    class="text-indigo-darken-2"
                    :class="isHovering ? 'hover' : undefined"
                    @click="router.push({ name: 'register' })"
                  >
                    Registrieren
                  </span>
                </template>
              </v-hover>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            type="submit"
            variant="tonal"
            @click="router.push({ name: 'home' })"
          >
            Schließen
          </v-btn>
          <v-btn :disabled="!meta.valid" type="submit" variant="tonal">
            Anmelden
          </v-btn>
        </v-card-actions>
      </v-card>
    </Form>
  </v-dialog>

  <ResetPasswordDialog
    v-model="showResetPasswordDialog"
    @close="
      dialog = true;
      showResetPasswordDialog = false;
    "
  />

  <UserLockedDialog
    v-model="showLockedDialog"
    @close="
      showLockedDialog = false;
      dialog = true;
    "
  />
</template>
