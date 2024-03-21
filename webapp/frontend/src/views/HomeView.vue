<script setup lang="ts">

const props = defineProps({
  weatherModule: {
    type: Object,
    required: true
  }
});

console.log({...props})

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