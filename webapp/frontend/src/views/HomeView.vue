<script setup lang="ts">
defineProps({
  weatherModule: {
    type: Object,
    required: true
  }
})
</script>
<template>
  <main>
    <component :is="weatherModule.container" />
  </main>
</template>
<style>
main {
  width: 100%;
}
</style>
