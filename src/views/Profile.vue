<template>
  <v-list rounded="lg">
    <v-list-item>
      <v-card>
        <template #prepend>
          <v-img>
            <v-avatar
              :size="150"
              class="avatar"
              :image="usersStore.user?.avatar"
            ></v-avatar>
          </v-img>
        </template>
        <template #title>
          <div class="d-flex">
            {{ `${usersStore.user?.firstName} ${usersStore.user?.lastName}` }}
            <v-spacer></v-spacer>
            <v-hover
              v-if="Number($route.params.id) !== authStore.user?.id"
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
            <v-btn v-else variant="tonal">Profil bearbeiten</v-btn>
          </div>
        </template>
        <template #subtitle>
          <p>{{ `@${usersStore.user?.username}` }}</p>
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

    <PostList :posts="store.sortedUserPosts"></PostList>
  </v-list>
</template>

<script setup lang="ts">
import { usePostStore } from "@/store/posts";
import { computed, onMounted } from "vue";
import { useUsersStore } from "@/store/users";
import { onBeforeRouteUpdate } from "vue-router";
import PostList from "@/components/Posts/PostList.vue";
import { useAuthenticationStore } from "@/store/authentication";

const store = usePostStore();
const usersStore = useUsersStore();
const authStore = useAuthenticationStore();

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.id !== from.params.id) {
    usersStore.getUser(Number(to.params.id));
    store.getPostsForUser(usersStore.user!.id);
  }
});

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
