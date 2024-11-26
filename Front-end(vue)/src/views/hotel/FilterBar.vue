<template>
  <div class="filter-travel">
    <button @click="filterHotel" class="filter-button">
      <img src="../../assets/icon/filter.png" alt="filter" />
    </button>
    <div class="filter-item">
      <span class="filter__label">Khoảng giá (phòng/đêm)</span>
      <div class="filter-range" ref="rangeSlider">
        <div class="track"></div>
        <div class="range" ref="range"></div>
        <div class="thumb left-thumb" ref="leftThumb" @mousedown="startDrag('left')"></div>
        <div class="thumb right-thumb" ref="rightThumb" @mousedown="startDrag('right')"></div>
        <div class="output">
          <span>VND {{ formatNumber(currentMin) }}</span> -
          <span> {{ formatNumber(currentMax) }}</span>
        </div>
      </div>
    </div>
    <div class="filter-item">
      <span class="filter__label">Rating</span>
      <div class="checkboxes"><input type="checkbox" :value="1" v-model="ratings" /> 1*</div>
      <div class="checkboxes"><input type="checkbox" :value="2" v-model="ratings" /> 2*</div>
      <div class="checkboxes"><input type="checkbox" :value="3" v-model="ratings" /> 3*</div>
      <div class="checkboxes"><input type="checkbox" :value="4" v-model="ratings" /> 4*</div>
      <div class="checkboxes"><input type="checkbox" :value="5" v-model="ratings" /> 5*</div>
    </div>
    <div class="filter-item">
      <span class="filter__label">Đánh giá từ khách</span>
      <div class="checkboxes"><input type="checkbox" :value="7" v-model="guestRatings" /> 7+</div>
      <div class="checkboxes"><input type="checkbox" :value="8" v-model="guestRatings" /> 8+</div>
      <div class="checkboxes"><input type="checkbox" :value="9" v-model="guestRatings" /> 9+</div>
    </div>
    <div class="filter-item">
      <span class="filter__label">Loại hình lưu trú</span>
      <div class="checkboxes">
        <input type="checkbox" :value="1" v-model="types" /> Khu nghỉ dưỡng (Resort)
      </div>
      <div class="checkboxes">
        <input type="checkbox" :value="2" v-model="types" /> Biệt thự (Villa)
      </div>
      <div class="checkboxes">
        <input type="checkbox" :value="3" v-model="types" /> Khách sạn (Hotel)
      </div>
      <div class="checkboxes">
        <input type="checkbox" :value="4" v-model="types" /> Căn hộ (Apartment)
      </div>
      <div class="checkboxes">
        <input type="checkbox" :value="5" v-model="types" /> Nhà nghỉ (Guest house)
      </div>
    </div>
    <div class="filter-item">
      <span class="filter__label">Tiện nghi phổ biến</span>
      <div v-for="facility in popularFacilities" :key="facility.Id" class="checkboxes">
        <input type="checkbox" :value="facility.Id" v-model="selectedHotelFacilities" />
        {{ facility.Name }}
      </div>
    </div>
    <div class="filter-item">
      <span class="filter__label">Tiện nghi đặc biệt</span>
      <div v-for="facility in uniqueFacilities" :key="facility.Id" class="checkboxes">
        <input type="checkbox" :value="facility.Id" v-model="selectedHotelFacilities" />
        {{ facility.Name }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, nextTick, onMounted, ref } from 'vue'
import '../../styles/layout/filterbar.css'
import { formatNumber } from '@/utils'
import { useHotelStore } from '@/stores/hotel'

const minPrice = 100000
const maxPrice = 20000000
const currentMin = ref(minPrice)
const currentMax = ref(maxPrice)
const sliderWidth = ref(0)

const emit = defineEmits(['filterHotel'])

const ratings = ref([])
const guestRatings = ref([])
const types = ref([])
const selectedHotelFacilities = ref([])

const rangeSlider = ref(null)
const leftThumb = ref(null)
const rightThumb = ref(null)
const range = ref(null)

const hotelStore = useHotelStore()

function filterHotel() {
  emit('filterHotel', {
    MinPrice: currentMin.value != 100000 ? currentMin.value : null,
    MaxPrice: currentMax.value != 20000000 ? currentMax.value : null,
    Ratings: ratings.value.length > 0 ? ratings.value : null,
    GuestRatings: guestRatings.value.length > 0 ? guestRatings.value : null,
    Types: types.value.length > 0 ? types.value : null,
    HotelFacilities: selectedHotelFacilities.value.length > 0 ? selectedHotelFacilities.value : null
  })
}

function updateRange() {
  const leftPercentage = ((currentMin.value - minPrice) / (maxPrice - minPrice)) * 100
  const rightPercentage = ((currentMax.value - minPrice) / (maxPrice - minPrice)) * 100

  range.value.style.left = `${leftPercentage}%`
  range.value.style.right = `${100 - rightPercentage}%`

  leftThumb.value.style.left = `${leftPercentage}%`
  rightThumb.value.style.left = `${rightPercentage}%`
}

onMounted(() => {
  nextTick(() => {
    hotelStore.fetchFacilities(1)
    hotelStore.fetchFacilities(2)
    sliderWidth.value = rangeSlider.value.offsetWidth
    updateRange()
  })
})

function startDrag(thumb) {
  const move = (e) => {
    const rect = rangeSlider.value.getBoundingClientRect()
    const offsetX = Math.min(Math.max(e.clientX - rect.left, 0), sliderWidth.value)
    const value =
      Math.floor((minPrice + (offsetX / sliderWidth.value) * (maxPrice - minPrice)) / 10000) * 10000

    if (thumb === 'left') {
      currentMin.value = Math.min(value, currentMax.value - 10)
    } else {
      currentMax.value = Math.max(value, currentMin.value + 10)
    }

    updateRange()
  }

  const stop = () => {
    document.removeEventListener('mousemove', move)
    document.removeEventListener('mouseup', stop)
  }

  document.addEventListener('mousemove', move)
  document.addEventListener('mouseup', stop)
}

const popularFacilities = computed(() => hotelStore.getPopularFacilities)
const uniqueFacilities = computed(() => hotelStore.getUniqueFacilities)
</script>
