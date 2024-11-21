<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý khách sạn</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div class="content-main">
      <HotelItem
        v-for="hotel in hotels"
        :hotel="hotel"
        :mode="1"
        :key="hotel.Id"
        @updateHotel="updateHotel(hotel.Id)"
      ></HotelItem>
    </div>
  </div>
  <HotelForm v-if="showDetail" @closeForm="resetDefault" :id="hotelUpdateId"></HotelForm>
</template>

<script setup>
import { useHotelStore } from '@/stores/hotel'
import { useUserStore } from '@/stores/user'
import { computed, onMounted, ref } from 'vue'
import HotelItem from './HotelItem.vue'
import '../../styles/base/button.css'
import HotelForm from './HotelForm.vue'

const showDetail = ref(false)
const hotelUpdateId = ref(null)

const hotelStore = useHotelStore()
const userStore = useUserStore()

function addNew() {
  showDetail.value = true
  hotelUpdateId.value = null
}

function updateHotel(id) {
  showDetail.value = true
  hotelUpdateId.value = id
}

function resetDefault() {
  showDetail.value = false
  hotelUpdateId.value = null
  fetchHotels()
}

function fetchHotels() {
  if (user.value && user.value.Id && token.value) {
    hotelStore.fetchHotelByPartner(user.value.Id, token.value)
  }
}

const hotels = computed(() => hotelStore.getHotels)
const token = computed(() => userStore.getToken)
const user = computed(() => userStore.getUser)

onMounted(() => {
  fetchHotels()
})
</script>
