<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h1>Parts</h1>
      <ul>
        <li v-for="part in parts">
          <router-link :to="`/parts/${part.id}`">{{part.title}}</router-link>
        </li>
      </ul>
      <k-form title="Add Part" :visible="showForm" @submit="addPart" @close="showForm = false">
        <k-input label="Title" v-model="newPart.title"></k-input>
        <k-input label="Description" v-model="newPart.description"></k-input>
      </k-form>
    </template>
    <template v-slot:detail>
      <div v-if="selectedPart">
        <h1>{{selectedPart.title}} <small>#{{selectedPart.id}}</small></h1>
        <p>{{selectedPart.description}}</p>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { Part } from '../models/Part';

interface PartsData {
  parts: Part[],
  selectedPart?: Part
}

export default defineComponent({
  name: 'Parts',
  components: { KListDetails, KButton, KForm, KInput },
  setup() {
    const route = useRoute();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const parts = ref([] as Part[]);
    const selectedPart = ref();
    const showForm = ref(false);
    const newPart = ref({ title: "", description: ""});

    const loadData = () => {
      fetch('/api/parts')
        .then(res => res.json())
        .then(res => {
          parts.value = res;
          if (!!selectedId.value) {
            selectedPart.value = parts.value.find((p: Part) => p.id.toString() == selectedId.value);
          }
        });
    };
    loadData();

    // fetch the user information when params change
    watch(() => route.params.id,
      async newId => {
        showForm.value = false;
        selectedId.value = newId;
        showDetails.value = !!selectedId.value;
        selectedPart.value = parts.value.find(p => p.id.toString() == selectedId.value);
      }
    );

    const addPart = () => {
      console.log(newPart.value);
      fetch('/api/parts', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newPart.value)
      }).then(() => {
        newPart.value = { title: "", description: "" };
      }).catch(() => {

      }).finally(() => {
        showForm.value = false;
        loadData();
      });
    };

    return {
      selectedId,
      showDetails,
      parts,
      selectedPart,
      showForm,
      addPart,
      newPart
    }
  }
})
</script>

