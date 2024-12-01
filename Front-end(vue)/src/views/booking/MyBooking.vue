<template>
  <div class="content content--column">
    <div class="content--column">
      <h3>Các phòng đã đặt</h3>
      <div v-for="booking in myBookingsRoom" :key="booking.Id">
        <BookingRoomItem :booking="booking" @cancelBooking="cancelBooking"></BookingRoomItem>
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
import { computed, onMounted } from 'vue'
import BookingRoomItem from './BookingRoomItem.vue'

const userStore = useUserStore()
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

const bookingStore = useBookingStore()

async function cancelBooking(id, reason) {
  var response = await bookingStore.cancelBookingRoom(id, reason, token.value)
  if (response.success) {
    bookingStore.fetchMyBooking(user.value.Id, token.value)
  } else {
    console.log(response.message)
  }
}

const myBookingsRoom = computed(() => bookingStore.getMyBookingsRoom)
// const myBookingsTour = computed(() => bookingStore.getMyBookingsTour)

onMounted(() => {
  if (user.value && user.value.Id && token.value) {
    bookingStore.fetchMyBooking(user.value.Id, token.value)
  }
})
</script>
