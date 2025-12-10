<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { onBeforeRouteUpdate, useRouter } from 'vue-router'
import DeleteUserDialog from '@/components/DeleteUserDialog.vue'
import IconWithTooltip from '@/components/IconWithTooltip.vue'
import LockUserDialog from '@/components/LockUserDialog.vue'
import PageToolbar from '@/components/PageToolbar.vue'
import PostList from '@/components/Posts/PostList.vue'
import { generateFileURL } from '@/helpers'
import { useAuthenticationStore } from '@/store/authentication'
import { usePostStore } from '@/store/posts'
import { useUsersStore } from '@/store/users'
import { Role } from '@/typescript-axios-generated'

const store = usePostStore()
const usersStore = useUsersStore()
const authStore = useAuthenticationStore()
const router = useRouter()

const lockDialog = ref(false)
const deleteDialog = ref(false)

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.username !== from.params.username) {
    usersStore.getUserByUsername(to.params.username as string)
    loadPosts(to.params.username as string)
  }
})

onMounted(() => {
  if (!usersStore.user?.username)
    return
  loadPosts(usersStore.user?.username)
})

const following = computed(() => {
  return authStore.user?.following?.includes(usersStore.user!.id!)
})

const birthDate = computed(() => {
  const userBirthDate = usersStore.user?.birthDate

  if (!userBirthDate) {
    return undefined
  }

  const [year, month, day] = userBirthDate.split('-')
  const date = new Date(Number.parseInt(year), Number.parseInt(month) - 1, Number.parseInt(day))
  return date.toLocaleDateString('de-DE', {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
  })
})

const created = computed(() => {
  if (!usersStore.user?.createdAt) {
    return ''
  }
  const date = new Date(usersStore.user?.createdAt)
  return date.toLocaleDateString('de-DE', {
    month: 'long',
    year: 'numeric',
  })
})

const genderText = computed(() => {
  const genderMap = {
    0: 'männlich',
    1: 'weiblich',
    2: 'divers',
  }

  const genderValue = usersStore.user!.gender!
  return genderMap[genderValue]
})

const genderIcon = computed(() => {
  const genderMap = {
    0: 'mdi-gender-male',
    1: 'mdi-gender-female',
    2: 'mdi-gender-non-binary',
  }

  const gender = usersStore.user!.gender!
  return genderMap[gender]
})

/**
 * Follow or unfollow user
 */
async function setFollow() {
  const followingIndex = authStore.user?.following?.indexOf(
    usersStore.user!.id!,
  )
  const followerIndex = usersStore.user?.followers?.indexOf(
    authStore.user!.id!,
  )

  if (followingIndex !== undefined && followingIndex !== -1) {
    authStore.user?.following?.splice(followingIndex, 1)
    if (followerIndex !== undefined && followerIndex !== -1) {
      usersStore.user?.followers?.splice(followerIndex, 1)
    }
    usersStore.unfollowUser(usersStore.user!.id!)
  }
  else {
    authStore.user?.following?.push(usersStore.user!.id!)
    usersStore.user?.followers?.push(authStore.user!.id!)
    usersStore.followUser(usersStore.user!.id!)
  }
  authStore.setUserData(authStore.user)
}

/**
 * Load all posts for a specific user
 */
function loadPosts(username: string) {
  store.getPostsForUser(username)
}
</script>

<template>
  <PageToolbar icon="mdi-account" title="Profil" />
  <v-card flat>
    <template #prepend>
      <v-avatar v-if="!usersStore.user?.avatar" size="75" color="grey">
        <v-icon icon="mdi-account-circle" size="60" />
      </v-avatar>
      <v-img v-else>
        <v-avatar
          size="75"
          class="avatar"
          :image="generateFileURL(usersStore.user?.avatar)"
        />
      </v-img>
    </template>
    <template #append>
      <div v-if="authStore.loggedIn">
        <IconWithTooltip
          v-if="$route.params.username !== authStore.user?.username"
          :text="following ? 'Entfolgen' : 'Folgen'"
          :icon="following ? 'mdi-account-check' : 'mdi-account-plus'"
          @click="setFollow"
        />
        <IconWithTooltip
          v-if="
            $route.params.username === authStore.user?.username
              || authStore.user?.role === Role.NUMBER_0
          "
          :text="
            $route.params.username === authStore.user?.username
              ? 'Profil bearbeiten'
              : 'Nutzer bearbeiten'
          "
          icon="mdi-account-edit"
          @click="router.push({ name: 'profile-settings' })"
        />
        <template
          v-if="
            (authStore.user?.role === Role.NUMBER_0
              || authStore.user?.role === Role.NUMBER_1)
              && $route.params.username !== authStore.user?.username
          "
        >
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
          />
          <IconWithTooltip
            text="Nutzer löschen"
            icon="mdi-account-remove"
            @click="deleteDialog = true"
          />
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
          {{ `${usersStore.user?.followers?.length}` }}
        </span>
        Abonnenten

        <span class="font-weight-black">{{
          `${usersStore.user?.following?.length}`
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
  <v-divider />

  <PostList
    :posts="store.sortedUserPosts"
    no-posts-message="Der Nutzer hat noch keine Beiträge veröffentlicht."
  />
  <router-view />

  <DeleteUserDialog v-model="deleteDialog" />
  <LockUserDialog v-model="lockDialog" />
</template>
