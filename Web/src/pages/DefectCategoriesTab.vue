<template>
  <p v-if="!selectedDefectCategories">Loading...</p>
  <p class="text-secondary" v-else-if="selectedDefectCategories.length == 0">No categories of defect for this type of part found.</p>
  <div v-else class="row">
    <div class="col ps-0">
      <ul class="table ps-0 border-top mt-3">
        <li v-for="(defect, i) in selectedDefectCategories" class="border-bottom" :key="i">
          <router-link :to="`/defect-categories/${defect.id}`" class="pt-2">
            {{defect.title}}<br>
            <small class="d-block p-0 text-secondary" style="font-size:0.8em; margin-top:-2px;">{{values[i]}} defects recorded</small>
          </router-link>
        </li>
      </ul>
    </div>
    <div class="col">
      <canvas id="defects-chart" width="300" height="350"></canvas>
    </div>
  </div>
</template>

<script lang="ts">
import Chart from 'chart.js/auto';
import { ref, defineComponent } from 'vue'
var myChart: any = null;
export default defineComponent({
  name: 'DefectCategoriesTab',
  props: {
    selectedDefectCategories: Array
  },
  data:() => {
    return { values: [] } as any; 
  },
  setup: () => {
  },
  mounted() {
    console.log(this.$props);
    
    var getRandomNum = (min: number, max: number) => {
      min = Math.ceil(min);
      max = Math.floor(max);
      return Math.floor(Math.random() * (max - min) + min); //The maximum is exclusive and the minimum is inclusive
    }

    var ctx: any = document.getElementById('defects-chart');
    var labels = this.$props.selectedDefectCategories?.map((c: any) => c.title);
    this.$data.values = this.$props.selectedDefectCategories?.map((c: any) => getRandomNum(0, 1000)).sort((a: any, b: any) => b - a);
    if (myChart) { myChart.reset(); }
    myChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: '# of defects',
          backgroundColor: ['#c4e5ee', '#9cd4e3', '#74c3d7', '#4db2cc', '#3398b2', '#28768b', '#1c5563'].reverse(),
          data: this.$data.values,
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
})
</script>

