<template>
  <div class="search">
    <div class="search-item search-location" ref="searchLocationRef" @click="showSearch">
      <input
        v-if="!errorLocation"
        type="text"
        v-model="keyword"
        placeholder="Nhập từ khóa để tìm kiếm..."
      />
      <span v-if="errorLocation" class="error-message"> {{ errorLocation }}</span>
      <div v-if="showSelectSearch === 1" class="show-select select-location">
        <ul v-if="provincesFilter.length > 0">
          <li
            v-for="province in provincesFilter"
            :key="province.Id"
            @click="selectProvince(province)"
            class="location-item"
          >
            <p class="location--name">{{ province.Name }}</p>
            <p class="location--type">Tỉnh</p>
          </li>
        </ul>
        <ul v-if="citiesFilter.length > 0">
          <li
            v-for="city in citiesFilter"
            :key="city.Id"
            @click="selectCity(city)"
            class="location-item"
          >
            <img :src="getLinkImage(city.Image)" alt="image" class="location--image" />
            <div>
              <p class="location--name">{{ city.Name }}</p>
              <p class="location--name-province">{{ city.Province.Name }}</p>
            </div>
            <p class="location--type">Thành phố</p>
          </li>
        </ul>
      </div>
    </div>
    <div class="search-item search-time" ref="searchTimeRef">
      <div @click="showSelectSearch = 2">Khởi hành sau {{ formatDate(query.DateStart) }}</div>
      <div v-if="showSelectSearch == 2" class="show-select select-time">
        <div class="time-item">
          <div class="label">Khởi hành:</div>
          <input type="date" v-model="query.DateStart" />
        </div>
        <div class="time-item time-control">
          <div>
            <div v-if="errorTime" class="error-message">{{ errorTime }}</div>
          </div>
          <button @click="closeSelectSearch" class="btn btn--success">Ok</button>
        </div>
      </div>
    </div>
    <div class="search-item search-button" @click="searchTour">
      <button>Tìm kiếm</button>
    </div>
  </div>
</template>
<script setup>
import { useLocationStore } from '@/stores/location'
import { formatDate, formatDateForm, getLinkImage } from '@/utils'
import { isBefore, isSameDay, parseISO } from 'date-fns'
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'

const keyword = ref('')
const showSelectSearch = ref(0)
const citiesFilter = ref([])
const provincesFilter = ref([])

const searchLocationRef = ref(null)
const searchTimeRef = ref(null)
const searchRoomRef = ref(null)
const errorTime = ref(null)
const errorLocation = ref(null)

const emit = defineEmits(['searchTour'])

const query = ref({
  ProvinceId: null,
  CityId: null,
  DateStart: formatDateForm(new Date())
})

function showSearch() {
  errorLocation.value = null
  showSelectSearch.value = 1
}

function searchTour() {
  if (query.value.CityId == null && query.value.ProvinceId == null) {
    errorLocation.value = 'Vui lòng chọn địa điểm'
  } else emit('searchTour', query.value)
}

function filterLocation() {
  const query = keyword.value.toLowerCase()
  provincesFilter.value = provinces.value
    ? provinces.value.filter((province) => province.Name.toLowerCase().includes(query))
    : []

  citiesFilter.value = cities.value
    ? cities.value.filter((city) => city.Name.toLowerCase().includes(query))
    : []
}

watch(keyword, filterLocation)

function closeSelectSearch() {
  if (validateTime()) {
    showSelectSearch.value = 0
  }
}

watch([() => query.value.DateStart], validateTime)

function validateTime() {
  errorTime.value = null
  const today = new Date()
  const dateStart = parseISO(query.value.DateStart)

  if (isBefore(dateStart, today) && !isSameDay(dateStart, today)) {
    errorTime.value = 'Ngày khởi hành phải từ hôm nay trở đi'
    return false
  }

  return true
}

function selectProvince(province) {
  query.value.ProvinceId = province.Id
  query.value.CityId = null
  keyword.value = province.Name
  showSelectSearch.value = 0
}

function selectCity(city) {
  query.value.ProvinceId = null
  query.value.CityId = city.Id
  keyword.value = city.Name
  showSelectSearch.value = 0
}

const locationStore = useLocationStore()

const cities = computed(() => locationStore.getCities)
const provinces = computed(() => locationStore.getProvinces)

function handleClickOutside(event) {
  if (
    showSelectSearch.value === 1 &&
    searchLocationRef.value &&
    !searchLocationRef.value.contains(event.target)
  ) {
    closeSelectSearch()
  }
  if (
    showSelectSearch.value === 2 &&
    searchTimeRef.value &&
    !searchTimeRef.value.contains(event.target)
  ) {
    closeSelectSearch()
  }
  if (
    showSelectSearch.value === 3 &&
    searchRoomRef.value &&
    !searchRoomRef.value.contains(event.target)
  ) {
    closeSelectSearch()
  }
}

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})

onMounted(() => {
  locationStore.fetchCities()
  locationStore.fetchProvinces()
  document.addEventListener('click', handleClickOutside)
})
</script>
