<template>
  <div class="content content--column">
    <div class="content--column">
      <h1>dat phong</h1>
      <div>
        <div v-for="booking in myBookingsRoom" :key="booking.Id">
          <div>
            <h3>ngay dat:</h3>
            <h3>{{ formatDate(booking.CreatedAt) }}</h3>
          </div>
          <div>
            <h3>nhan phong:</h3>
            <h3>{{ formatDate(booking.CheckInDate) }}</h3>
          </div>
          <div>
            <h3>tra phong:</h3>
            <h3>{{ formatDate(booking.CheckOutDate) }}</h3>
          </div>
          <div>
            <h3>tong so tien:</h3>
            <h3>{{ formatMoney(booking.Price) }}</h3>
          </div>
        </div>
      </div>
    </div>
    <div class="content--column">
      <h1>dat tour</h1>
    </div>
  </div>
</template>
<script setup>
import { useBookingStore } from '@/stores/booking'
import { useUserStore } from '@/stores/user'
import { formatDate, formatMoney } from '@/utils'
import { computed, onMounted } from 'vue'

const userStore = useUserStore()
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

const bookingStore = useBookingStore()

const myBookingsRoom = computed(() => bookingStore.getMyBookingsRoom)
// const myBookingsTour = computed(() => bookingStore.getMyBookingsTour)

onMounted(() => {
  if (user.value && user.value.Id && token.value) {
    bookingStore.fetchMyBooking(user.value.Id, token.value)
  }
})
</script>
