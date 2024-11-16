<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý khách sạn</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <!-- <img src="/src/assets/icon/add.png" alt="logo" class="button--add-logo" /> -->
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div class="content-main">
      <HotelItem :hotel="hotel" v-for="hotel in hotels" :key="hotel.Id"></HotelItem>
    </div>
  </div>
</template>

<script setup>
import { useHotelStore } from '@/stores/hotel'
import { useUserStore } from '@/stores/user'
import { computed, onMounted } from 'vue'
import HotelItem from './HotelItem.vue'
import '../../styles/base/button.css'

const hotelStore = useHotelStore()
const userStore = useUserStore()

const hotels = computed(() => hotelStore.getHotels)
const token = computed(() => userStore.getToken)
const user = computed(() => userStore.getUser)

onMounted(() => {
  if (user.value && user.value.Id && token.value) {
    hotelStore.fetchHotelByPartner(user.value.Id, token.value)
  }
})
</script>
