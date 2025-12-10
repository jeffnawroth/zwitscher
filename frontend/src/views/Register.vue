<script setup lang="ts">
import { Form } from 'vee-validate'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { object, setLocale, string, ref as yupRef } from 'yup'
import BaseInputWithValidation from '@/components/BaseComponents/BaseInputWithValidation.vue'
import BasePasswordInput from '@/components/BaseComponents/BasePasswordInput.vue'
import yupLocaleDe from '@/plugins/yupLocaleDe'
import { useAuthenticationStore } from '@/store/authentication'

setLocale(yupLocaleDe)

const store = useAuthenticationStore()
const router = useRouter()
const dialog = ref(true)

const initialValues = {
  username: '',
  name: '',
  email: '',
  password: '',
  passwordConfirm: '',
}

const validationSchema = object({
  username: string().required().label('Benutzername'),
  name: string().required().label('Name'),
  email: string().required().email().label('E-Mail'),
  password: string().required().label('Passwort'),
  passwordConfirm: string()
    .required()
    .oneOf([yupRef('password')])
    .label('Passwörter'),
})

/**
 * Register user with passed values
 * @param values
 */
async function submit(values: any) {
  const { passwordConfirm, ...credentials } = values
  await store.register(credentials)
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
      <v-card title="Registrieren" :loading="store.loading">
        <v-card-text>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="username"
                label="Benutzername"
                type="text"
                prefix="@"
                @keydown.space.prevent
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="name"
                label="Name"
                type="text"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <BaseInputWithValidation
                name="email"
                label="E-Mail"
                type="text"
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
            <v-col>
              <BasePasswordInput
                name="passwordConfirm"
                label="Passwort bestätigen"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col />
            <v-col cols="12">
              <span> Du hast bereits ein Konto? </span>
              <v-hover>
                <template #default="{ isHovering, props }">
                  <span
                    v-bind="props"
                    class="text-indigo-darken-2"
                    :class="isHovering ? 'hover' : undefined"
                    @click="router.push({ name: 'login' })"
                  >
                    Anmelden
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
            Registrieren
          </v-btn>
        </v-card-actions>
      </v-card>
    </Form>
  </v-dialog>
</template>
