<template>
  <input :class="classes" :type="type" :value="modelValue" 
    @input="onInput" @focus="$emit('focus')" @blur="$emit('blur')" @change="$emit('change', $event)"
    :placeholder="placeholder ? placeholder : label" :aria-label="label"
    :readonly="readonly ? true : null" :autocomplete="autocomplete"
    :required="required ? true : null" :aria-required="required ? true : null"
    :autofocus="autofocus ? true : null" :aria-invalid="state === false"
    :disabled="disabled ? true : null" :aria-disabled="disabled ? true : null" />
</template>

<script lang="ts">
import { reactive, computed } from 'vue';

export default {
  name: 'k-input',
  
  emits: ['update:modelValue', 'focus', 'blur'],

  props: {
    modelValue: String,
    type: {
      type: String,
      default: 'text',
      validator: (value: string) => {
        return ['text', 'password', 'email', 'url', 'tel', 'search'].indexOf(value) !== -1
      }
    },
    label: {
      type: String
    },
    placeholder: {
      type: String
    },
    size: {
      type: String,
      validator: (value: string) => {
        return ['sm', 'md', 'lg'].indexOf(value) !== -1
      }
    },
    state: {
      type: Boolean
    },
    disabled: {
      type: Boolean
    },
    readonly: {
      type: Boolean
    },
    plaintext: {
      type: Boolean
    },
    autocomplete: {
      type: Boolean,
      default: null
    },
    autofocus: {
      type: Boolean,
      default: null
    },
    required: {
      type: Boolean,
      default: null
    }
  },

  setup(props, { emit }) {
    props = reactive(props);
    return {
      classes: computed(() => ({
        'form-control': true,
        [`form-control-${props.size || 'md'}`]: true,
        'disabled': props.disabled,
        'form-control-plaintext': props.plaintext,
        'is-invalid': props.state === false,
        'is-valid': props.state === true
      })),
      onInput(event: any) {
        emit('update:modelValue', event.target.value)
      }
    }
  },
};
</script>
