<template>
  <v-card :title="cardTitle" flat>
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
      <v-avatar v-if="!authStore.user?.avatar" color="grey">
        <v-icon icon="mdi-account-circle" size="x-large"></v-icon>
      </v-avatar>
      <v-img v-else>
        <v-avatar :image="generateFileURL(authStore.user?.avatar)"> </v-avatar>
      </v-img>
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
        ></BaseTextarea>
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
        <v-btn
          v-if="!props.editMode"
          variant="tonal"
          type="submit"
          :disabled="!meta.valid"
          >{{ buttonText }}</v-btn
        >

        <template v-if="editMode">
          <v-btn icon="mdi-close" @click="$emit('set-edit-mode', false)">
          </v-btn>
          <v-btn
            :disabled="!meta.valid || !meta.dirty"
            icon="mdi-check"
            type="submit"
          ></v-btn>
        </template>
      </v-card-actions>
    </Form>
  </v-card>
</template>

<script setup lang="ts">
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { computed, ref } from "vue";
import { mixed, object, setLocale, string } from "yup";
import { Form, Field } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import { useRoute } from "vue-router";
import FileLayout from "./FileLayout.vue";
import BaseTextarea from "../BaseComponents/BaseTextarea.vue";
import { generateFileURL } from "@/helpers";
import { onMounted } from "vue";
import { PropType } from "vue";
import { PostAdd, PostResult } from "@/typescript-axios-generated";

const emit = defineEmits<{
  (e: "set-edit-mode", value: boolean): void;
  (e: "close-dialog"): void;
}>();

const props = defineProps({
  post: {
    type: Object as PropType<PostResult>,
    default: null,
  },
  editMode: {
    type: Boolean,
  },
});

setLocale(yupLocaleDe);

const authStore = useAuthenticationStore();
const postsStore = usePostStore();
const route = useRoute();

const fileInput = ref<HTMLInputElement | null>(null);

const files = ref<File[]>([]);

const form = ref<InstanceType<typeof Form> | null>(null);

const initialValues = {
  text: "",
  file: [] as File[] | string[],
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

const cardTitle = computed(() => {
  return route.name == "home" ? `${authStore.user?.name}` : "";
});

onMounted(() => {
  if (props.post) {
    if (props.post.text) {
      initialValues.text = props.post.text;
    }
    if (props.post.files) {
      initialValues.file = props.post.files;
    }

    form.value?.resetForm({
      values: initialValues,
    });
  }
});

function removeFile(file: File) {
  const fileIndex = files.value.indexOf(file);
  files.value.splice(fileIndex, 1);
}

async function submit(values: any, { resetForm }: any) {
  if (props.editMode) {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const { text, files, ...rest } = props.post;
    const postEdit = {
      ...rest,
      text: values.text,
      files: values.file,
    };
    await postsStore.updatePost(postEdit);
    emit("set-edit-mode", false);
  } else {
    const post: PostAdd = {
      userId: authStore.user!.id,
      text: values.text,
      files: values.file,
    };

    route.name == "home"
      ? await postsStore.createPost(post)
      : await postsStore.addComment(post);
    resetForm();
    emit("close-dialog");
  }
}
</script>

<style>
video {
  width: 100%;
  height: auto;
}
</style>
