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
      <div @click="showSelectSearch = 2">
        {{ formatDate(query.CheckIn) }} - {{ formatDate(query.CheckOut) }},
        {{ calculateDay(query.CheckIn, query.CheckOut) }} ngày
      </div>
      <div v-if="showSelectSearch == 2" class="show-select select-time">
        <div class="time-item">
          <div class="label">CheckIn:</div>
          <input type="date" v-model="query.CheckIn" />
        </div>
        <div class="time-item">
          <div class="label">CheckOut:</div>
          <input type="date" v-model="query.CheckOut" />
        </div>
        <div class="time-item time-control">
          <div>
            <div v-if="errorTime" class="error-message">{{ errorTime }}</div>
          </div>
          <button @click="closeSelectSearch" class="btn btn--success">Ok</button>
        </div>
      </div>
    </div>
    <div class="search-item search-room" ref="searchRoomRef">
      <div @click="showSelectSearch = 3">
        {{ query.QuantityAdultPeople }} Người lớn, {{ query.QuantityChildrenPeople }} Trẻ em,
        {{ query.QuantityRoom }} Phòng
      </div>
      <div v-if="showSelectSearch === 3" class="show-select select-room">
        <div class="room-item">
          <div class="label">Người lớn:</div>
          <div class="room-input">
            <button class="btn--quantity" @click="subtractAdultPeople">-</button>
            <div class="show-quantity">{{ query.QuantityAdultPeople }}</div>
            <button class="btn--quantity" @click="addAdultPeople">+</button>
          </div>
        </div>
        <div class="room-item">
          <div class="label">Trẻ em:</div>
          <div class="room-input">
            <button class="btn--quantity" @click="subtractChildrenPeople">-</button>
            <div class="show-quantity">{{ query.QuantityChildrenPeople }}</div>
            <button class="btn--quantity" @click="addChildrenPeople">+</button>
          </div>
        </div>
        <div class="room-item">
          <div class="label">Số phòng:</div>
          <div class="room-input">
            <button class="btn--quantity" @click="subtractRoom">-</button>
            <div class="show-quantity">{{ query.QuantityRoom }}</div>
            <button class="btn--quantity" @click="addRoom">+</button>
          </div>
        </div>
        <div class="room-item room-control">
          <div>
            <div v-if="errorRoom" class="error-message">{{ errorRoom }}</div>
          </div>
          <button @click="closeSelectSearch" class="btn btn--success">Ok</button>
        </div>
      </div>
    </div>
    <div class="search-item search-button">
      <button @click="searchHotel">Tìm kiếm</button>
    </div>
  </div>
</template>
<script setup>
import { useLocationStore } from '@/stores/location'
import { calculateDay, formatDate, formatDateForm, getLinkImage } from '@/utils'
import { isBefore, isSameDay, parseISO } from 'date-fns'
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'

const keyword = ref('')
const showSelectSearch = ref(0)
const provincesFilter = ref([])
const citiesFilter = ref([])

const searchLocationRef = ref(null)
const searchTimeRef = ref(null)
const searchRoomRef = ref(null)

const errorRoom = ref(null)
const errorTime = ref(null)
const errorLocation = ref(null)

const emit = defineEmits(['searchHotel'])

const query = ref({
  ProvinceId: null,
  CityId: null,
  CheckIn: formatDateForm(new Date()),
  CheckOut: formatDateForm(new Date().setDate(new Date().getDate() + 1)),
  QuantityRoom: 1,
  QuantityAdultPeople: 2,
  QuantityChildrenPeople: 1
})

function showSearch() {
  errorLocation.value = null
  showSelectSearch.value = 1
}

function searchHotel() {
  if (query.value.ProvinceId == null && query.value.CityId == null) {
    errorLocation.value = 'Vui lòng chọn địa điểm'
  } else emit('searchHotel', query.value)
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

watch([() => query.value.CheckIn, () => query.value.CheckOut], validateTime)

function validateTime() {
  errorTime.value = null
  const today = new Date()
  const checkInDate = parseISO(query.value.CheckIn)
  const checkOutDate = parseISO(query.value.CheckOut)

  if (isBefore(checkInDate, today) && !isSameDay(checkInDate, today)) {
    errorTime.value = 'Ngày nhận phòng phải từ hôm nay trở đi'
    return false
  }

  if (!isBefore(checkInDate, checkOutDate)) {
    errorTime.value = 'Ngày trả phòng phải sau ngày nhận phòng'
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

function addAdultPeople() {
  query.value.QuantityAdultPeople++
  errorRoom.value = null
}

function subtractAdultPeople() {
  if (
    query.value.QuantityAdultPeople > 1 &&
    query.value.QuantityAdultPeople > query.value.QuantityRoom
  ) {
    query.value.QuantityAdultPeople--
    errorRoom.value = null
  } else {
    errorRoom.value = 'Số lượng người lớn không được nhỏ hơn số phòng'
  }
}

function addChildrenPeople() {
  query.value.QuantityChildrenPeople++
  errorRoom.value = null
}

function subtractChildrenPeople() {
  if (query.value.QuantityChildrenPeople > 0) {
    query.value.QuantityChildrenPeople--
    errorRoom.value = null
  } else {
    errorRoom.value = 'Số lượng trẻ em phải lớn hơn 0'
  }
}

function addRoom() {
  if (query.value.QuantityRoom < query.value.QuantityAdultPeople) {
    query.value.QuantityRoom++
  } else {
    errorRoom.value = 'Số phòng không được lớn hơn số lượng người lớn'
  }
}

function subtractRoom() {
  if (query.value.QuantityRoom > 1) {
    query.value.QuantityRoom--
  } else {
    errorRoom.value = 'Số phòng phải lớn hơn 0'
  }
}

const locationStore = useLocationStore()

const provinces = computed(() => locationStore.getProvinces)
const cities = computed(() => locationStore.getCities)

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
  locationStore.fetchProvinces()
  locationStore.fetchCities()
  document.addEventListener('click', handleClickOutside)
})
</script>
