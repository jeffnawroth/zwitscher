<script setup lang="ts">
import { Field, Form } from 'vee-validate'
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import {
  array,
  mixed,
  number,
  object,
  setLocale,
  string,
  ref as yupRef,
} from 'yup'
import yupLocaleDe from '@/plugins/yupLocaleDe'
import { useAuthenticationStore } from '@/store/authentication'
import { useUsersStore } from '@/store/users'
import { Gender, Role } from '@/typescript-axios-generated'
import Avatar from './Avatar.vue'
import BaseCombobox from './BaseComponents/BaseCombobox.vue'
import BaseDiscardDialog from './BaseComponents/BaseDiscardDialog.vue'
import BaseInputWithValidation from './BaseComponents/BaseInputWithValidation.vue'
import BasePasswordInput from './BaseComponents/BasePasswordInput.vue'
import BaseSelectWithValidation from './BaseComponents/BaseSelectWithValidation.vue'
import BaseTextarea from './BaseComponents/BaseTextarea.vue'

setLocale(yupLocaleDe)

const store = useUsersStore()
const authStore = useAuthenticationStore()
const dialog = ref(true)
const discardDialog = ref(false)
const route = useRoute()
const router = useRouter()

const fileInput = ref<HTMLInputElement | null>(null)
const file = ref<File>()

const tab = ref(1)

const modCreatesUser = computed(() => {
  return route.name === 'create-user' && authStore.user?.role === Role.NUMBER_1
})

const initialValues = ref({
  avatar: null,
  role: modCreatesUser.value ? Role.NUMBER_2 : null,
  username: '',
  name: '',
  email: '',
  gender: null,
  password: null,
  passwordConfirm: null,
  birthDate: null,
  interests: null,
  bio: null,
})

const form = ref<InstanceType<typeof Form> | null>(null)

const roles = [
  { text: 'Admin', value: Role.NUMBER_0 },
  { text: 'Moderator', value: Role.NUMBER_1 },
  { text: 'Nutzer', value: Role.NUMBER_2 },
]
const gender = [
  { text: 'männlich', value: Gender.NUMBER_0 },
  { text: 'weiblich', value: Gender.NUMBER_1 },
  { text: 'divers', value: Gender.NUMBER_2 },
]
const interests = [
  'Sport',
  'Musik',
  'Lesen',
  'Kunst',
  'Reisen',
  'Kochen',
  'Filme',
  'Gaming',
  'Mode',
  'Tiere',
  'Natur',
  'Technologie',
  'Geschichte',
  'Politik',
  'Wissenschaft',
  'Fotografie',
  'Fitness',
  'Yoga',
  'Schreiben',
  'Tanzen',
]

const showAccountSettings = computed(() => {
  return (
    (authStore.user?.role === Role.NUMBER_0
      && authStore.user.id !== store.user?.id)
    || route.name === 'create-user'
  )
})

// Validationrules
const validationSchema = computed(() => object({
  role: showAccountSettings.value ? number().required().label('Rolle') : number().nullable(),
  username: showAccountSettings.value
    ? string()
        .required()
        .label('Benutzername')
        .matches(
          /^[\w-]+$/,
          'Der Benutzername darf nur Buchstaben, Zahlen, Bindestriche und Unterstriche enthalten',
        )
    : string().nullable(),
  name: string().required().label('Name'),
  gender: number().label('Geschlecht').nullable(),
  interests: array().label('Interessen').nullable(),
  email: showAccountSettings.value ? string().required().email().label('E-Mail') : string().nullable(),
  birthDate: string().nullable(),
  bio: string().nullable(),
  password:
    showAccountSettings.value
      ? route.name === 'create-user'
        ? string().required().label('Passwort')
        : string().nullable().label('Passwort')
      : string().nullable(),
  passwordConfirm:
    showAccountSettings.value
      ? route.name === 'create-user'
        ? string()
            .required()
            .oneOf([yupRef('password')], 'Passwörter stimmen nicht überein')
        : string().nullable().label('Passwort bestätigen')
      : string().nullable(),
  avatar: mixed().nullable(),
}))

const profileSettings = computed(() => {
  return route.name === 'profile-settings'
})

const dateToday = computed(() => {
  return new Date().toISOString().slice(0, 10)
})

const userLocked = computed(() => {
  return store.user?.locked
})

const cardTitle = computed(() => {
  return route.name === 'create-user' ? 'Nutzer erstellen' : 'Nutzer bearbeiten'
})

onMounted(() => {
  // Set initial values when editing a users
  if (
    store.user
    && (route.name === 'edit-user' || route.name === 'profile-settings')
  ) {
    const { gender, interests, birthDate, ...rest } = JSON.parse(
      JSON.stringify(store.user),
    )

    const initialValues = {
      ...rest,
      password: null,
      passwordConfirm: null,
      gender,
      interests,
      birthDate,
      avatar: store.user.avatar,
    }

    form.value?.resetForm({
      values: initialValues,
    })
  }
})

// Check if changes were made before the dialog is closed
function cancel(dirty?: boolean) {
  if (dirty) {
    discardDialog.value = true
  }
  else {
    close()
  }
}

function onFileChange(e: any) {
  const files = e.target.files || e.dataTransfer.files
  if (!files.length)
    return
  file.value = e.target.files[0]
}

/**
 * Return to profile or data-management
 */
function close() {
  profileSettings.value
    ? router.push({ name: 'profile' })
    : router.push({ name: 'data-management' })
}

/**
 * Submit values and create or update user
 * @param values
 */
async function submit(values: any) {
  const { passwordConfirm, ...rest } = values
  const updatedValues = { ...rest }

  if (route.name === 'create-user') {
    await store.createUser(updatedValues)
  }
  else {
    await store.updateUser(updatedValues)
  }
  close()
}
</script>

<template>
  <v-dialog v-model="dialog" persistent width="500">
    <v-card :title="cardTitle" :loading="store.crudCardLoading">
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
              accept="image/*"
              @change="handleChange($event), onFileChange($event)"
              @blur="handleBlur"
            >
          </Field>
          <Avatar :file="file" @click="fileInput?.click()" />
        </v-row>
        <v-tabs v-model="tab" fixed-tabs>
          <v-tab :value="1">
            Profil
          </v-tab>
          <v-tab v-if="showAccountSettings" :value="2">
            Konto
          </v-tab>
        </v-tabs>
        <v-window v-model="tab">
          <v-window-item eager :value="1">
            <v-card-text>
              <v-row>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="name"
                    label="Name"
                    type="text"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="birthDate"
                    label="Geburtsdatum"
                    type="date"
                    :clearable="false"
                    :max="dateToday"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12">
                  <BaseSelectWithValidation
                    name="gender"
                    label="Geschlecht"
                    :items="gender"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12">
                  <BaseCombobox
                    name="interests"
                    label="Interessen"
                    :items="interests"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12">
                  <BaseTextarea
                    label="Bio"
                    type="text"
                    name="bio"
                    auto-grow
                    :disabled="userLocked"
                  />
                </v-col>
              </v-row>
            </v-card-text>
          </v-window-item>
          <v-window-item v-if="showAccountSettings" eager :value="2">
            <v-card-text>
              <v-row>
                <v-col cols="12">
                  <BaseSelectWithValidation
                    name="role"
                    label="Rolle"
                    :items="roles"
                    :disabled="userLocked || modCreatesUser"
                  />
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="username"
                    label="Benutzername"
                    type="text"
                    :disabled="userLocked"
                    prefix="@"
                    @keydown.space.prevent
                  />
                </v-col>
                <v-col cols="12">
                  <BaseInputWithValidation
                    name="email"
                    label="E-Mail"
                    type="text"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12" sm="6">
                  <BasePasswordInput
                    name="password"
                    label="Passwort"
                    :disabled="userLocked"
                  />
                </v-col>
                <v-col cols="12" sm="6">
                  <BasePasswordInput
                    name="passwordConfirm"
                    label="Passwort bestätigen"
                    :disabled="userLocked"
                  />
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
          />
          <v-spacer />
          <v-btn variant="tonal" @click="cancel(meta.dirty)">
            Abbrechen
          </v-btn>

          <v-btn
            type="submit"
            :disabled="!meta.valid || !meta.dirty"
            variant="tonal"
          >
            Speichern
          </v-btn>
          <v-btn
            v-if="tab < 2 && showAccountSettings"
            variant="plain"
            icon="mdi-chevron-right"
            @click="tab++"
          />
        </v-card-actions>
      </Form>
    </v-card>
  </v-dialog>

  <BaseDiscardDialog
    v-model="discardDialog"
    @cancel="discardDialog = false"
    @discard="cancel"
  />
</template>
