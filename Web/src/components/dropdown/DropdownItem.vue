<template>
  <li>
    <router-link v-if="!!to" class="dropdown-item" :class="classes" active-class="active" :to="to" :disabled="disabled ? true : null">
      <slot>
        <i v-if="!!icon" :class="`fa-light fa-fw fa-${icon} me-1`"></i>
        {{label}}
      </slot>
    </router-link>
    <a v-else class="dropdown-item" :class="classes" :href="href" @click="onClick" :aria-disabled="disabled ? true : null" >
      <slot>
        <i v-if="!!icon" :class="`fa-light fa-fw fa-${icon} me-1`"></i>
        {{label}}
      </slot>
    </a>
  </li>
</template>

<script lang="ts">
import { reactive, computed } from 'vue';

export default {
  name: 'k-dropdown-item',

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
    href: {
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
        'active': props.pressed,
        'disabled': props.disabled
      })),
      onClick() {
        emit('click');
      }
    }
  },
};
</script>
