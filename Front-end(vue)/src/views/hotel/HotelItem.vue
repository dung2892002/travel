<template>
  <div class="content--row content-item">
    <div class="item-img">
      <img :src="getLinkImage(hotel.Image[0].Path)" alt="anh ks" v-if="hotel.Image.length > 0" />
      <img src="../../assets/image/hotel.jpg" alt="" v-else />
    </div>
    <div class="content--column item-info">
      <div class="info-value info__name">{{ hotel.Name }}</div>
      <div class="content--row">
        <div class="info-value info__type">
          <img src="../../assets/icon/hotel.png" alt="icon" class="info__icon" />
          {{ getTypeHotel(hotel.Type) }}
        </div>
        <div class="info-value info__rating">
          {{ renderRating(hotel.Rating) }}
        </div>
      </div>
      <div class="info-value info__location">
        <img src="../../assets/icon/location.png" alt="icon" class="info__icon" />{{
          hotel.City.Name
        }}, {{ hotel.City.Province.Name }}
      </div>
      <div class="info-value">
        <div
          v-for="facility in hotel.HotelFacility.slice(0, 5)"
          :key="facility.Id"
          class="info__facility"
        >
          {{ facility.Facility.Name }}
        </div>
        <div
          v-if="hotel.HotelFacility.length > 5"
          class="info__facility info__facility--more"
          @mouseover="showAllFacilities = true"
          @mouseleave="showAllFacilities = false"
        >
          +{{ hotel.HotelFacility.length - 5 }}
          <div v-if="showAllFacilities" class="all-facilities-popup">
            <div
              v-for="facility in hotel.HotelFacility.slice(5)"
              :key="facility.Id"
              class="popup-facility"
            >
              {{ facility.Facility.Name }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getLinkImage } from '@/utils'
import '../../styles/main.css'
import { ref } from 'vue'

const showAllFacilities = ref(false)

function getTypeHotel(type) {
  const types = {
    1: 'Khu nghỉ dưỡng (Resort)',
    2: 'Biệt thự (Villa)',
    3: 'Khách sạn (Hotel)',
    4: 'Căn hộ (Apartment)',
    5: 'Nhà nghỉ (Guest house)'
  }

  return types[type]
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  // Kết hợp số sao đầy và sao rỗng
  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

// eslint-disable-next-line no-unused-vars
const props = defineProps({
  hotel: {
    type: Object,
    required: true
  }
})
</script>
