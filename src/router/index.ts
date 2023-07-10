// Composables
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { useUsersStore } from "@/store/users";
import { Role } from "@/typescript-axios-generated";
import {
  RouteLocationNormalized,
  createRouter,
  createWebHistory,
} from "vue-router";

const routes = [
  {
    path: "/",
    name: "home",
    component: () => import("@/views/Home.vue"),
    children: [
      {
        path: "/login",
        name: "login",
        component: () => import("@/views/Login.vue"),
      },
      {
        path: "/register",
        name: "register",
        component: () => import("@/views/Register.vue"),
      },
    ],
  },
  {
    path: "/:username",
    name: "profile",
    component: () => import("@/views/Profile.vue"),
    beforeEnter: async (to: RouteLocationNormalized) => {
      const store = useUsersStore();
      await store.getUserByUsername(to.params.username as string);
    },

    children: [
      {
        path: "settings",
        name: "profile-settings",
        component: () => import("@/components/UserDialog.vue"),
        meta: { requiresAuth: true },
        beforeEnter: (to: RouteLocationNormalized) => {
          const authStore = useAuthenticationStore();
          if (
            !(
              (authStore.loggedIn && authStore.user?.role == Role.NUMBER_0) ||
              to.params.username === authStore.user?.username
            )
          ) {
            return {
              name: "profile",
              params: { username: to.params.username },
            };
          }
        },
      },
    ],
  },

  {
    path: "/:username/post/:postId",
    name: "post",
    component: () => import("@/views/PostDetails.vue"),
    beforeEnter: async (to: RouteLocationNormalized) => {
      const store = usePostStore();
      await store.getPost(to.params.postId as string);
    },
  },

  {
    path: "/dashboard",
    name: "dashboard",
    component: () => import("@/views/Dashboard.vue"),
    meta: { requiresAuth: true },
    beforeEnter: checkAccess,
  },
  {
    path: "/settings",
    name: "settings",
    component: () => import("@/views/Settings.vue"),
    meta: { requiresAuth: true },
  },
  /* {
    path: "/landing-page",
    name: "landing-page",
    component: () => import("@/views/LandingPage.vue"),
  }, */

  {
    path: "/users",
    name: "users",
    component: () => import("@/views/UserManagement.vue"),
    meta: { requiresAuth: true },
    beforeEnter: checkAccess,

    children: [
      {
        name: "create-user",
        path: "create-user",
        component: () => import("@/components/UserDialog.vue"),
        beforeEnter() {
          const store = useUsersStore();
          store.user = {};
        },
      },
      {
        name: "edit-user",
        path: "user/:username",
        component: () => import("@/components/UserDialog.vue"),
        beforeEnter: (to: RouteLocationNormalized) => {
          //TO-DO: Add api
          const store = useUsersStore();
          store.getUserByUsername(to.params.username as string);
        },
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to) => {
  const loggedIn = localStorage.getItem("user");
  if (to.meta.requiresAuth && !loggedIn) return { name: "login" };
});

function checkAccess() {
  const authStore = useAuthenticationStore();

  if (
    !(
      authStore.user?.role == Role.NUMBER_0 ||
      authStore.user?.role == Role.NUMBER_1
    )
  ) {
    return router.back();
  }
}

export default router;
