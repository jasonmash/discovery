<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h2 class="mb-2">Parts</h2>
      <input class="form-control form-control-sm mb-2" v-model="search" @input="searchChange" type="text" placeholder="Search..." />
      <ul class="table">
        <li v-for="(part, i) in parts" :key="i">
          <router-link :to="`/parts/${part.id}`">
            <small class="p-0">{{part.partRef}}</small><br>
            {{part.title}}
          </router-link>
        </li>
      </ul>
      <k-form title="Add Part" :visible="showForm" @submit="addPart" @close="showForm = false">
        <k-input label="Title" v-model="newPart.title"></k-input>
        <k-input label="Description" v-model="newPart.description"></k-input>
        <k-input label="Part Ref" v-model="newPart.partRef"></k-input>
        <input @change="handleImage" class="custom-input" type="file" accept="image/*">
      </k-form>
    </template>
    <template v-slot:detail>
      <div v-if="selectedPart" >
        <k-button class="float-end" variant="danger" @click="deletePart">Delete</k-button>
        <h1>{{selectedPart.title}}</h1>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 0 ? 'active' : ''" @click="tabId = 0" aria-current="page" href="#">Overview</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 1 ? 'active' : ''" @click="tabId = 1" href="#">Activities</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 2 ? 'active' : ''" @click="tabId = 2" href="#">Defects</a>
          </li>
        </ul>

        <div class="tab-content bg-white border" id="nav-tabContent">
          <div class="tab-pane fade p-3 pt-2" :class="tabId == 0 ? 'show active' : ''" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
            <div class="row mb-4">
              <div class="col">
                <h6 class="activity-section-heading">Part Reference</h6>
                <p class="mb-4">{{selectedPart.partRef}} (<small>#{{selectedPart.id}}</small>)</p>

                <h6 class="activity-section-heading">Description</h6>
                <p class="mb-4">{{selectedPart.description}}</p>

                <h6 class="activity-section-heading">Defect Categories</h6>
                <p class="mb-4">{{selectedDefectCategories.length}} categories</p>

                <h6 class="activity-section-heading">Inspection Pass Rate</h6>
                <p class="mb-4 text-success">{{selectedPartPercent}}%</p>
              </div>
              <div class="col text-end">
                <img v-if="selectedPart.image" style="width: 300px" :src="selectedPart.image" alt=""/>
                <p v-else>No image uploaded</p>
              </div>
            </div>
          </div>
          <div class="tab-pane fade p-3 pt-2" :class="tabId == 1 ? 'show active' : ''" id="nav-profile" role="tabpanel" aria-labelledby="nav-profile-tab">

            <div class="activity-list" v-for="activity in selectedActivities">
              <div class="activity-section-heading">{{activity.date}}</div>
              <div class="activity activity-clickable" v-for="item in activity.items">
                <div class="activity-row activity-type-2">
                  <i class="activity-icon fa-fw fas fa-car-crash"></i>
                  <div class="activity-time">{{item.time}}</div>
                  <div class="activity-content">
                    Operator logged defect "<router-link to="/defects">{{item.defect}}</router-link>" against inspected part (ref {{item.ref}})
                  </div>
                </div>
              </div>
            </div>
            
          </div>
          <div class="tab-pane fade p-3" :class="tabId == 2 ? 'show active' : ''" id="nav-contact" role="tabpanel" aria-labelledby="nav-contact-tab">
            <k-button class="float-end mt-n3" variant="success" @click="showCategoryForm = !showCategoryForm"><i class="fa fa-fw fa-plus"></i></k-button>
            <h4>Defects</h4>
            <defect-categories-tab v-if="tabId == 2" :selectedDefectCategories="selectedDefectCategories"/>
          
          </div>
        </div>

        
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
import { useRoute, useRouter } from 'vue-router';
import { DefectCategory } from '../models/DefectCategory';
import { Part } from '../models/Part';
import DefectCategoriesTab from "./DefectCategoriesTab.vue";


export default defineComponent({
  name: 'Parts',
  components: { KListDetails, KButton, KForm, KInput, DefectCategoriesTab },
  setup() {
    const route = useRoute();
    const router = useRouter();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const allParts = ref([] as Part[]);
    const parts = ref([] as Part[]);
    const selectedDefectCategories = ref([] as DefectCategory[]);
    const selectedPart = ref();
    const showForm = ref(false);
    const tabId = ref(0);
    const selectedPartPercent = ref(0);
    const search = ref("");
    const showCategoryForm = ref(false);
    const newPart = ref({ title: "", description: "", partRef: "", image: "" });
    const newCategory = ref({ title: "", description: "", partId: "" });
    const selectedActivities = ref([] as any[]);

    var generateActivities = () => {
      var activities: any = [];
      var numberOfDays = getRandomPercent(2, 3);

      var day = getRandomPercent(2, 15);
      for (var i = numberOfDays; i > 0; i--) {
        var item: any = {
          date: `${day + i} JUN 2021`,
          items: []
        };
      
        var numberOfItems = getRandomPercent(2, 10);
      
        var time = getRandomPercent(25, 59);
        var hour = getRandomPercent(10, 22);

        var x = time;
        for (var j = numberOfItems; j > 0; j--) {
          x -= getRandomPercent(1, 4);
          var f = getRandomPercent(1, 3);
          var ind = getRandomPercent(0, selectedDefectCategories.value.length);
          var activity = {
            time: `${hour + Math.floor(j / 12)}:${x}`,
            ref: `PART-00${i}${Math.floor(j / f)}`,
            defect: selectedDefectCategories.value[ind] ? selectedDefectCategories.value[ind].title : ''
          };

          item.items.push(activity);
        }

        activities.push(item);
      }

      return activities;
    };

    var getRandomPercent = (min: number, max: number) => {
      min = Math.ceil(min);
      max = Math.floor(max);
      return Math.floor(Math.random() * (max - min) + min); //The maximum is exclusive and the minimum is inclusive
    }

    const loadData = () => {
      selectedDefectCategories.value = [];
      selectedPartPercent.value = getRandomPercent(60, 95);
      fetch('/api/parts')
        .then(res => res.json())
        .then(res => {
          parts.value = res;
          allParts.value = res;
          if (!!selectedId.value) {
            selectedPart.value = parts.value.find((p: Part) => p.id.toString() == selectedId.value);
          }
        });
      if (!!selectedId.value) {
        fetch('/api/parts/' + selectedId.value.toString())
          .then(res => res.json())
          .then(res => {
            selectedPart.value = res;
            selectedDefectCategories.value = res.defectCategories;
            selectedActivities.value = generateActivities();
          });
      }
    };
    loadData();

    const searchChange = (e: any) => {
      if (!e.data) { parts.value = allParts.value; }
      else {
        parts.value = allParts.value.filter((p) => p.title.toLowerCase().includes(search.value.toLowerCase()));
      }
    };

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
        newPart.value = { title: "", description: "", partRef: "", image: "" };
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

    var deletePart = () => {
      var d = confirm("Are you sure you wish to delete this part?");
      if (!d) { return; }
      fetch('/api/parts/' + selectedId.value, {
        method: 'DELETE',
      })
      .then(res => {
        if (res.ok) {
          parts.value.splice(parts.value.findIndex(i => i.id.toString() == selectedId.value), 1);
          router.replace("/parts");
        }
      });
    };

    var createBase64Image = (fileObject: any) => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        newPart.value.image = e.target.result;
      };
      reader.readAsDataURL(fileObject);
    };

    var handleImage = (e: any) => {
      const selectedImage = e.target.files[0]; // get first file
      createBase64Image(selectedImage);
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
      showCategoryForm,
      deletePart,
      search,
      searchChange,
      handleImage,
      tabId,
      selectedDefectCategories,
      selectedPartPercent,
      selectedActivities
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
    height: 58px;
    padding: 4px .8rem 0;
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

  #nav-tabContent {
    border-top: none !important;
  }

</style>