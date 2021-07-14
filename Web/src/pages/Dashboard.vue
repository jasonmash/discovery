<template>
  <div class="dashboard bg-light p-3">
    <div class="float-end p-2 pt-3">
      <button type="button" class="btn me-3 btn-secondary py-1 date-button">Today</button>
      <button title="Previous year" type="button" class="btn me-1 btn-secondary py-1 date-button">
        <i class="fas fa-fw fa-chevron-double-left"></i>
      </button>
      <button title="Previous month" type="button" class="btn me-1 btn-secondary py-1 date-button">
        <i class="fa fa-fw fa-chevron-left"></i>
      </button>
      <button type="button" class="btn me-1 btn-secondary py-1 date-button" style="min-width: 84px;">01 July 2021 - 07 July 2021</button>
      <button title="Next month" type="button" class="btn me-1 btn-secondary py-1 date-button">
        <i class="fa fa-fw fa-chevron-right"></i>
      </button>
      <button title="Next year" type="button" class="btn btn-secondary py-1 date-button">
        <i class="fas fa-fw fa-chevron-double-right"></i>
      </button> 
    </div>
    <h1>Dashboard</h1>
    <p class="text-secondary"><small>For 7 days between 1 July 2021 and 7 July 2021</small></p>

    <div class="row mt-4">
      <div class="col-2">
        <div class="card p-3 mb-3">
          <h6 class="stat-label">Parts Processed</h6>
          <h2 class="mb-0">4965</h2>
        </div>
        <div class="card p-3 mb-3">
          <h6 class="stat-label">Identified Defects</h6>
          <h2 class="mb-0">3182</h2>
        </div>
        <div class="card p-3 mb-3">
          <h6 class="stat-label">Avg Defect Count</h6>
          <h2 class="mb-0">0.2 <small class="stat-label">per part</small></h2>
        </div>
      </div>
      <div class="col-2">
        <div class="card p-3 mb-3">
          <h6 class="stat-label">Avg Pass Rate</h6>
          <h2 class="mb-0">82%</h2>
        </div>
        <div class="card p-3 mb-3">
          <h6 class="stat-label">Inspection Time</h6>
          <h2 class="mb-0">28<small class="stat-label"> hours</small></h2>
        </div>
        <div class="card p-3">
          <h6 class="stat-label">Operator Count</h6>
          <h2 class="mb-0">3<small class="stat-label"> operators</small></h2>
        </div>
      </div>
      <div class="col-4">
        <div class="card p-3">
          <h5 class="mb-0">Identified Defects</h5>
          <canvas id="parts-chart" width="300" height="250"></canvas>
        </div>
      </div>
      <div class="col-4">
        <div class="card p-3">
          <h5 class="mb-0">Inspection Pass Rate</h5>
          <canvas id="pass-chart" width="300" height="250"></canvas>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Chart from 'chart.js/auto';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Dashboard',
  data() {
    return { forecasts: [] };
  },
  created() {
    fetch('weatherforecast')
      .then(res => res.json())
      .then(forecasts => { this.forecasts = forecasts; });
  },
  mounted() {
    var ctx: any = document.getElementById('parts-chart');
    var myChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        datasets: [{
          label: '# of defects',
          data: [120, 192, 83, 52, 89, 97, 20],
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
    var ctx1: any = document.getElementById('pass-chart');
    var myChart1 = new Chart(ctx1, {
      type: 'line',
      data: {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        datasets: [{
          label: '% of processed parts with no defects',
          data: [80, 65, 81, 79, 80, 90, 85],
          borderColor: 'rgba(32, 201, 91, 1)',
          backgroundColor: 'rgba(32, 201, 91, 0.3)',
          borderWidth: 1,
          fill: true,
          tension: 0.3
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    });
  }
})
</script>

<style scoped>
.dashboard {
  display: block;
  position: absolute;
  right: 0;
  bottom: 0;
  top: 0;
  left: var(--k-navbar-width);
}
.stat-label {
  font-size: 0.8rem;
  color: #555;
}

.date-button {
  background: #ddd;
  border-color: #ddd;
  color: #000;
  font-size: 0.8rem;
}
</style>