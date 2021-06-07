<template>
  <k-navbar name="Discovery" :logo="logoUrl" :logo-lg="logoLgUrl">
    <k-navbar-item to="/" label="Dashboard" icon="home" />
    <k-navbar-item to="/parts" label="Parts" icon="car-bus"/>
    <k-navbar-item to="/defect-categories" label="Defect Categories" icon="tags"/>
    <k-navbar-item to="/defects" label="Defects" icon="car-crash"/>
    <k-navbar-item to="/about" label="About" icon="circle-info"/>
  </k-navbar>
  <div id="content" class="p-3">
    <router-view />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { KNavbar, KNavbarItem } from './components';

import logoUrl from './assets/Logo.svg';
import logoLgUrl from './assets/LogoLG.svg';

export default defineComponent({
  name: 'App',
  components: { KNavbar, KNavbarItem },
  data() {
    return {
      logoUrl: logoUrl,
      logoLgUrl: logoLgUrl
    }
  }
})
</script>

<style>
  #content {
    margin-top: var(--k-content-top);
    margin-left: var(--k-content-left);
    background: var(--k-background);
    color: var(--k-color);
  }

  html, body {
    background: var(--k-background);
    color: var(--k-color);
  }
</style>