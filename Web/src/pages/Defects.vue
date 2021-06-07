<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h2>Defects</h2>
      <ul class="table">
        <li v-for="defect in defects">
          <router-link :to="`/defects/${defect.id}`">{{defect.title}}</router-link>
        </li>
      </ul>
      <k-form title="Add Part" :visible="showForm" @submit="addDefect" @close="showForm = false">
        <k-input label="Title" v-model="newDefect.title"></k-input>
        <k-input label="Description" v-model="newDefect.description"></k-input>
      </k-form>
    </template>
    <template v-slot:detail>
      <div v-if="selectedDefect">
        <h1>{{selectedDefect.title}} <small>#{{selectedDefect.id}}</small></h1>
        <p>{{selectedDefect.description}}</p>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { Defect } from '../models/Defect';
import { Part } from '../models/Part';

interface DefectsData {
  defects: Part[],
  selectedDefect?: Part
}

export default defineComponent({
  name: 'Defects',
  components: { KListDetails, KButton, KForm, KInput },
  setup() {
    const route = useRoute();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const defects = ref([] as Defect[]);
    const selectedDefect = ref();
    const showForm = ref(false);
    const newDefect = ref({ title: "", description: ""});

    const loadData = () => {
      fetch('/api/defects')
        .then(res => res.json())
        .then(res => {
          defects.value = res;
          if (!!selectedId.value) {
            selectedDefect.value = defects.value.find((p: Defect) => p.id.toString() == selectedId.value);
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
        selectedDefect.value = defects.value.find(p => p.id.toString() == selectedId.value);
      }
    );

    const addDefect = () => {
      console.log(newDefect.value);
      fetch('/api/defects', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newDefect.value)
      }).then(() => {
        newDefect.value = { title: "", description: "" };
      }).catch(() => {

      }).finally(() => {
        showForm.value = false;
        loadData();
      });
    };

    return {
      selectedId,
      showDetails,
      defects,
      selectedDefect,
      showForm,
      addDefect,
      newDefect
    }
  }
})
</script>

