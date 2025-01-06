<template class="container">
  <div class="sticky-header"><SearchTag @searchTour="updateSearchQuery" /></div>

  <div class="content--row">
    <FilterBar @filterTour="updateFilters" v-if="isSearch" :resetFilters="resetFilters" />
    <div v-if="tours" class="content--column">
      <div class="content--column" v-if="tours.length > 0">
        <TourItem
          v-for="tour in tours"
          :key="tour.Id"
          :tour="tour"
          :mode="0"
          @selectTour="viewDetail(tour.Id)"
        ></TourItem>
        <ThePagnigation
          :pageNumber="pageNumber"
          :totalItems="totalItems"
          :totalPages="totalPages"
          @pageChange="handlePageChange"
        />
      </div>
      <div v-else class="content--column" style="margin-left: 320px">Không tìm thấy tour</div>
    </div>
    <div v-else style="margin: 0; padding: 0">
      <img
        src="https://ik.imagekit.io/tvlk/image/imageResource/2025/01/05/1736039105980-f92c5570060d22e1a5d244eed8bc7f87.png?tr=dpr-2,q-75"
        alt=""
        style="margin: 0; padding: 0"
      />
    </div>
  </div>
</template>

<script setup>
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import '../../styles/layout/search.css'
import '../../styles/utils.css'
import { computed, nextTick, onMounted, ref } from 'vue'
import FilterBar from './FilterBar.vue'
import { useRouter } from 'vue-router'
import ThePagnigation from '@/components/ThePagnigation.vue'
import SearchTag from './SearchTag.vue'
import TourItem from './TourItem.vue'
import { useTourStore } from '@/stores/tour'

const tourStore = useTourStore()
const searchQuery = ref(null)
const filters = ref(null)
const pageNumber = ref(1)
const router = useRouter()
const isSearch = ref(false)
const resetFilters = ref(false)

function handlePageChange(value) {
  pageNumber.value = value
  handleSearch()
}

function updateSearchQuery(newQuery) {
  isSearch.value = true
  searchQuery.value = newQuery
  filters.value = {}

  resetFilters.value = true
  nextTick(() => {
    resetFilters.value = false
  })

  handleSearch()
}

function updateFilters(newFilters) {
  filters.value = newFilters
  handleSearch()
}

async function handleSearch() {
  const query = Object.assign({}, searchQuery.value, filters.value, {
    PageNumber: pageNumber.value
  })
  await fetchTours(query)
}

function viewDetail(id) {
  sessionStorage.setItem(
    'querySchedule',
    JSON.stringify({
      TourId: id,
      DateStart: searchQuery.value.DateStart,
      MinPrice: filters.value.MinPrice,
      MaxPrice: filters.value.MaxPrice
    })
  )
  router.push({
    name: 'tourDetail',
    params: { id: id }
  })
}

const tours = computed(() => tourStore.getSearchTours)

const totalPages = computed(() => tourStore.getTotalPages)
const totalItems = computed(() => tourStore.getTotalItems)

async function fetchTours(query) {
  tourStore.clearSearchTours()
  await tourStore.fetchSearchTours(query)
}

onMounted(() => {
  tourStore.clearSearchTours()
})
</script>
