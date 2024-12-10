<template>
  <div class="user-page">
    <TheSidebar></TheSidebar>
    <RouterView />
  </div>
</template>

<script setup>
import TheSidebar from '@/components/TheSidebar.vue'
</script>

<style scoped>
.user-page {
  display: flex;
  flex-direction: row;
  align-items: flex-start;
  padding: 20px 200px;
}
</style>
