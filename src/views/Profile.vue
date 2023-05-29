<template>
  <v-list rounded="lg">
    <v-list-item>
      <v-card>
        <template #prepend>
          <v-img>
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
              text="Nutzer bearbeiten"
              icon="mdi-account-edit"
              @click="router.push({ name: 'profile-settings' })"
            ></IconWithTooltip>
            <IconWithTooltip
              :text="
                usersStore.user?.locked ? 'Nutzer entsperren' : 'Nutzer sperren'
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
          </div>
        </template>

        <template #title>
          {{ `${usersStore.user?.name}` }}
          <p class="v-card-subtitle">
            {{ `@${usersStore.user?.username}` }}
          </p>
          <p class="v-card-subtitle">
            <span class="font-weight-black">
              {{ `${usersStore.user?.follower.length}` }}
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
            <v-chip size="x-small" prepend-icon="mdi-calendar-range">
              {{ `Beigetreten ${created}` }}
            </v-chip>
            <v-chip
              v-if="usersStore.user?.birthdate"
              size="x-small"
              prepend-icon="mdi-cake"
            >
              {{ `Geboren ${birtdate}` }}
            </v-chip>
            <v-chip
              v-if="usersStore.user?.gender"
              size="x-small"
              :prepend-icon="genderIcon"
            >
              {{ usersStore.user?.gender }}
            </v-chip>
          </v-chip-group>

          <v-chip-group selected-class="" variant="outlined">
            <v-chip
              v-for="interest in usersStore.user?.interests"
              :key="interest"
              size="x-small"
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
import { useRouter } from "vue-router";

const store = usePostStore();
const usersStore = useUsersStore();
const authStore = useAuthenticationStore();
const router = useRouter();

const lockDialog = ref(false);
const deleteDialog = ref(false);

onMounted(() => {
  store.getPostsForUser(usersStore.user!.id);
});

const following = computed(() => {
  return authStore.user?.following.includes(usersStore.user!.id);
});

const birtdate = computed(() => {
  const birthdate = usersStore.user?.birthdate;

  if (!birthdate) {
    return undefined;
  }

  const [year, month, day] = birthdate.split("-");
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

const genderIcon = computed(() => {
  const gender = usersStore.user?.gender;

  if (gender === "männlich") {
    return "mdi-gender-male";
  } else if (gender === "weiblich") {
    return "mdi-gender-female";
  } else {
    return "mdi-gender-non-binary";
  }
});

function setFollow() {
  const followingIndex = authStore.user?.following.indexOf(usersStore.user!.id);
  const followerIndex = usersStore.user?.follower.indexOf(authStore.user!.id);

  if (followingIndex != undefined && followingIndex !== -1) {
    authStore.user?.following.splice(followingIndex, 1);
    if (followerIndex != undefined && followerIndex !== -1) {
      usersStore.user?.follower.splice(followerIndex, 1);
    }
  } else {
    authStore.user?.following.push(usersStore.user!.id);
    usersStore.user?.follower.push(authStore.user!.id);
  }
}
</script>
