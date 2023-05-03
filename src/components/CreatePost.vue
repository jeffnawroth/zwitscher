<template>
  <Form v-slot="{ meta }" :initial-values="initialValues" @submit="submit">
    <v-card
      :prepend-avatar="authStore.user?.avatar"
      :title="`${authStore.user?.firstName} ${authStore.user?.lastName}`"
      :subtitle="`@${authStore.user?.username}`"
    >
      <v-card-text>
        <Field v-slot="{ field }" name="text" :rules="fieldSchema">
          <v-textarea
            v-bind="field"
            placeholder="Was gibt's neues?"
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
        <v-btn variant="tonal" type="submit" :disabled="!meta.valid"
          >Veröffentlichen</v-btn
        >
      </v-card-actions>
    </v-card>
  </Form>
</template>

<script setup lang="ts">
import { AddPost, Post } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { ref } from "vue";
import { setLocale, string } from "yup";
import { Form, Field } from "vee-validate";
import yupLocaleDe from "@/plugins/yupLocaleDe";

setLocale(yupLocaleDe);

const authStore = useAuthenticationStore();
const postsStore = usePostStore();

const initialValues = {
  text: "",
};

const fieldSchema = string().required().max(281);

function submit(values: any, { resetForm }: any) {
  const post: AddPost = {
    userId: authStore.user!.id,
    text: values.text,
  };
  postsStore.createPost(post);
  resetForm();
}
</script>
