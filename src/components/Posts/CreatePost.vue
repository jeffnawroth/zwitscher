<template>
  <Form
    v-slot="{ meta, validate }"
    :initial-values="initialValues"
    :validation-schema="validationSchema"
    @submit="submit"
  >
    <v-card
      :prepend-avatar="authStore.user?.avatar"
      :title="cardTitle"
      :subtitle="cardSubtitle"
    >
      <v-card-text>
        <Field v-slot="{ field }" name="text">
          <v-textarea
            v-bind="field"
            :placeholder="placeholder"
            variant="outlined"
            clearable
            counter="281"
            hide-details="auto"
            :rows="3"
            auto-grow
            persistent-counter
          ></v-textarea>
        </Field>
        <FileLayout
          class="mt-2"
          :files="files"
          remove-file-btn
          @remove-file="
            (file: File) => {
              removeFile(file);
              validate();
            }
          "
        ></FileLayout>
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
          />
        </Field>
        <v-btn
          icon="mdi-image-outline"
          :disabled="files.length == 4 || files[0]?.type == 'video/mp4'"
          @click="fileInput?.click()"
        ></v-btn>
        <v-btn :disabled="files.length >= 1" icon="mdi-file-gif-box"></v-btn>
        <v-btn icon="mdi-emoticon-happy-outline"></v-btn>
        <v-spacer></v-spacer>
        <v-btn variant="tonal" type="submit" :disabled="!meta.valid">{{
          buttonText
        }}</v-btn>
      </v-card-actions>
    </v-card>
  </Form>
</template>

<script setup lang="ts">
import { AddPost, Post } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { computed, ref } from "vue";
import { mixed, object, setLocale, string } from "yup";
import { Form, Field } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import router from "@/router";
import { useUsersStore } from "@/store/users";
import FileLayout from "./FileLayout.vue";

setLocale(yupLocaleDe);

const authStore = useAuthenticationStore();
const postsStore = usePostStore();
const usersStore = useUsersStore();

const fileInput = ref<HTMLInputElement | null>(null);

const files = ref<File[]>([]);

const initialValues = {
  text: "",
  file: [],
};

const validationSchema = object({
  text: string()
    .max(281)
    .when("file", {
      is: (file: File[]) => file && file.length > 0,
      then: (schema) => schema.nullable(),
      otherwise: (schema) => schema.required(),
    }),
  file: mixed(),
});

const placeholder = computed(() => {
  return router.currentRoute.value.name == "home"
    ? "Was gibt's neues?"
    : "Kommentieren";
});

const buttonText = computed(() => {
  return router.currentRoute.value.name == "home"
    ? "Veröffentlichen"
    : "Kommentieren";
});

const cardSubtitle = computed(() => {
  return router.currentRoute.value.name == "home"
    ? `@${authStore.user?.username}`
    : `Antworten auf @${usersStore.user?.username}`;
});

const cardTitle = computed(() => {
  return router.currentRoute.value.name == "home"
    ? `${authStore.user?.firstName} ${authStore.user?.lastName}`
    : "";
});

function removeFile(file: File) {
  const fileIndex = files.value.indexOf(file);
  files.value.splice(fileIndex, 1);
}

function submit(values: any, { resetForm }: any) {
  const post: AddPost = {
    userId: authStore.user!.id,
    text: values.text,
    files: values.file,
  };

  router.currentRoute.value.name == "home"
    ? postsStore.createPost(post)
    : postsStore.addComment(post);
  resetForm();
}
</script>

<style>
video {
  width: 100%;
  height: auto;
}
</style>
