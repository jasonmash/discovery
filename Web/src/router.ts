import { createWebHistory, createRouter } from "vue-router";
import Dashboard from "./pages/Dashboard.vue";
import About from "./pages/About.vue";
import Parts from "./pages/Parts.vue";
import DefectCategories from "./pages/DefectCategories.vue";
import Defects from "./pages/Defects.vue";
import Images from "./pages/Images.vue";

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
  {
    path: "/defect-categories",
    component: DefectCategories,
  },
  {
    path: "/defect-categories/:id",
    name: "defectcategories",
    component: DefectCategories,
  },
  {
    path: "/defects",
    component: Defects,
  },
  {
    path: "/defects/:id",
    name: "defects",
    component: Defects,
  },
  {
    path: "/images",
    component: Images,
  },
  {
    path: "/images/:id",
    name: "images",
    component: Images,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;