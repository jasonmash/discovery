<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <h1>Parts</h1>
      <ul>
        <li v-for="part in parts">
          <router-link :to="`/parts/${part.id}`">{{part.title}}</router-link>
        </li>
      </ul>
    </template>
    <template v-slot:detail>
      <h1>Selected Part: {{selectedId}}</h1>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute } from 'vue-router';

export default defineComponent({
  name: 'Parts',
  components: { KListDetails },
  data() {
    return { parts: [] };
  },
  created() {
    fetch('/api/parts')
      .then(res => res.json())
      .then(parts => { this.parts = parts; });
  },
  setup() {
    const route = useRoute();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);

    // fetch the user information when params change
    watch(() => route.params.id,
      async newId => {
        selectedId.value = newId;
        showDetails.value = !!selectedId.value;
      }
    );

    return {
      selectedId,
      showDetails
    }
  }
})
</script>

