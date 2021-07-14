<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <k-button class="float-end" @click="showForm = !showForm"><i class="fa fa-fw fa-plus"></i></k-button>
      <h2>Defect Categories</h2>
      <input class="form-control form-control-sm mb-2" type="text" placeholder="Search..." />
      <ul class="table">
        <li v-for="category in categories">
          <router-link :to="`/defect-categories/${category.id}`">
            <small class="p-0">{{partForId(category.partId) ? partForId(category.partId).title : ''}}</small><br>
            {{category.title}}
          </router-link>
        </li>
      </ul>
      <k-form title="Add Defect Category" :visible="showForm" @submit="addCategory" @close="showForm = false">
        <k-input label="Title" v-model="newCategory.title"></k-input>
        <k-input label="Description" v-model="newCategory.description"></k-input>
      </k-form>
    </template>
    <template v-slot:detail>
      <div v-if="selectedCategory">
        <k-button class="float-end" variant="danger" @click="deleteCategory">Delete</k-button>
        <h5><router-link style="text-decoration: none; color: #444;" :to="`/parts/${selectedCategory.partId}`">{{partForId(selectedCategory.partId) ? partForId(selectedCategory.partId).title : ''}}</router-link></h5>
        <h1>{{selectedCategory.title}}</h1>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 0 ? 'active' : ''" @click="tabId = 0" aria-current="page" href="#">Overview</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 1 ? 'active' : ''" @click="tabId = 1" href="#">Images</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" :class="tabId == 2 ? 'active' : ''" @click="tabId = 2" href="#">Activities</a>
          </li>
        </ul>

        <div class="tab-content bg-white border" id="nav-tabContent">
          <div class="tab-pane fade p-3 pt-2" :class="tabId == 0 ? 'show active' : ''" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
            <div class="row">
              <div class="col">
                <h6 class="activity-section-heading">Title</h6>
                <p class="mb-4">{{selectedCategory.title}} (<small>#{{selectedCategory.id}}</small>)</p>
                
                <h6 v-if="selectedCategory.description" class="activity-section-heading">Description</h6>
                <p class="mb-4">{{selectedCategory.description}}</p>
                
                <h6 class="activity-section-heading">Frequency of occurance</h6>
                <p class="mb-4">12% of inspected parts</p>
              </div>
              <div class="col">
                <h6 class="activity-section-heading mt-3">Identified Defects (this week)</h6>
                <canvas v-if="tabId == 0" id="parts-chart" width="300" height="250"></canvas>
              </div>
            </div>
          </div>
          <div class="tab-pane fade p-3 pt-3" :class="tabId == 1 ? 'show active' : ''" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
            <h4>Images</h4>
            <div class="row">
              <div class="col-4">
                <div class="card">
                  <img src="../assets/2.jpeg"/>
                </div>
              </div>
              <div class="col-4">
                <div class="card">
                  <img src="../assets/3.jpeg"/>
                </div>
              </div>
              <div class="col-4">
                <div class="card">
                  <img src="../assets/4.jpeg"/>
                </div>
              </div>
            </div>
          </div> 
          <div class="tab-pane fade p-3 pt-2" :class="tabId == 2 ? 'show active' : ''" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
            <div class="activity-list">
              <div class="activity-section-heading">08 JUL 2021</div>
              <div class="activity activity-clickable">
                <div class="activity-row activity-type-2">
                  <i class="activity-icon fa-fw fas fa-car-crash"></i>
                  <div class="activity-time">10:29</div>
                  <div class="activity-content">
                    Operator logged defect "{{selectedCategory.title}}" against inspected part
                  </div>
                </div>
              </div><div class="activity activity-clickable">
                <div class="activity-row activity-type-2">
                  <i class="activity-icon fa-fw fas fa-car-crash"></i>
                  <div class="activity-time">10:26</div>
                  <div class="activity-content">
                    Operator logged defect "{{selectedCategory.title}}" against inspected part
                  </div>
                </div>
              </div><div class="activity activity-clickable">
                <div class="activity-row activity-type-2">
                  <i class="activity-icon fa-fw fas fa-car-crash"></i>
                  <div class="activity-time">10:25</div>
                  <div class="activity-content">
                    Operator logged defect "{{selectedCategory.title}}" against inspected part
                  </div>
                </div>
              </div>
            </div>
          </div> 
        </div>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import Chart from 'chart.js/auto';
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { Part } from '../models/Part';
import { DefectCategory } from '../models/DefectCategory';


var myChart: any;

export default defineComponent({
  name: 'DefectCategories',
  components: { KListDetails, KButton, KForm, KInput },
  setup() {
    const route = useRoute();
    const router = useRouter();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const categories = ref([] as DefectCategory[]);
    const selectedCategory = ref();
    const showForm = ref(false);
    const newCategory = ref({ title: "", description: ""});

    const parts = ref([] as Part[]);
    const tabId = ref(0);

    const loadChart = () => {
      var ctx: any = document.getElementById('parts-chart');
      if (!ctx) { return; }
      if (myChart) { myChart.reset(); }
      myChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        datasets: [{
          label: '# of defects',
          data: [5, 10, 18, 15, 8, 10, 2],
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.3)',
          borderWidth: 1,
          fill: true,
          tension: 0.3
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
    }

    const loadData = () => {
      fetch('/api/defectcategories')
        .then(res => res.json())
        .then(res => {
          categories.value = res;
          if (!!selectedId.value) {
            selectedCategory.value = categories.value.find((p: Part) => p.id.toString() == selectedId.value);
          }
          loadChart();
        });
      fetch('/api/parts')
        .then(res => res.json())
        .then(res => {
          parts.value = res;
        });

      setTimeout(loadChart, 300);
    };
    loadData();
    

    // fetch the user information when params change
    watch(() => route.params.id,
      async newId => {
        showForm.value = false;
        selectedId.value = newId;
        showDetails.value = !!selectedId.value;
        selectedCategory.value = categories.value.find(p => p.id.toString() == selectedId.value);
        loadChart();
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

    var deleteCategory = () => {
      fetch('/api/defectcategories/' + selectedId.value, {
        method: 'DELETE',
      })
      .then(res => {
        if (res.ok) {
          categories.value.splice(categories.value.findIndex(i => i.id.toString() == selectedId.value), 1);
          router.replace("/defect-categories");
        }
      });
    };

    var partForId = (id: any) => {
      return parts.value.find((p) => p.id == id);
    }

    return {
      selectedId,
      showDetails,
      categories,
      selectedCategory,
      showForm,
      addCategory,
      newCategory,
      deleteCategory,
      partForId,
      tabId
    }
  }
})
</script>

