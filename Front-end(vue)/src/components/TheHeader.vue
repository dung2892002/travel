<template>
  <header>
    <RouterLink :to="{ name: 'home' }">
      <img src="./../assets/image/logo.png" alt="logo_web" class="header--logo" />
    </RouterLink>
    <TheNavbar />
    <UserMenu />
  </header>
</template>

<script setup>
import '../styles/layout/header.css'

import TheNavbar from './TheNavbar.vue'
import UserMenu from './UserMenu.vue'
</script>
