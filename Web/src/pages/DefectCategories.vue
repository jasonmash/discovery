<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h2>Defect Categories</h2>
      <ul class="table">
        <li v-for="category in categories">
          <router-link :to="`/defect-categories/${category.id}`">{{category.title}}</router-link>
        </li>
      </ul>
      <k-form title="Add Defect Category" :visible="showForm" @submit="addCategory" @close="showForm = false">
        <k-input label="Title" v-model="newCategory.title"></k-input>
        <k-input label="Description" v-model="newCategory.description"></k-input>
      </k-form>
    </template>
    <template v-slot:detail>
      <div v-if="selectedCategory">
        <h1>{{selectedCategory.title}} <small>#{{selectedCategory.id}}</small></h1>
        <p>{{selectedCategory.description}}</p>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { Part } from '../models/Part';
import { DefectCategory } from '../models/DefectCategory';

export default defineComponent({
  name: 'DefectCategories',
  components: { KListDetails, KButton, KForm, KInput },
  setup() {
    const route = useRoute();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const categories = ref([] as DefectCategory[]);
    const selectedCategory = ref();
    const showForm = ref(false);
    const newCategory = ref({ title: "", description: ""});

    const loadData = () => {
      fetch('/api/defectcategories')
        .then(res => res.json())
        .then(res => {
          categories.value = res;
          if (!!selectedId.value) {
            selectedCategory.value = categories.value.find((p: Part) => p.id.toString() == selectedId.value);
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
        selectedCategory.value = categories.value.find(p => p.id.toString() == selectedId.value);
      }
    );

    const addCategory = () => {
      console.log(newCategory.value);
      fetch('/api/defectcategories', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newCategory.value)
      }).then(() => {
        newCategory.value = { title: "", description: "" };
      }).catch(() => {

      }).finally(() => {
        showForm.value = false;
        loadData();
      });
    };

    return {
      selectedId,
      showDetails,
      categories,
      selectedCategory,
      showForm,
      addCategory,
      newCategory
    }
  }
})
</script>

