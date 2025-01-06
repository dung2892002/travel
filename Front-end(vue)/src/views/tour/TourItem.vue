<template>
  <div class="content--row content-item tour-item" style="height: 200px">
    <div class="item-img">
      <div v-if="tour.Image.length > 0" class="item-img--hasimage">
        <img :src="getLinkImage(tour.Image[0].Path)" alt="anh tour" class="item-img" />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div class="content--column item-info" @click="handleSelectTour">
      <div
        class="content--row"
        style="display: flex; flex-direction: row; justify-content: space-between"
      >
        <span class="info-value info__name" style="width: 420px">
          {{ tour.Name }}
        </span>
        <div class="info-value info__review">
          <span v-if="tour.QuantityReview > 0"> {{ tour.AverageReview }}</span>
          <span v-if="tour.QuantityReview > 0"> ({{ tour.QuantityReview }} đánh giá)</span>
        </div>
        <div class="info-value info__review" v-if="mode == 3" style="position: absolute; right: 0">
          <button
            @click="handleSelectTour"
            class="action-button action--first"
            style="width: 64px; height: 32px"
          >
            Xóa
          </button>
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
        <span>Giá khách/tour từ</span>
        <span>{{ formatNumber(tour.MinPrice) }} VND</span>
      </div>
    </div>
    <div class="item-action" v-if="mode === 1">
      <button @click="handleSelectTour" class="action-button action--first">Chỉnh sửa</button>
      <button @click="showSchedule" class="action-button action--first">Lịch khởi hành</button>
    </div>

    <ImageGallery :images="tour.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
</template>

<script setup>
import { formatNumber, getLinkImage } from '@/utils'
import '../../styles/main.css'
import { ref } from 'vue'
import ImageGallery from '@/components/ImageGallery.vue'
import { useRouter } from 'vue-router'

const showImagePopup = ref(false)
const router = useRouter()
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

function showSchedule() {
  router.push({
    name: 'ScheduleList',
    params: {
      tourId: props.tour.Id
    }
  })
}

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

<style scoped>
.tour-item:hover {
  border: 1px solid #078cf8;
}
</style>
