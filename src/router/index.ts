// Composables
import { User } from "@/interfaces";
import { useAuthenticationStore } from "@/store/authentication";
import { usePostStore } from "@/store/posts";
import { useUsersStore } from "@/store/users";
import { Role } from "@/typescript-axios-generated";
import {
  NavigationGuardNext,
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
    beforeEnter(
      to: RouteLocationNormalized,
      from: RouteLocationNormalized,
      next: NavigationGuardNext
    ) {
      //TO-DO: Add api
      const store = useUsersStore();
      const authStore = useAuthenticationStore();

      to.params.username === authStore.user?.username
        ? (store.user = authStore.user)
        : store.getUserByUsername(to.params.username as string);
      next();
    },

    children: [
      {
        path: "settings",
        name: "profile-settings",
        component: () => import("@/components/UserDialog.vue"),
        meta: { requiresAuth: true },
        beforeEnter(
          to: RouteLocationNormalized,
          from: RouteLocationNormalized,
          next: NavigationGuardNext
        ) {
          const authStore = useAuthenticationStore();
          if (authStore.loggedIn && authStore.user?.role == Role.NUMBER_0) {
            // Zugriff für Admins und Moderatoren erlauben
            next();
          } else if (
            authStore.loggedIn &&
            to.params.username === authStore.user?.username
          ) {
            // Zugriff für Profilbesitzer erlauben
            next();
          } else {
            // Zugriff verweigern und auf eine andere Route umleiten
            next({ name: "profile", params: { username: to.params.username } }); // Ändern Sie "home" entsprechend der gewünschten Umleitungsroute
          }
        },
      },
    ],
  },

  {
    path: "/:username/post/:postId",
    name: "post",
    component: () => import("@/views/PostDetails.vue"),
    beforeEnter(
      to: RouteLocationNormalized,
      from: RouteLocationNormalized,
      next: NavigationGuardNext
    ) {
      //TO-DO: Add api
      const store = usePostStore();
      store.getPost(to.params.postId as string);
      next();
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
          store.user = {} as User;
        },
      },
      {
        name: "edit-user",
        path: "user/:id",
        component: () => import("@/components/UserDialog.vue"),
        beforeEnter(
          to: RouteLocationNormalized,
          from: RouteLocationNormalized,
          next: NavigationGuardNext
        ) {
          //TO-DO: Add api
          const store = useUsersStore();
          store.getUser(to.params.id as string);
          next();
        },
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem("user");
  if (to.meta.requiresAuth && !loggedIn) next({ name: "login" });
  else next();
});

function checkAccess(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  const authStore = useAuthenticationStore();

  if (
    authStore.loggedIn &&
    (authStore.user?.role == Role.NUMBER_0 ||
      authStore.user?.role == Role.NUMBER_1)
  ) {
    next();
  } else {
    next({ name: "home" });
  }
}

export default router;
