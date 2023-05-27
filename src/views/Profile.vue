<template>
  <v-list rounded="lg">
    <v-list-item>
      <v-card>
        <template #prepend>
          <v-img>
            <v-avatar
              :size="175"
              class="avatar"
              :image="generateFileURL(usersStore.user?.avatar)"
            ></v-avatar>
          </v-img>
        </template>
        <template #title>
          <div class="d-flex mb-1">
            <div>
              {{ `${usersStore.user?.name}` }}
              <p class="v-card-subtitle">
                {{ `@${usersStore.user?.username}` }}
              </p>
            </div>
            <v-spacer></v-spacer>
            <div v-if="authStore.loggedIn">
              <v-hover
                v-if="$route.params.username !== authStore.user?.username"
                v-slot="{ isHovering, props }"
              >
                <v-btn
                  v-bind="props"
                  :color="follow == 'Folge ich' && isHovering ? 'red' : ''"
                  :variant="followButtonVariant"
                  @click="setFollow"
                  >{{
                    follow == "Folge ich" && isHovering ? "Entfolgen" : follow
                  }}</v-btn
                >
              </v-hover>
              <v-btn
                v-if="
                  $route.params.username === authStore.user?.username ||
                  authStore.user?.role === 'Admin'
                "
                variant="tonal"
                @click="router.push({ name: 'profile-settings' })"
                >Profil bearbeiten</v-btn
              >
              <IconWithTooltip
                :text="
                  usersStore.user?.locked
                    ? 'Nutzer entsperren'
                    : 'Nutzer sperren'
                "
                :icon="usersStore.user?.locked ? 'mdi-lock' : 'mdi-lock-open'"
                @click="lockDialog = true"
              ></IconWithTooltip>
              <IconWithTooltip
                text="Nutzer löschen"
                icon="mdi-delete"
                @click="deleteDialog = true"
              ></IconWithTooltip>
            </div>
          </div>
        </template>
        <template #subtitle>
          <p>
            <v-icon size="small">mdi-calendar-range</v-icon>
            {{ `Beigetreten: ${created}` }}
          </p>
          <p v-if="usersStore.user?.birthdate">
            <v-icon size="small">mdi-cake</v-icon>
            {{ `Geboren: ${birtdate}` }}
          </p>
          <p v-if="usersStore.user?.gender">
            <v-icon size="small">{{ genderIcon }}</v-icon>
            {{ usersStore.user?.gender }}
          </p>

          <p v-if="usersStore.user?.interests" class="mb-1">
            <v-icon size="small">mdi-heart-outline</v-icon>
            Interessen:
            <span
              v-for="(interest, index) in usersStore.user?.interests"
              :key="interest"
              >{{ interest
              }}{{
                usersStore.user?.interests &&
                index != usersStore.user?.interests?.length - 1
                  ? ", "
                  : ""
              }}
            </span>
          </p>

          <p class="text-bold">
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
          {{ usersStore.user?.bio }}
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

const follow = computed(() => {
  return authStore.user?.following.includes(usersStore.user!.id)
    ? "Folge ich"
    : "Folgen";
});

const followButtonVariant = computed(() => {
  return authStore.user?.following.includes(usersStore.user!.id)
    ? "tonal"
    : "outlined";
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
