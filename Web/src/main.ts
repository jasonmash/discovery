import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

import "@popperjs/core";
import "bootstrap/dist/js/bootstrap";

import "bootstrap/dist/css/bootstrap.css";
import "./styles/webfonts/font-awesome-6.css";
import "./styles/layout.css";

createApp(App).use(router).mount("#app");
