<template>
  <div class="mb-3">
    <label v-if="label">{{label}}</label>
    <input :class="classes" :type="type" v-model="input" 
      @focus="$emit('focus')" @blur="$emit('blur')" @change="$emit('change', $event)"
      :placeholder="placeholder ? placeholder : label" :aria-label="label"
      :readonly="readonly ? true : undefined" :autocomplete="autocomplete ? 'true' : 'false'"
      :required="required ? true : undefined" :aria-required="required ? true : undefined"
      :autofocus="autofocus ? true : undefined" :aria-invalid="state === false"
      :disabled="disabled ? true : undefined" :aria-disabled="disabled ? true : undefined" />
  </div>
</template>

<script lang="ts">
import { reactive, computed } from 'vue';
import useInputValidator from "./input-validator";
import { minLength } from "./validators";

export default {
  name: 'k-input',
  // 
  emits: ['update:modelValue', 'change', 'focus', 'blur'],

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

    const { input, errors } = useInputValidator(
      props.modelValue,
      [minLength(3)],
      (value: string) => {
        emit('update:modelValue', value);
      }
    );
    return {
      classes: computed(() => ({
        'form-control': true,
        [`form-control-${props.size || 'md'}`]: true,
        'disabled': props.disabled,
        'form-control-plaintext': props.plaintext,
        'is-invalid': !!errors.value && errors.value.find(v => v !== null), //.state === false,
        'is-valid': props.state === true
      })),
      input,
      errors
    }
  },
};
</script>
