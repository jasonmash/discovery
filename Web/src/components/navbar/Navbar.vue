<template>
  <nav>
    <div class="navbar-bg d-sm-none" :class="isDevMode ? 'bg-dev': ''"> </div>

    <div class="navbar fixed-top d-sm-none">
      <a class="navbar-toggle" aria-label="Toggle navigation" @click="toggleMenu()" v-show="!canGoBack()" role="button">
        <i class="fas fa-bars"></i>
      </a>
      <a class="navbar-toggle" aria-label="Back" @click="goBack()" v-show="canGoBack()" role="button">
        <i class="fas fa-arrow-left"></i>
      </a>
      <div>
        {{name}}
        <small v-if="isDevMode">Dev</small>
      </div>
      <div class="navbar-right"></div>
    </div>

    <ul class="sidebar" v-bind:class="{ open: isMenuOpen }">
      <div class="d-none d-sm-block my-2 ml-3">
        <span @click="toggleMenu()">{{name}}</span>
        <span class="ml-2 sidebar-item-title">
          <small v-if="isDevMode">Dev</small>
        </span>
      </div>

      <div class="sidebar-items">
        <slot></slot>
      </div>

      <div class="sidebar-bottom">
      </div>
    </ul>
  </nav>
</template>

<script lang="ts">
import { reactive, ref, computed } from 'vue';

export default {
  name: 'k-navbar',

  props: {
    name: String,
    isDevMode: Boolean
  },

  setup(props, { emit }) {
    props = reactive(props);
    const isMenuOpen = ref(false);

    return {
      toggleMenu() {
        isMenuOpen.value = !isMenuOpen.value;
      },
      hideMenu() {
        isMenuOpen.value = false;
      },
      goBack() {

      },
      canGoBack(): boolean {
        return false;
      },
      isMenuOpen
    }
  },
};
</script>

<style src="./Navbar.css"></style>