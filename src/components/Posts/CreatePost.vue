<template>
  <Form v-slot="{ meta }" :initial-values="initialValues" @submit="submit">
    <v-card
      :prepend-avatar="authStore.user?.avatar"
      :title="cardTitle"
      :subtitle="cardSubtitle"
    >
      <v-card-text>
        <Field v-slot="{ field }" name="text" :rules="fieldSchema">
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
      </v-card-text>
      <v-card-actions>
        <v-btn icon="mdi-image-outline"></v-btn>
        <v-btn icon="mdi-file-gif-box"></v-btn>
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
import { setLocale, string } from "yup";
import { Form, Field } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";
import router from "@/router";
import { useUsersStore } from "@/store/users";

setLocale(yupLocaleDe);

const authStore = useAuthenticationStore();
const postsStore = usePostStore();
const usersStore = useUsersStore();

const initialValues = {
  text: "",
};

const fieldSchema = string().required().max(281);

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

function submit(values: any, { resetForm }: any) {
  const post: AddPost = {
    userId: authStore.user!.id,
    text: values.text,
  };
  router.currentRoute.value.name == "home"
    ? postsStore.createPost(post)
    : postsStore.addComment(post);
  resetForm();
}
</script>
