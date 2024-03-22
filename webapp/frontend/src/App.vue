<script setup lang="ts">
import { RouterView } from 'vue-router'

import './assets/topography.svg'
</script>

<template>
  <header class="hero-section">
    <h1 class="hero-title">Weather app</h1>
    <h2 class="hero-subtitle">We've got better weather!</h2>
  </header>

  <RouterView />
</template>
<style scoped>
.hero-section {
  background-image: linear-gradient(to bottom, transparent,#ffffff 100%), url("@/assets/topography.svg");
  padding: 50px 20px;
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  align-items: center;
  height: 400px; 
  color: #334155;
}

.hero-title {
  font-size: 2rem;
  margin: 0;
  font-weight: bold;
}

.hero-subtitle {
  font-size: 1.5rem;
  margin-top: 10px;
}

</style>
