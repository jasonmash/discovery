<template>
  <li><h6 class="dropdown-header">{{label}}</h6></li>
</template>

<script lang="ts">
import { reactive, computed } from 'vue';

export default {
  name: 'k-dropdown-header',

  props: {
    label: {
      type: String,
      required: true,
    }
  }
};
</script>
