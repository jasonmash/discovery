<template>
  <h1>Dashboard</h1>
  <code style="text-align: left;">
    <pre>{{ JSON.stringify(forecasts, null, 2) }}</pre>
  </code>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Dashboard',
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

