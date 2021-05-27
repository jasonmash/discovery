<template>
  <router-link active-class="active" :to="to" :disabled="disabled ? true : null" role="button" class="sidebar-item">
    <i :class="`far fa-fw fa-${icon}`"></i><span class="sidebar-item-title">{{label}}</span>
  </router-link>
</template>

<script lang="ts">
import { reactive, computed } from 'vue';

export default {
  name: 'k-navbar-item',

  props: {
    label: {
      type: String,
      required: true,
    },
    disabled: {
      type: Boolean
    },
    icon: {
      type: String
    },
    to: {
      type: String
    }
  },

  emits: ['click'],

  setup(props, { emit }) {
    props = reactive(props);
    return {
      classes: computed(() => ({
        'disabled': props.disabled
      })),
      onClick() {
        emit('click');
      }
    }
  },
};
</script>
