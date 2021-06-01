import { createWebHistory, createRouter } from "vue-router";
import Dashboard from "./pages/Dashboard.vue";
import About from "./pages/About.vue";
import Parts from "./pages/Parts.vue";

const routes = [
  {
    path: "/",
    name: "dashboard",
    component: Dashboard,
  },
  {
    path: "/about",
    name: "about",
    component: About,
  },
  {
    path: "/parts",
    component: Parts,
  },
  {
    path: "/parts/:id",
    name: "parts",
    component: Parts,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;