<template>
  <div class="content content--column">
    <div class="content--column">
      <div style="display: flex; flex-direction: row; justify-content: space-between">
        <h3 style="margin-bottom: 10px">Các phòng đã đặt</h3>
        <button @click="goBookingTour" class="btn btn--add">Đặt tour</button>
      </div>
      <div class="content--row">
        <div class="btn btn--add" @click="selectStatus(0)">Chưa thanh toán</div>
        <div class="btn btn--add" @click="selectStatus(1)">Đã thanh toán</div>
        <div class="btn btn--add" @click="selectStatus(2)">Đã hủy</div>
      </div>
      <div v-for="booking in myBookingsRoom" :key="booking.Id">
        <BookingRoomItem
          :booking="booking"
          @cancelBooking="cancelBooking"
          @paymentBooking="paymentBooking"
        ></BookingRoomItem>
      </div>
    </div>

    <ThePagnigation
      :pageNumber="pageNumber"
      :totalItems="totalItems"
      :totalPages="totalPages"
      @pageChange="handlePageChange"
    />
  </div>
</template>
<script setup>
import { useBookingStore } from '@/stores/booking'
import { useUserStore } from '@/stores/user'
import { computed, onMounted, ref } from 'vue'
import BookingRoomItem from './BookingRoomItem.vue'
import ThePagnigation from '@/components/ThePagnigation.vue'
import { useRouter } from 'vue-router'

const userStore = useUserStore()
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

const statusBooking = ref(null)
const pageNumber = ref(1)

const bookingStore = useBookingStore()

const router = useRouter()

function goBookingTour() {
  router.push({
    name: 'my-booking-tour'
  })
}

function selectStatus(value) {
  statusBooking.value = value
  pageNumber.value = 1
  fetchBooking()
}

function handlePageChange(value) {
  pageNumber.value = value
  fetchBooking()
}

async function cancelBooking(id, reason) {
  const response = await bookingStore.cancelBookingRoom(id, reason, token.value)
  if (response.success) {
    bookingStore.fetchMyBookingRoom(
      user.value.Id,
      statusBooking.value,
      pageNumber.value,
      token.value
    )
  } else {
    console.log(response.message)
  }
}

async function paymentBooking(id) {
  const request = {
    BookingId: id,
    Amount: 0
  }

  const response = await bookingStore.paymentForBookingRoom(request, token.value)
  if (response.success) {
    const urlPayment = response.url
    window.location.href = urlPayment
  } else {
    console.log(response.message)
  }
}

function fetchBooking() {
  if (user.value && user.value.Id && token.value) {
    bookingStore.resetBookingRooms()
    bookingStore.fetchMyBookingRoom(
      user.value.Id,
      statusBooking.value,
      pageNumber.value,
      token.value
    )
  }
}

const myBookingsRoom = computed(() => bookingStore.getMyBookingsRoom)
const totalPages = computed(() => bookingStore.getTotalPages)
const totalItems = computed(() => bookingStore.getTotalItems)

onMounted(() => {
  fetchBooking()
})
</script>

<style scoped>
.btn {
  margin-right: 10px;
}
</style>
