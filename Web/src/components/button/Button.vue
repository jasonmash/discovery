<template>
  <button :type="type" class="btn" :class="classes" :disabled="disabled ? true : null" @click="onClick"
    :aria-disabled="disabled ? true : null" :aria-pressed="pressed ? true : null">
    <slot>
      <i v-if="!!loading" class="fa fa-fw fa-loader fa-spin me-1"></i>
      <i v-else-if="!!icon" :class="`fa fa-fw fa-${icon} me-1`"></i>
      {{label}}
    </slot>
  </button>
</template>

<script lang="ts">
import { reactive, computed } from 'vue';

export default {
  name: 'k-button',

  props: {
    label: {
      type: String
    },
    type: {
      type: String,
      default: "button"
    },
    variant: {
      type: String,
      validator: (value: string) => {
        return ['primary', 'secondary', 'danger', 'warning', 'info', 'success', 'light', 'dark', 'link'].indexOf(value) !== -1;
      },
    },
    size: {
      type: String,
      validator: (value: string) => {
        return ['sm', 'md', 'lg'].indexOf(value) !== -1;
      },
    },
    disabled: {
      type: Boolean
    },
    pressed: {
      type: Boolean
    },
    loading: {
      type: Boolean
    },
    icon: {
      type: String
    }
  },

  emits: ['click'],

  setup(props, { emit }) {
    props = reactive(props);
    return {
      classes: computed(() => ({
        'btn': true,
        [`btn-${props.size || 'md'}`]: true,
        [`btn-${props.variant || 'secondary'}`]: true,
        'active': props.pressed,
        'disabled': props.disabled || props.loading,
        'ps-2': !!props.icon || props.loading
      })),
      onClick(event: any) {
        if (props.type == "submit") { event.preventDefault(); }
        if (!props.disabled) {
          emit('click');
        }
      }
    }
  },
};
</script>
