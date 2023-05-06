// Composables
import { useUsersStore } from "@/store/users";
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
    name: "user",
    component: () => import("@/views/User.vue"),
    children: [
      {
        name: "profile",
        path: "profile",
        component: () => import("@/views/Profile.vue"),
        beforeEnter(
          to: RouteLocationNormalized,
          from: RouteLocationNormalized,
          next: NavigationGuardNext
        ) {
          //TO-DO: Add api
          const store = useUsersStore();
          store.getUserByUsername(to.params.username as string);
          next();
        },
      },
      {
        path: "post/:postId",
        name: "post",
        component: () => import("@/views/PostDetails.vue"),
      },
    ],
  },

  {
    path: "/dashboard",
    name: "dashboard",
    component: () => import("@/views/Dashboard.vue"),
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
    children: [
      {
        name: "create-user",
        path: "create-user",
        component: () => import("@/components/UserDialog.vue"),
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
          store.getUser(Number(to.params.id));
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

/* router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem("");
  if (to.name !== "login" && !loggedIn) next({ name: "login" });
  else next();
}); */

export default router;
