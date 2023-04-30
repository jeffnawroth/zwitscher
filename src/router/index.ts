// Composables
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/",
    name: "landing-page",
    component: () => import("@/views/LandingPage.vue"),
  },
  {
    path: "/home",
    name: "home",
    component: () => import("@/views/Home.vue"),
    //meta: { requiresAuth: true },
  },
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
  {
    path: "/users",
    name: "users",
    component: () => import("@/views/UserManagement.vue"),
    children: [
      {
        name: "create-user",
        path: "create-user",
        props: true,
        component: () =>
          import(
            /* weppackChunkName: "create-user" */ "@/components/UserDialog.vue"
          ),
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
