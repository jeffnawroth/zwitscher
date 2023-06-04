<template>
  <PageToolbar title="Profil"></PageToolbar>
  <v-list rounded="lg">
    <v-list-item>
      <v-card>
        <template #prepend>
          <v-avatar v-if="!usersStore.user?.avatar" size="75" color="grey">
            <v-icon icon="mdi-account-circle" size="75"></v-icon>
          </v-avatar>
          <v-img v-else>
            <v-avatar
              size="75"
              class="avatar"
              :image="generateFileURL(usersStore.user?.avatar)"
            ></v-avatar>
          </v-img>
        </template>
        <template #append>
          <div v-if="authStore.loggedIn">
            <IconWithTooltip
              v-if="$route.params.username !== authStore.user?.username"
              :text="following ? 'Entfolgen' : 'Folgen'"
              :icon="following ? 'mdi-account-check' : 'mdi-account-plus'"
              @click="setFollow"
            ></IconWithTooltip>
            <IconWithTooltip
              v-if="
                $route.params.username == authStore.user?.username ||
                authStore.user?.role == Role.NUMBER_0
              "
              :text="
                $route.params.username == authStore.user?.username
                  ? 'Profil bearbeiten'
                  : 'Nutzer bearbeiten'
              "
              icon="mdi-account-edit"
              @click="router.push({ name: 'profile-settings' })"
            ></IconWithTooltip>
            <template
              v-if="
                (authStore.user?.role == Role.NUMBER_0 ||
                  authStore.user?.role == Role.NUMBER_1) &&
                $route.params.username != authStore.user?.username
              "
            >
              <IconWithTooltip
                :text="
                  usersStore.user?.locked
                    ? 'Nutzer entsperren'
                    : 'Nutzer sperren'
                "
                :icon="
                  usersStore.user?.locked
                    ? 'mdi-account-lock'
                    : 'mdi-account-lock-open'
                "
                @click="lockDialog = true"
              ></IconWithTooltip>
              <IconWithTooltip
                text="Nutzer löschen"
                icon="mdi-delete"
                @click="deleteDialog = true"
              ></IconWithTooltip>
            </template>
          </div>
        </template>

        <template #title>
          {{ `${usersStore.user?.name}` }}
          <p class="v-card-subtitle">
            {{ `@${usersStore.user?.username}` }}
          </p>
          <p class="v-card-subtitle">
            <span class="font-weight-black">
              {{ `${usersStore.user?.followers.length}` }}
            </span>
            Abonnenten

            <span class="font-weight-black">{{
              `${usersStore.user?.following.length}`
            }}</span>
            Folge ich
          </p>
        </template>
        <template #text>
          <v-chip-group selected-class="">
            <v-chip size="small" prepend-icon="mdi-calendar-range">
              {{ `Beigetreten ${created}` }}
            </v-chip>
            <v-chip
              v-if="usersStore.user?.birthDate"
              size="small"
              prepend-icon="mdi-cake"
            >
              {{ `Geboren ${birthDate}` }}
            </v-chip>
            <v-chip
              v-if="usersStore.user?.gender != null"
              size="small"
              :prepend-icon="genderIcon"
            >
              {{ genderText }}
            </v-chip>
          </v-chip-group>

          <v-chip-group selected-class="" variant="outlined">
            <v-chip
              v-for="interest in usersStore.user?.interests"
              :key="interest"
              size="small"
            >
              {{ interest }}
            </v-chip>
          </v-chip-group>

          <span>{{ usersStore.user?.bio }}</span>
        </template>
      </v-card>
    </v-list-item>
    <v-divider></v-divider>

    <PostList
      v-if="store.sortedUserPosts.length > 0"
      :posts="store.sortedUserPosts"
    ></PostList>
    <v-list-item v-else class="d-flex justify-center">
      Der Nutzer hat noch keine Beiträge veröffentlicht.
    </v-list-item>
  </v-list>
  <router-view></router-view>

  <DeleteUserDialog v-model="deleteDialog"></DeleteUserDialog>
  <LockUserDialog v-model="lockDialog"></LockUserDialog>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import { computed, onMounted, ref } from "vue";
import { useUsersStore } from "@/store/users";
import PostList from "@/components/Posts/PostList.vue";
import { useAuthenticationStore } from "@/store/authentication";
import { generateFileURL } from "@/helpers";
import IconWithTooltip from "@/components/IconWithTooltip.vue";
import LockUserDialog from "@/components/LockUserDialog.vue";
import DeleteUserDialog from "@/components/DeleteUserDialog.vue";
import { onBeforeRouteUpdate, useRouter } from "vue-router";
import { Role } from "@/typescript-axios-generated";
import PageToolbar from "@/components/PageToolbar.vue";

const store = usePostStore();
const usersStore = useUsersStore();
const authStore = useAuthenticationStore();
const router = useRouter();

const lockDialog = ref(false);
const deleteDialog = ref(false);

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.username !== from.params.username) {
    if (to.params.username === authStore.user?.username)
      usersStore.user = authStore.user;
    else usersStore.getUserByUsername(to.params.username as string);

    loadPosts();
  }
});

onMounted(() => {
  loadPosts();
});

const following = computed(() => {
  return authStore.user?.following.includes(usersStore.user!.id);
});

const birthDate = computed(() => {
  const userBirthDate = usersStore.user?.birthDate;

  if (!userBirthDate) {
    return undefined;
  }

  const [year, month, day] = userBirthDate.split("-");
  const date = new Date(parseInt(year), parseInt(month) - 1, parseInt(day));
  return date.toLocaleDateString("de-DE", {
    day: "numeric",
    month: "long",
    year: "numeric",
  });
});

const created = computed(() => {
  const date = new Date(usersStore.user?.createdAt!);
  return date.toLocaleDateString("de-DE", {
    month: "long",
    year: "numeric",
  });
});

const genderText = computed(() => {
  const genderMap = {
    0: "männlich",
    1: "weiblich",
    2: "divers",
  };

  const genderValue = usersStore.user!.gender!;
  return genderMap[genderValue];
});

const genderIcon = computed(() => {
  const genderMap = {
    0: "mdi-gender-male",
    1: "mdi-gender-female",
    2: "mdi-gender-non-binary",
  };

  const gender = usersStore.user!.gender!;
  return genderMap[gender];
});

function setFollow() {
  const followingIndex = authStore.user?.following.indexOf(usersStore.user!.id);
  const followerIndex = usersStore.user?.followers.indexOf(authStore.user!.id);

  if (followingIndex != undefined && followingIndex !== -1) {
    authStore.user?.following.splice(followingIndex, 1);
    if (followerIndex != undefined && followerIndex !== -1) {
      usersStore.user?.followers.splice(followerIndex, 1);
    }
  } else {
    authStore.user?.following.push(usersStore.user!.id);
    usersStore.user?.followers.push(authStore.user!.id);
  }
}

function loadPosts() {
  store.getPostsForUser(usersStore.user!.id);
}
</script>
