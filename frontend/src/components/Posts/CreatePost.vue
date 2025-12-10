<script setup lang="ts">
import type { PropType } from 'vue'
import type { CommentAdd, PostAdd, PostResult } from '@/typescript-axios-generated'
import { Field, Form } from 'vee-validate'
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { mixed, object, setLocale, string } from 'yup'
import { generateFileURL } from '@/helpers'
import yupLocaleDe from '@/plugins/yupLocaleDe'
import { useAuthenticationStore } from '@/store/authentication'
import { usePostStore } from '@/store/posts'
import BaseTextarea from '../BaseComponents/BaseTextarea.vue'
import FileLayout from './FileLayout.vue'

const props = defineProps({
  post: {
    type: Object as PropType<PostResult>,
    default: null,
  },
  editMode: {
    type: Boolean,
  },
})

const emit = defineEmits<{
  (e: 'setEditMode', value: boolean): void
  (e: 'closeDialog'): void
}>()

setLocale(yupLocaleDe)

const authStore = useAuthenticationStore()
const postsStore = usePostStore()
const route = useRoute()

const fileInput = ref<HTMLInputElement | null>(null)

const files = ref<File[]>([])

const form = ref<InstanceType<typeof Form> | null>(null)

const initialValues = {
  text: '',
  file: [] as File[] | string[],
}

const validationSchema = object({
  text: string()
    .max(281)
    .when('file', {
      is: (file: File[]) => file && file.length > 0,
      then: schema => schema.nullable(),
      otherwise: schema => schema.required(),
    }),
  file: mixed(),
})

const placeholder = computed(() => {
  return route.name === 'home' ? 'Was gibt\'s neues?' : 'Antworten'
})

const buttonText = computed(() => {
  return route.name === 'home' ? 'Zwitschern' : 'Antworten'
})

const cardTitle = computed(() => {
  return route.name === 'post'
    ? ''
    : props.editMode
      ? props.post.name!
      : `${authStore.user?.name}`
})

onMounted(() => {
  // Editmode: Set values
  if (props.post) {
    if (props.post.text) {
      initialValues.text = props.post.text
    }
    if (props.post.files) {
      initialValues.file = props.post.files
    }

    form.value?.resetForm({
      values: initialValues,
    })
  }
})

/**
 * Remove a file from array
 * @param file
 */
function removeFile(file: File) {
  const fileIndex = files.value.indexOf(file)
  files.value.splice(fileIndex, 1)
}

/**
 * Create or update a post or add a comment
 * @param values
 * @param resetForm
 */
async function submit(values: any, { resetForm }: any) {
  if (props.editMode) {
    const { text, files, ...rest } = props.post
    const postEdit = {
      ...rest,
      text: values.text,
      files: values.file,
    }
    await postsStore.updatePost(postEdit)
    emit('setEditMode', false)
  }
  else {
    const post: PostAdd | CommentAdd = {
      userId: authStore.user!.id,
      text: values.text,
      files: values.file,
    }

    route.name === 'home'
      ? await postsStore.createPost(post)
      : await postsStore.addComment({
          ...post,
          parentPostId: postsStore.post?.id,
        })
    resetForm()
    emit('closeDialog')
  }
}
</script>

<template>
  <v-card :title="cardTitle" flat>
    <template #subtitle>
      <div v-if="route.name === 'post'">
        Antworten auf
        <router-link
          class="text-decoration-none"
          :to="{
            name: 'profile',
            params: { username: postsStore.post?.username },
          }"
        >
          {{ ` @${postsStore.post?.username}` }}
        </router-link>
      </div>
      <div v-else>
        {{ editMode ? `@${post.username}` : `@${authStore.user?.username}` }}
      </div>
    </template>
    <template #prepend>
      <template v-if="editMode">
        <v-avatar v-if="!post.avatar" color="grey">
          <v-icon icon="mdi-account-circle" size="x-large" />
        </v-avatar>
        <v-img v-else>
          <v-avatar :image="generateFileURL(post.avatar)" />
        </v-img>
      </template>
      <template v-else>
        <v-avatar v-if="!authStore.user?.avatar" color="grey">
          <v-icon icon="mdi-account-circle" size="x-large" />
        </v-avatar>
        <v-img v-else>
          <v-avatar :image="generateFileURL(authStore.user?.avatar)" />
        </v-img>
      </template>
    </template>
    <Form
      ref="form"
      v-slot="{ meta, validate }"
      :initial-values="initialValues"
      :validation-schema="validationSchema"
      @submit="submit"
    >
      <v-card-text>
        <BaseTextarea
          type="text"
          name="text"
          :placeholder="placeholder"
          flat
          variant="solo"
          clearable
          counter="281"
          :rows="1"
          persistent-counter
          error-messages=""
        />
        <FileLayout
          v-if="files"
          class="mt-2"
          :files="files"
          remove-file-btn
          @remove-file="
            (file: File) => {
              removeFile(file);
              validate();
            }
          "
        />
      </v-card-text>
      <v-card-actions>
        <Field
          v-slot="{ handleChange, handleBlur }"
          v-model="files"
          name="file"
        >
          <input
            ref="fileInput"
            multiple
            hidden
            type="file"
            accept="image/*, video/*"
            @change="handleChange"
            @blur="handleBlur"
          >
        </Field>
        <v-btn
          icon="mdi-image-outline"
          :disabled="files.length === 4 || files[0]?.type === 'video/mp4'"
          @click="fileInput?.click()"
        />
        <v-btn :disabled="files.length >= 1" icon="mdi-file-gif-box" />
        <v-btn icon="mdi-emoticon-happy-outline" />
        <v-spacer />
        <v-btn
          v-if="!props.editMode"
          variant="tonal"
          type="submit"
          :disabled="!meta.valid"
          :loading="postsStore.crudCardLoading"
        >
          {{ buttonText }}
        </v-btn>

        <template v-if="editMode">
          <v-btn icon="mdi-close" @click="$emit('setEditMode', false)" />
          <v-btn
            :disabled="!meta.valid || !meta.dirty"
            icon="mdi-check"
            type="submit"
          />
        </template>
      </v-card-actions>
    </Form>
  </v-card>
</template>

<style>
video {
  width: 100%;
  height: auto;
}
</style>
