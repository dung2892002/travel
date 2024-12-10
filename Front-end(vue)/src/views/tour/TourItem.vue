<template>
  <div class="content--row content-item">
    <div class="item-img">
      <div v-if="tour.Image.length > 0" class="item-img--hasimage">
        <img :src="getLinkImage(tour.Image[0].Path)" alt="anh tour" class="item-img" />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div class="content--column item-info" @click="handleSelectTour">
      <div class="content--row">
        <div class="info-value info__name">{{ tour.Name }}</div>
        <div class="info-value info__review">
          <span v-if="tour.QuantityReview > 0"> {{ tour.AverageReview }}</span>
          <span v-if="tour.QuantityReview > 0"> ({{ tour.QuantityReview }} đánh giá)</span>
        </div>
      </div>
      <div class="content--row">
        <div class="info-value info__rating">
          {{ renderRating(tour.Rating) }}
        </div>
      </div>
      <div class="info-value info__location">
        <span style="margin-right: 4px">Khởi hành từ: </span
        ><span v-if="!tour.DepartureCity"> {{ tour.CityName }}, {{ tour.ProvinceName }}</span>
        <span v-else>{{ tour.DepartureCity.Name }}, {{ tour.DepartureCity.Province.Name }}</span>
      </div>
      <div class="info-value info__location">
        <span>Phương tiện di chuyển: {{ tour.Transport }}</span>
      </div>
      <div class="info-value info__location">
        <span>Thời gian: {{ tour.NumberOfDay }} ngày, {{ tour.NumberOfNight }} đêm</span>
      </div>
      <div v-if="mode === 0" class="info-value info__room">
        <span>Giá tour</span>
        <span>{{ formatNumber(hotel.MinPrice) }} VND</span>
      </div>
    </div>
    <div class="item-action" v-if="mode === 1">
      <button @click="handleSelectTour" class="action-button action--first">Chỉnh sửa</button>
    </div>
    <div class="item-action" v-if="mode === 3">
      <button @click="handleSelectTour" class="action-button action--first">Xóa</button>
    </div>

    <ImageGallery :images="tour.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
</template>

<script setup>
import { formatNumber, getLinkImage } from '@/utils'
import '../../styles/main.css'
import { ref } from 'vue'
import ImageGallery from '@/components/ImageGallery.vue'

const showImagePopup = ref(false)

// eslint-disable-next-line no-unused-vars
const props = defineProps({
  tour: {
    type: Object,
    required: true
  },
  mode: {
    type: Number,
    required: true
  }
})

const emit = defineEmits(['selectTour'])

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

function handleSelectTour() {
  emit('selectTour')
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}
</script>
