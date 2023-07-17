<template>
  <PageToolbar class="mb-4" icon="mdi-folder" title="Verwaltung"></PageToolbar>

  <v-container>
    <v-expansion-panels v-model="panel">
      <v-expansion-panel :value="0">
        <v-expansion-panel-title>
          <v-icon class="mr-3" icon="mdi-account-group"></v-icon>
          <p class="font-weight-bold">Benutzerverwaltung</p>
        </v-expansion-panel-title>
        <v-expansion-panel-text>
          <UserManagement></UserManagement>
        </v-expansion-panel-text>
      </v-expansion-panel>
      <v-expansion-panel v-model="panel" :value="1">
        <v-expansion-panel-title>
          <v-icon class="mr-3" icon="mdi-post"></v-icon>
          <p class="font-weight-bold">Beitragsverwaltung</p>
        </v-expansion-panel-title>
        <v-expansion-panel-text>
          <PostManagement></PostManagement>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-container>
</template>

<script lang="ts" setup>
import PostManagement from "@/components/PostManagement.vue";
import UserManagement from "./UserManagement.vue";
import PageToolbar from "@/components/PageToolbar.vue";
import { ref } from "vue";

const panel = ref([]);
</script>
