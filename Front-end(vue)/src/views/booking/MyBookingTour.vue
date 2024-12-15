<template>
  <div class="content content--column">
    <div class="content--column">
      <h3 style="margin-bottom: 10px">Các tour đã đặt</h3>
      <div class="content--row">
        <div class="btn btn--add" @click="selectStatus(0)">Chưa thanh toán</div>
        <div class="btn btn--add" @click="selectStatus(1)">Đã thanh toán</div>
        <div class="btn btn--add" @click="selectStatus(2)">Đã hủy</div>
      </div>
      <div v-for="booking in myBookingsTour" :key="booking.Id">
        <BookingTourItem
          :booking="booking"
          @cancelBooking="cancelBooking"
          @paymentBooking="paymentBooking"
        ></BookingTourItem>
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
import ThePagnigation from '@/components/ThePagnigation.vue'
import BookingTourItem from './BookingTourItem.vue'

const userStore = useUserStore()
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

const statusBooking = ref(null)
const pageNumber = ref(1)

const bookingStore = useBookingStore()

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
  const response = await bookingStore.cancelBookingTour(id, reason, token.value)
  if (response.success) {
    bookingStore.fetchMyBookingTour(
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

  const response = await bookingStore.paymentForBookingTour(request, token.value)
  if (response.success) {
    const urlPayment = response.url
    window.open(urlPayment, '_blank')
  } else {
    console.log(response.message)
  }
}

function fetchBooking() {
  if (user.value && user.value.Id && token.value) {
    bookingStore.resetBookingTours()
    bookingStore.fetchMyBookingTour(
      user.value.Id,
      statusBooking.value,
      pageNumber.value,
      token.value
    )
  }
}

const myBookingsTour = computed(() => bookingStore.getMyBookingsTour)
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
