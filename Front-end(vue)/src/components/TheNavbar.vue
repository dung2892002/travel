<template>
  <nav>
    <RouterLink :to="{ name: 'hotel' }" class="btn nav-link"> Khách sạn </RouterLink>
    <RouterLink :to="{ name: 'tour' }" class="btn nav-link"> Tour </RouterLink>
  </nav>
</template>

<script setup>
import '../styles/base/button.css'
import '../styles/layout/navbar.css'
</script>
