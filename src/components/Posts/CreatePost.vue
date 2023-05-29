<template>
  <Form
    v-slot="{ meta, validate }"
    :initial-values="initialValues"
    :validation-schema="validationSchema"
    @submit="submit"
  >
    <v-card :title="cardTitle">
      <template #subtitle>
        <div v-if="route.name == 'post'">
          Antworten auf
          <router-link
            class="text-decoration-none"
            :to="{
              name: 'profile',
              params: { username: postsStore.post?.username },
            }"
            >{{ ` @${postsStore.post?.username}` }}</router-link
          >
        </div>
        <div v-else>
          {{ `@${authStore.user?.username}` }}
        </div>
      </template>
      <template #prepend>
        <v-avatar v-if="!authStore.user?.avatar" size="40" color="grey">
          <v-icon icon="mdi-account-circle" size="40"></v-icon>
        </v-avatar>
        <v-img v-else>
          <v-avatar :image="generateFileURL(authStore.user?.avatar)">
          </v-avatar>
        </v-img>
      </template>
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
        ></BaseTextarea>
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
import { AddPost } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { computed, ref } from "vue";
import { mixed, object, setLocale, string } from "yup";
import { Form, Field } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useRoute } from "vue-router";
import { useUsersStore } from "@/store/users";
import FileLayout from "./FileLayout.vue";
import BaseTextarea from "../BaseComponents/BaseTextarea.vue";
import { generateFileURL } from "@/helpers";

setLocale(yupLocaleDe);

const authStore = useAuthenticationStore();
const postsStore = usePostStore();
const usersStore = useUsersStore();
const route = useRoute();

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
  return route.name == "home" ? "Was gibt's neues?" : "Antworten";
});

const buttonText = computed(() => {
  return route.name == "home" ? "Zwitschern" : "Antworten";
});

const cardSubtitle = computed(() => {
  return route.name == "home"
    ? `@${authStore.user?.username}`
    : `Antworten auf @${postsStore.post?.username}`;
});

const cardTitle = computed(() => {
  return route.name == "home" ? `${authStore.user?.name}` : "";
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

  route.name == "home"
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
