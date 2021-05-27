<template>
  <k-navbar name="Discovery">
    <k-navbar-item to="/" label="Dashboard" icon="home" />
    <k-navbar-item to="/about" label="About" icon="car"/>
  </k-navbar>
  <div id="content" class="p-3">
    <router-view />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { KNavbar, KNavbarItem } from './components';

export default defineComponent({
  name: 'App',
  components: { KNavbar, KNavbarItem }
})
</script>

<style>
  #content {
    margin-top: var(--k-content-top);
    margin-left: var(--k-content-left);
  }
</style>