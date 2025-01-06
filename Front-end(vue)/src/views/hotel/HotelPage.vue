<template class="container">
  <div class="sticky-header"><SearchTag @searchHotel="updateSearchQuery" /></div>

  <div class="content--row">
    <FilterBar @filterHotel="updateFilters" v-if="isSearch" :resetFilters="resetFilters" />
    <div v-if="hotels" class="content--column">
      <div class="content--column" v-if="hotels.length > 0">
        <HotelItem
          v-for="hotel in hotels"
          :key="hotel.Id"
          :hotel="hotel"
          :mode="0"
          @selectHotel="viewDetail(hotel.Id)"
        ></HotelItem>
        <ThePagnigation
          :pageNumber="pageNumber"
          :totalItems="totalItems"
          :totalPages="totalPages"
          @pageChange="handlePageChange"
        />
      </div>
      <div v-else class="content--column" style="margin-left: 320px">Không tìm thấy khách sạn</div>
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
import { useHotelStore } from '@/stores/hotel'
import HotelItem from './HotelItem.vue'
import SearchTag from './SearchTag.vue'
import FilterBar from './FilterBar.vue'
import { useRouter } from 'vue-router'
import ThePagnigation from '@/components/ThePagnigation.vue'

const hotelStore = useHotelStore()
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
  await fetchHotels(query)
}

function viewDetail(id) {
  sessionStorage.setItem(
    'queryRoom',
    JSON.stringify({
      HotelId: id,
      CheckIn: searchQuery.value.CheckIn,
      CheckOut: searchQuery.value.CheckOut,
      QuantityAdultPeople: searchQuery.value.QuantityAdultPeople,
      QuantityChildrenPeople: searchQuery.value.QuantityChildrenPeople,
      QuantityRoom: searchQuery.value.QuantityRoom,
      MinPrice: filters.value.MinPrice,
      MaxPrice: filters.value.MaxPrice
    })
  )
  router.push({
    name: 'hotelDetail',
    params: { id: id }
  })
}

const hotels = computed(() => hotelStore.getSearchHotels)

const totalPages = computed(() => hotelStore.getTotalPages)
const totalItems = computed(() => hotelStore.getTotalItems)

async function fetchHotels(query) {
  hotelStore.clearSearchHotels()
  await hotelStore.fetchSearchHotels(query)
}

onMounted(() => {
  hotelStore.clearSearchHotels()
})
</script>
