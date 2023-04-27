<template>
  <v-container class="fill-height justify-center">
    <v-row>
      <v-col class="d-flex justify-center">
        <h1>Willkommen!</h1>
      </v-col>
      <v-col class="d-flex justify-center" cols="12">
        <h2>
          Um den Mikroblogging-Dienst zu nutzen, müssen Sie sich
          <router-link :to="{ name: 'login' }"> Anmelden </router-link>
        </h2>
        <h2>
          &nbsp;oder
          <router-link :to="{ name: 'register' }"> Registrieren </router-link>
        </h2>
      </v-col>
    </v-row>
  </v-container>
</template>
