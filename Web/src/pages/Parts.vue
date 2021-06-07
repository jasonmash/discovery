<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h2>Parts</h2>
      <ul class="table">
        <li v-for="(part, i) in parts" :key="i">
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

        <k-button class="float-end" variant="success" @click="showCategoryForm = !showCategoryForm"><i class="fa fa-fw fa-plus"></i></k-button>
        <h4 class="mt-4">Defect Categories</h4>
        <p v-if="!selectedPart.defectCategories">Loading...</p>
        <p class="text-secondary" v-else-if="selectedPart.defectCategories.length == 0">No categories of defect for this type of part found.</p>
        <div v-else class="w-100 mb-4">
          <div v-for="defect in selectedPart.defectCategories" :key="defect.id" class="w-50 d-inline-block">
            <router-link class="card m-2 p-2" :to="`/defect-categories/${defect.id}`">
              <h6 class="mb-0">{{defect.title}}</h6>
              <p class="p-0 m-0"><small>{{defect.description}}</small></p>
            </router-link>
          </div>
        </div>

        <h4 class="mt-4">Logged Defects</h4>
        <p class="text-secondary">No defects have been logged yet for this type of part.</p>

        <k-form title="Add Category" :visible="showCategoryForm" @submit="addCategory" @close="showCategoryForm = false">
          <k-input label="Title" v-model="newCategory.title"></k-input>
          <k-input label="Description" v-model="newCategory.description"></k-input>
        </k-form>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { DefectCategory } from '../models/DefectCategory';
import { Part } from '../models/Part';


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
    const showCategoryForm = ref(false);
    const newPart = ref({ title: "", description: "" });
    const newCategory = ref({ title: "", description: "", partId: "" });

    const loadData = () => {
      fetch('/api/parts')
        .then(res => res.json())
        .then(res => {
          parts.value = res;
          if (!!selectedId.value) {
            selectedPart.value = parts.value.find((p: Part) => p.id.toString() == selectedId.value);
          }
        });
      if (!!selectedId.value) {
        fetch('/api/parts/' + selectedId.value.toString())
          .then(res => res.json())
          .then(res => {
            selectedPart.value = res;
          });
      }
    };
    loadData();

    // fetch the user information when params change
    watch(() => route.params.id,
      async newId => {
        showForm.value = false;
        selectedId.value = newId;
        showDetails.value = !!selectedId.value;
        selectedPart.value = parts.value.find(p => p.id.toString() == selectedId.value);
        loadData();
      }
    );

    const addPart = () => {
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

    const addCategory = () => {
      newCategory.value.partId = selectedId.value.toString();
      console.log(newCategory.value);
      fetch('/api/defectcategories', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newCategory.value)
      }).then(() => {
        newCategory.value = { title: "", description: "", partId: selectedId.value.toString() };
      }).catch(() => {

      }).finally(() => {
        showCategoryForm.value = false;
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
      newPart,
      addCategory,
      newCategory,
      showCategoryForm
    }
  }
})
</script>

<style>
  a {
    color: #0c677e;
  }
  a:hover {
    color: #0a4d5e;
  }

  ul.table {
    list-style: none;
    padding: 0;
  }


  ul.table > li {
    list-style: none;
    text-decoration: none;
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
  }

  ul.table > li > a {
    display: block;
    text-decoration: none;
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
    height: 34px;
    line-height: 34px;
    padding: 0 .5rem;
    border-radius: .4rem;
    width: auto;
    background: transparent;
    border: none;
    color: black;
  }

  ul.table > li > a:hover {
    background: rgba(0,0,0,.04);
  }

  ul.table > li > a:active {
    background: rgba(0,0,0,.08);
    transition: none;
  }

  ul.table > li > a.router-link-active {
    background: rgb(219 235 239 / 65%);
    color: #0c677e;
  }

</style>