<template>
  <SearchTag @searchHotel="updateSearchQuery" />
  <div class="content--row" v-if="hotels">
    <FilterBar @filterHotel="updateFilters" />
    <div class="content--column" v-if="hotels.length > 0">
      <HotelItem
        v-for="hotel in hotels"
        :key="hotel.Id"
        :hotel="hotel"
        :mode="0"
        @selectHotel="viewDetail(hotel.Id)"
      ></HotelItem>
    </div>
    <div v-else>không có khách sạn nào thỏa mãn</div>
  </div>
</template>

<script setup>
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import '../../styles/layout/search.css'
import '../../styles/utils.css'
import { computed, ref } from 'vue'
import { useHotelStore } from '@/stores/hotel'
import HotelItem from './HotelItem.vue'
import SearchTag from './SearchTag.vue'
import FilterBar from './FilterBar.vue'

const hotelStore = useHotelStore()
const searchQuery = ref(null)
const filters = ref()

function updateSearchQuery(newQuery) {
  searchQuery.value = newQuery
  filters.value = {}
  handleSearch()
}

function updateFilters(newFilters) {
  filters.value = newFilters
  handleSearch()
}

async function handleSearch() {
  const query = Object.assign({}, searchQuery.value, filters.value)
  await fetchHotels(query)
}

function viewDetail(id) {
  console.log('xem chi tiet hotel', id)
}

const hotels = computed(() => hotelStore.getSearchHotels)

async function fetchHotels(query) {
  await hotelStore.fetchSearchHotels(query)
}
</script>
