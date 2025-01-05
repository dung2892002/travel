<template>
  <div class="content content--column">
    <div
      class="content__header"
      style="
        display: flex;
        flex-direction: column;
        gap: 10px;
        margin-bottom: 20px;
        align-items: self-start;
      "
    >
      <h1 class="content__title">Trang thống kê</h1>
      <span v-if="error" class="error-message">{{ error }}</span>
    </div>
    <div class="content-main">
      <div class="toolbar">
        <div class="toolbar__search">
          <div>
            <span>Ngày bắt đầu</span>
            <input class="form__input" type="date" v-model="start" />
          </div>
          <div>
            <span>Ngày kết thúc</span>
            <input class="form__input" type="date" v-model="end" />
          </div>
          <div v-if="checkRole(5)" style="position: relative" @click="toggleSelect">
            <span>Chọn khách sạn</span>
            <input
              type="text"
              class="form__input"
              placeholder="Chọn khách sạn"
              v-model="selectedHotel.Name"
              v-if="selectedHotel"
            />
            <input type="text" class="form__input" placeholder="Chọn khách sạn" v-else />
            <div v-if="showSelect" class="show-select">
              <span v-for="hotel in hotels" :key="hotel.Id" @click="selectHotel(hotel)">{{
                hotel.Name
              }}</span>
            </div>
          </div>
          <div v-if="checkRole(6)" style="position: relative" @click="toggleSelect">
            <span>Chọn tour</span>
            <input
              type="text"
              class="form__input"
              placeholder="Chọn tour"
              v-model="selectedTour.Name"
              v-if="selectedTour"
            />
            <input type="text" class="form__input" placeholder="Chọn tour" v-else />
            <div v-if="showSelect" class="show-select">
              <span v-for="tour in tours" :key="tour.Id" @click="selectTour(tour)">{{
                tour.Name
              }}</span>
            </div>
          </div>
          <button
            class="btn btn--add"
            @click="handleStatistical"
            style="min-width: 120px; margin-top: 20px"
          >
            Thống kê
          </button>
        </div>
      </div>
      <div class="main-container" v-if="statistical">
        <div class="content--row">
          <div class="content--column" style="margin-bottom: 10px">
            <span>Tống số booking: {{ statistical.TotalBooking }}</span>
            <span>Số booking hoàn thành: {{ statistical.TotalBookingSuccess }}</span>
            <span>Số booking bị hủy: {{ statistical.TotalBookingCancel }}</span>
            <span>Số booking chưa hoàn thành: {{ statistical.TotalBookingPending }}</span>
            <span
              >Tống số tiền được thanh toán:
              {{ formatNumber(statistical.TotalPaymentAccessValue) }} vnd</span
            >
            <span
              >Tống số tiền hoàn trả:
              {{ formatNumber(statistical.TotalPaymentRefundValue) }} vnd</span
            >
          </div>
          <!-- Biểu đồ tròn -->
          <div class="chart-container" style="width: 300px; height: 300px; margin: 20px auto">
            <canvas id="pieChart"></canvas>
          </div>
        </div>
        <!-- Biểu đồ cột -->
        <div class="chart-container" style="margin-bottom: 20px">
          <canvas id="barChart"></canvas>
        </div>
        <table class="travel-table">
          <thead>
            <tr>
              <th class="w-12">Ngày</th>
              <th class="w-8">Tổng</th>
              <th class="w-11">Hoàn thành</th>
              <th class="w-9">Đã hủy</th>
              <th class="w-18">Chưa hoàn thành</th>
              <th class="w-20">Thanh toán (vnd)</th>
              <th>Hoàn trả (vnd)</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(statisticalDay, index) in statistical.StatisticalDay" :key="index">
              <td>{{ formatDate(statisticalDay.Day) }}</td>
              <td>{{ statisticalDay.TotalBooking }}</td>
              <td>{{ statisticalDay.TotalBookingSuccess }}</td>
              <td>{{ statisticalDay.TotalBookingCancel }}</td>
              <td>{{ statisticalDay.TotalBookingPending }}</td>
              <td>{{ formatNumber(statisticalDay.PaymentValue) }}</td>
              <td>{{ formatNumber(statisticalDay.RefundValue) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useHotelStore } from '@/stores/hotel'
import { useStatiscalStore } from '@/stores/statiscal'
import { useTourStore } from '@/stores/tour'
import { useUserStore } from '@/stores/user'
import { formatDate, formatDateForm, formatNumber } from '@/utils'
import Chart from 'chart.js/auto'
import { computed, onMounted, ref } from 'vue'

const userStore = useUserStore()
const hotelStore = useHotelStore()
const tourStore = useTourStore()
const statiscalStore = useStatiscalStore()

const start = ref(formatDateForm(new Date()))
const end = ref(formatDateForm(new Date()))
const selectedHotel = ref(null)
const selectedTour = ref(null)
const error = ref(null)

var showSelect = ref(false)

function selectHotel(hotel) {
  selectedHotel.value = hotel
}

function selectTour(tour) {
  selectedTour.value = tour
}

function toggleSelect() {
  showSelect.value = !showSelect.value
}

function checkRole(value) {
  return user.value.Roles.some((role) => role.RoleValue === value)
}

async function handleStatistical() {
  statiscalStore.reset()
  if (end.value < start.value) {
    error.value = 'Ngày kết thúc phải lớn hơn ngày bắt đầu'
    return
  }
  if (checkRole(10)) {
    await statiscalStore.adminStatistical(start.value, end.value, token.value)
  } else if (checkRole(5)) {
    await statiscalStore.hotelStatistical(
      start.value,
      end.value,
      user.value.Id,
      selectedHotel.value ? selectedHotel.value.Id : null,
      token.value
    )
  } else {
    await statiscalStore.tourStatistical(
      start.value,
      end.value,
      user.value.Id,
      selectedTour.value ? selectedTour.value.Id : null,
      token.value
    )
  }

  createCharts()
}

function createCharts() {
  const pieCtx = document.getElementById('pieChart').getContext('2d')
  const barCtx = document.getElementById('barChart').getContext('2d')

  new Chart(pieCtx, {
    type: 'pie',
    data: {
      labels: ['Hoàn thành', 'Chưa hoàn thành', 'Đã hủy'],
      datasets: [
        {
          data: [
            (statistical.value.TotalBookingSuccess / statistical.value.TotalBooking) * 100,
            (statistical.value.TotalBookingPending / statistical.value.TotalBooking) * 100,
            (statistical.value.TotalBookingCancel / statistical.value.TotalBooking) * 100
          ],
          backgroundColor: ['#4caf50', '#ff9800', '#f44336']
        }
      ]
    }
  })

  new Chart(barCtx, {
    type: 'bar',
    data: {
      labels: statistical.value.StatisticalDay.filter(
        (day) => day.PaymentValue > 0 || day.RefundValue > 0
      ).map((day) => formatDate(day.Day)),
      datasets: [
        {
          label: 'Thanh toán (vnd)',
          data: statistical.value.StatisticalDay.filter(
            (day) => day.PaymentValue > 0 || day.RefundValue > 0
          ).map((day) => day.PaymentValue),
          backgroundColor: '#4caf50'
        },
        {
          label: 'Hoàn trả (vnd)',
          data: statistical.value.StatisticalDay.filter(
            (day) => day.PaymentValue > 0 || day.RefundValue > 0
          ).map((day) => day.RefundValue),
          backgroundColor: '#f44336'
        }
      ]
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        }
      }
    }
  })
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
const hotels = computed(() => hotelStore.getHotels)
const tours = computed(() => tourStore.getTours)
const statistical = computed(() => statiscalStore.getStatiscal)

onMounted(() => {
  statiscalStore.reset()
  if (userStore.getUser.Roles.some((role) => role.RoleValue == 5)) {
    hotelStore.fetchHotelByPartner(userStore.getUser.Id, userStore.getToken)
  }

  if (userStore.getUser.Roles.some((role) => role.RoleValue == 6)) {
    tourStore.fetchTourByPartner(userStore.getUser.Id, userStore.getToken)
  }
})
</script>

<style scoped>
.show-select {
  display: flex;
  flex-direction: column;
  gap: 8px;
  position: absolute;
  top: 100%;
  left: 0;
  width: auto;
  padding: 4px 0;
}

.show-select span {
  cursor: pointer;

  padding-left: 8px;
}

.show-select span:hover {
  background-color: #078cf8;
  color: #ffffff;
}
</style>
