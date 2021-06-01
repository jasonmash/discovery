<template>
  <div class="list-details" v-bind:class="{ 'show-detail': showDetails, 'full-width': isFullWidth && showDetails }">
    <div class="list-panel p-3">
      <slot name="list"></slot>
    </div>
    <div class="details-panel p-3" v-if="showDetails">
      <div class="divider" v-if="showDetails">
        <button class="btn btn-secondary divider-toggle" @click="isFullWidth = !isFullWidth">
          <i :class="`fa fa-chevron-${isFullWidth ? 'right' : 'left' }`"></i>
        </button>
      </div>
      <slot name="detail"></slot>
    </div>
  </div>
</template>

<script lang="ts">
import { reactive, ref } from 'vue';

export default {
  name: 'k-list-details',

  props: {
    showDetails: Boolean
  },

  setup(props, { emit }) {
    props = reactive(props);
    const isFullWidth = ref(false);

    return {
      isFullWidth
    }
  },
};
</script>

<style src="./ListDetails.css"></style>