<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý lịch khởi hành</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div v-if="showForm" class="form">
      <span class="error-message" v-if="errorMessage">{{ errorMessage }}</span>
      <div class="form__item">
        <span>Ngày khởi hành</span>
        <input type="date" v-model="scheduleSubmit.DateStart" class="form__input" />
      </div>
      <div class="form__item">
        <span>Giá</span>
        <input type="number" v-model="scheduleSubmit.Price" class="form__input" />
      </div>
      <div class="content--row" style="justify-content: space-between">
        <button class="btn btn--remove" @click="closeForm" style="width: 60px">Hủy</button>
        <button class="btn btn--add" @click="submitForm" style="width: 60px">Lưu</button>
      </div>
    </div>
    <div class="content-main">
      <div class="main-container">
        <table class="travel-table">
          <thead>
            <tr>
              <th>STT</th>
              <th>Ngày khởi hành</th>
              <th>Giá tiền</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(schedule, index) in schedules" :key="schedule.Id">
              <td class="w-10">{{ index + 1 }}</td>
              <td class="w-32">{{ formatDate(schedule.DateStart) }}</td>
              <td class="w-32">
                <span v-if="isEditPrice != index">{{ formatNumber(schedule.Price) }}</span>
                <div v-else>
                  <span class="error-message" v-if="errorUpdateMessage">
                    {{ errorUpdateMessage }}</span
                  >
                  <input type="number" v-model="priceUpdate" class="form__input" />
                </div>
              </td>
              <td>
                <button
                  class="btn btn--add"
                  v-if="isEditPrice != index"
                  @click="(isEditPrice = index), (priceUpdate = schedule.Price)"
                >
                  Sửa giá
                </button>
                <div v-else>
                  <button
                    class="btn btn--remove"
                    @click="(isEditPrice = -1), (priceUpdate = 0)"
                    style="margin-right: 4px"
                  >
                    Hủy
                  </button>
                  <button class="btn btn--add" @click="handleEditPrice(schedule)">Lưu</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/form.css'
import '../../styles/layout/table.css'
import '../../styles/utils.css'
import { computed, onMounted, ref } from 'vue'
import '../../styles/base/button.css'
import { useRoute } from 'vue-router'
import { useTourStore } from '@/stores/tour'
import { formatDate, formatDateForm, formatNumber } from '@/utils'
import { useUserStore } from '@/stores/user'

const showForm = ref(false)
const isEditPrice = ref(-1)
const priceUpdate = ref(0)
const errorMessage = ref(false)
const errorUpdateMessage = ref(false)
const scheduleSubmit = ref({
  DateStart: formatDateForm(new Date())
})

const route = useRoute()
const tourId = route.params.tourId

const tourStore = useTourStore()
const userStore = useUserStore()

function addNew() {
  showForm.value = true
}

function fetchSchedules() {
  tourStore.fetchScheduleByTour(tourId)
}

function closeForm() {
  scheduleSubmit.value = {
    DateStart: formatDateForm(new Date())
  }
  showForm.value = false
  errorMessage.value = false
}

async function handleEditPrice(schedule) {
  schedule.Price = priceUpdate.value

  const response = await tourStore.updatePriceSchedule(schedule, schedule.Id, token.value)

  if (response.success) {
    isEditPrice.value = -1
    errorUpdateMessage.value = false
    fetchSchedules()
  } else {
    errorUpdateMessage.value = response.message
  }
}

async function submitForm() {
  scheduleSubmit.value.TourId = tourId
  const response = await tourStore.createSchedule(scheduleSubmit.value, token.value)
  if (response.success) {
    fetchSchedules()
    showForm.value = false
  } else {
    errorMessage.value = response.message
  }
}

const schedules = computed(() => tourStore.getSchedules)
const token = computed(() => userStore.getToken)

onMounted(() => {
  fetchSchedules()
})
</script>

<style scoped>
.form {
  width: 200px;
  margin-bottom: 20px;
  margin-left: 80%;
  z-index: 1;
  display: flex;
  flex-direction: column;
  gap: 6px;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  right: 0;
}
</style>
