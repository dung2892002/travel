<template>
  <div>
    <div class="user-menu" v-if="!user">
      <RouterLink :to="{ name: 'login' }" class="btn">Đăng nhập</RouterLink>
      <RouterLink :to="{ name: 'register' }" class="btn">Đăng ký</RouterLink>
    </div>

    <div class="user-menu" v-else>
      <img
        :src="getLinkImage(user.AvatarImage)"
        alt="anh dai dien"
        class="avatar-image"
        @click="toggleMenu"
      />
      <h3>Welcome, {{ user.DisplayName }}</h3>
      <div v-if="showMenu" class="user-dropdown-menu">
        <ul>
          <li @click="showProfile">Thông tin cá nhân</li>
          <li @click="booking(user.Id)">Booking</li>
          <li @click="logout">Đăng xuất</li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import '../styles/layout/header.css'
import '../styles/base/button.css'
import { computed, ref } from 'vue'
import { useUserStore } from '../stores/user/index'
import { getLinkImage } from '@/utils'
import { useRouter } from 'vue-router'

const showMenu = ref(false)
const router = useRouter()

const toggleMenu = () => {
  showMenu.value = !showMenu.value
}

const userStore = useUserStore()
const user = computed(() => userStore.getUser)
const showProfile = () => {
  router.push('profile')
  toggleMenu()
}

const logout = () => {
  userStore.logout()
  toggleMenu()
}

const booking = () => {
  router.push('my-booking')
  toggleMenu()
}
</script>
