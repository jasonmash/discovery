<template>
  <form v-if="visible" class="form-panel p-3" novalidate @submit.prevent="submitForm"> 
    <k-button @click="closeForm" class="float-end" title="Close"><i class="fa fa-fw fa-close"></i></k-button>
    <h2>{{title}}</h2>
    <slot></slot>
    <k-button @click="submitForm" type="submit" variant="success" class="me-2">OK</k-button>
    <k-button @click="closeForm" variant="secondary">Cancel</k-button>
  </form>
</template>

<script lang="ts">
import { reactive, ref } from 'vue';
import { KButton } from "../index";

export default {
  name: 'k-form',
  components: { KButton },

  props: {
    title: String,
    visible: Boolean,
    showDetails: Boolean
  },

  setup(props, { emit }) {
    props = reactive(props);
    const isFullWidth = ref(false);

    const closeForm = () => {
      emit("close");
    };

    const submitForm = (evt: any) => {
      if (evt) {
        evt.preventDefault();
      }
      emit("submit", evt);
    };

    return {
      isFullWidth,
      closeForm,
      submitForm
    }
  },
};
</script>

<style src="./Form.css"></style>