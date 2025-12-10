<script lang="ts" setup>
import { ref } from 'vue'
import PageToolbar from '@/components/PageToolbar.vue'
import PostManagement from '@/components/PostManagement.vue'
import UserManagement from './UserManagement.vue'

const panel = ref([])
</script>

<template>
  <PageToolbar class="mb-4" icon="mdi-folder" title="Verwaltung" />

  <v-container>
    <v-expansion-panels v-model="panel">
      <v-expansion-panel :value="0">
        <v-expansion-panel-title>
          <v-icon class="mr-3" icon="mdi-account-group" />
          <p class="font-weight-bold">
            Benutzerverwaltung
          </p>
        </v-expansion-panel-title>
        <v-expansion-panel-text>
          <UserManagement />
        </v-expansion-panel-text>
      </v-expansion-panel>
      <v-expansion-panel v-model="panel" :value="1">
        <v-expansion-panel-title>
          <v-icon class="mr-3" icon="mdi-post" />
          <p class="font-weight-bold">
            Beitragsverwaltung
          </p>
        </v-expansion-panel-title>
        <v-expansion-panel-text>
          <PostManagement />
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-container>
</template>
