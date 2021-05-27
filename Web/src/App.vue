<template>
  <HelloWorld msg="Hello Vue 3 + TypeScript + Vite!" />
  <code style="text-align: left;">
    <pre>{{ JSON.stringify(forecasts, null, 2) }}</pre>
  </code>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HelloWorld from './components/HelloWorld.vue';

export default defineComponent({
  name: 'App',
  components: {
    HelloWorld
  },
  data() {
    return { forecasts: [] };
  },
  created() {
    fetch('weatherforecast')
      .then(res => res.json())
      .then(forecasts => { this.forecasts = forecasts; });
  }
})
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grays cale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
