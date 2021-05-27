<template>
  <div :class="dropdownClasses">
    <button v-if="split" type="button" class="btn" :class="classes" @click="onClick">
      <slot name="button">
        <i v-if="!!icon" :class="`fa fa-fw fa-${icon} me-1`"></i>
        {{label}}
      </slot>
    </button>
    <button type="button" class="btn dropdown-toggle dropdown-toggle-split" :class="classes" :disabled="disabled ? true : null" :id="id"
      data-bs-toggle="dropdown" aria-expanded="false" :aria-disabled="disabled ? true : null" :aria-pressed="pressed ? true : null">
      <slot name="button" v-if="!split">
        <i v-if="!!icon" :class="`fa fa-fw fa-${icon} me-1`"></i>
        {{label}}
      </slot>
    </button>
    <ul class="dropdown-menu" :class="alignright ? 'dropdown-menu-end' : ''" :aria-labelledby="id">
      <slot></slot>
    </ul>
  </div>
</template>

<style>
  .dropdown-toggle::after,
  .dropup .dropdown-toggle::after,
  .dropend .dropdown-toggle::after,
  .dropstart .dropdown-toggle::after {
    margin-left: .5rem;
  }
  .dropdown-toggle-no-caret::after {
    display: none;
  }
</style>

<script lang="ts">
import { reactive, computed } from 'vue';

let dropdownUuid = 0;

export default {
  name: 'k-dropdown',

  props: {
    label: {
      type: String,
      required: true,
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
    split: {
      type: Boolean
    },
    disabled: {
      type: Boolean
    },
    pressed: {
      type: Boolean
    },
    dropup: {
      type: Boolean
    },
    dropend: {
      type: Boolean
    },
    dropstart: {
      type: Boolean
    },
    alignright: {
      type: Boolean
    },
    noCaret: {
      type: Boolean
    },
    icon: {
      type: String
    }
  },

  emits: ['click'],

  setup(props, { emit }) {
    const uuid = "_dd_" + dropdownUuid.toString();
    dropdownUuid += 1;
    props = reactive(props);
    return {
      classes: computed(() => ({
        'btn': true,
        [`btn-${props.size || 'md'}`]: true,
        [`btn-${props.variant || 'secondary'}`]: true,
        'active': props.pressed,
        'disabled': props.disabled || props.loading,
        'ps-2': !!props.icon || props.loading,
        'dropdown-toggle-no-caret': props.noCaret
      })),
      dropdownClasses: computed(() => ({
        'dropup': props.dropup,
        'dropend': props.dropend,
        'dropstart': props.dropstart,
        'btn-group': props.split,
        'dropdown': !props.split
      })),
      id: uuid,
      onClick(event) {
        if (props.disabled && isEvent(event)) {
          stopEvent(event)
        } else {
          emit('click');
        }
      }
    }
  },
};
</script>
