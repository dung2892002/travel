<template>
  <div class="content--row content-item hotel-item">
    <div class="item-img">
      <div v-if="hotel.Image.length > 0" class="item-img--hasimage">
        <img :src="getLinkImage(hotel.Image[0].Path)" alt="anh ks" class="item-img" />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div class="content--column item-info" @click="handleSelectHotel">
      <div
        class="content--row"
        style="display: flex; flex-direction: row; justify-content: space-between"
      >
        <div class="info-value info__name">{{ hotel.Name }}</div>
        <div class="info-value info__review">
          <span v-if="hotel.QuantityReview > 0"> {{ hotel.AverageReview }}</span>
          <span v-if="hotel.QuantityReview > 0"> ({{ hotel.QuantityReview }} đánh giá)</span>
        </div>
        <div class="info-value info__review" v-if="mode == 3" style="position: absolute; right: 0">
          <button
            @click="handleSelectHotel"
            class="action-button action--first"
            style="width: 64px; height: 32px"
          >
            Xóa
          </button>
        </div>
      </div>
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
        <img src="../../assets/icon/location.png" alt="icon" class="info__icon" /><span
          v-if="!hotel.City"
          >{{ hotel.CityName }}, {{ hotel.ProvinceName }}</span
        >
        <span v-else>{{ hotel.City.Name }}, {{ hotel.City.Province.Name }}</span>
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
      <div v-if="mode === 0" class="info-value info__room">
        <span>Giá mỗi phòng / đêm chỉ từ</span>
        <span>{{ formatNumber(hotel.MinPrice) }} VND</span>
      </div>
    </div>
    <div class="item-action" v-if="mode == 1">
      <button @click="handleSelectHotel" class="action-button action--first">Chỉnh sửa</button>
      <button @click="showRoom" class="action-button">Xem phòng</button>
    </div>

    <ImageGallery :images="hotel.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
</template>

<script setup>
import { formatNumber, getLinkImage } from '@/utils'
import '../../styles/main.css'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import ImageGallery from '@/components/ImageGallery.vue'

const showImagePopup = ref(false)
const showAllFacilities = ref(false)

const props = defineProps({
  hotel: {
    type: Object,
    required: true
  },
  mode: {
    type: Number,
    required: true
  }
})

const router = useRouter()

const emit = defineEmits(['selectHotel'])

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

function handleSelectHotel() {
  emit('selectHotel')
}

function showRoom() {
  router.push({
    name: 'RoomsList',
    params: {
      hotelId: props.hotel.Id
    }
  })
}

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

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}
</script>

<style scoped>
.hotel-item:hover {
  border: 1px solid #078cf8;
}
</style>
