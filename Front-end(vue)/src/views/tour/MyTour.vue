<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý tour</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div class="content-main">
      <div v-if="tours">
        <TourItem
          v-for="tour in tours"
          :key="tour.Id"
          :tour="tour"
          :mode="1"
          @selectTour="updateTour(tour.Id)"
        ></TourItem>
      </div>
      <div v-else>loading</div>
    </div>
  </div>
  <TourForm v-if="showDetail" @closeForm="resetDefault" :id="tourUpdateId"></TourForm>
</template>

<script setup>
import { useUserStore } from '@/stores/user'
import { computed, onMounted, ref } from 'vue'
import '../../styles/base/button.css'
import TourItem from './TourItem.vue'
import TourForm from './TourForm.vue'
import { useTourStore } from '@/stores/tour'

const showDetail = ref(false)
const tourUpdateId = ref(null)

const tourStore = useTourStore()
const userStore = useUserStore()

function addNew() {
  showDetail.value = true
  tourUpdateId.value = null
}

function updateTour(id) {
  showDetail.value = true
  tourUpdateId.value = id
}

function resetDefault() {
  showDetail.value = false
  tourUpdateId.value = null
  fetchTours()
}

function fetchTours() {
  if (user.value && user.value.Id && token.value) {
    tourStore.fetchTourByPartner(user.value.Id, token.value)
  }
}

const tours = computed(() => tourStore.getTours)
const token = computed(() => userStore.getToken)
const user = computed(() => userStore.getUser)

onMounted(() => {
  fetchTours()
})
</script>
