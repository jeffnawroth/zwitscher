// Composables
import { createRouter, createWebHistory } from "vue-router";
//Test
const routes = [{
  path: "/",
  name: "home",
  component: () => import('@/views/Home.vue'),
}];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router; 
