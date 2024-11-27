<template>
  <div v-if="reviews">
    <div class="review-overall">
      <div>
        <h3>Đánh giá và nhận xét chung</h3>
        <span style="font-size: 14px; color: gray"
          >Từ <span style="font-weight: bold">{{ totalItems }}</span> đánh giá của khách hàng</span
        >
      </div>
      <div class="content--row" style="margin-top: 10px; width: 100%">
        <div class="average__review">
          <span>{{ overallReview.Average }}</span>
        </div>
        <div class="content--column" style="width: 60%">
          <div class="review-bar" v-for="(label, index) in reviewLabels" :key="index">
            <div class="review-bar__label">{{ label }}</div>
            <div class="review-bar-progress">
              <div class="progress" :style="{ width: getPercentage(index) + '%' }"></div>
            </div>
            <div class="percentage">{{ getQuantity(index) }}</div>
          </div>
        </div>
      </div>
    </div>
    <div class="form-review" v-if="user">
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <div
            v-for="value in [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]"
            :key="value"
            @click="changePoint(value)"
          >
            <span v-if="value === reviewForm.point" class="point point--selected">
              {{ value }}</span
            >
            <span v-else class="point"> {{ value }}</span>
          </div>
        </div>
        <div class="form-group">
          <textarea id="description" v-model="reviewForm.description" rows="4"></textarea>
        </div>
        <div class="form-group">
          <label for="files" class="upload-button">Chọn ảnh</label>
          <input type="file" id="files" multiple accept="image/*" @change="handleFileUpload" />
          <button type="submit" class="submit-button">
            <img src="../../assets/icon/send.png" alt="send" />
          </button>
        </div>
      </form>

      <div class="preview">
        <div v-for="(file, index) in previewImage" :key="index" class="image-preview">
          <img :src="file.preview" alt="Image preview" />
        </div>
      </div>
      <div v-if="sending" class="sending">Sending...</div>
    </div>
    <div v-else>
      <RouterLink :to="{ name: 'login' }">
        <span style="font-size: 14px; color: #0194f3"
          >Đăng nhập để có thể đánh giá khách sạn</span
        ></RouterLink
      >
    </div>
    <ReviewItem v-for="review in reviews" :key="review.Id" :review="review" />
    <ThePagnigation
      :pageNumber="pageNumber"
      :totalItems="totalItems"
      :totalPages="totalPages"
      @pageChange="handlePageChange"
    />
  </div>
</template>

<script setup>
import { useReviewStore } from '@/stores/review'
import { computed, onMounted, ref } from 'vue'
import ReviewItem from './ReviewItem.vue'
import ThePagnigation from '@/components/ThePagnigation.vue'
import { useUserStore } from '@/stores/user'

const reviewStore = useReviewStore()
const userStore = useUserStore()

const pageNumber = ref(1)

const sending = ref(false)
const reviewForm = ref({
  point: 1,
  description: '',
  files: []
})

const previewImage = ref([])

function changePoint(value) {
  reviewForm.value.point = value
}

function handleFileUpload(event) {
  const files = Array.from(event.target.files)
  reviewForm.value.files = files
  previewImage.value = files.map((file) => ({
    file,
    preview: URL.createObjectURL(file)
  }))
}

async function handleSubmit() {
  sending.value = true
  const formData = new FormData()
  formData.append('UserId', user.value.Id)
  formData.append('Point', reviewForm.value.point)
  formData.append('Description', reviewForm.value.description)
  if (props.hotelId) formData.append('HotelId', props.hotelId)
  if (props.tourId) formData.append('TourId', props.tourId)

  reviewForm.value.files.forEach((file) => {
    formData.append('files', file)
  })

  const response = await reviewStore.createReview(formData, token.value)
  sending.value = false
  if (response.success) {
    reviewForm.value.point = 1
    reviewForm.value.description = ''
    reviewForm.value.files = []
    pageNumber.value = 1
    fetchReviews()
  } else {
    console.log(response.message)
  }
}

const props = defineProps({
  hotelId: {
    type: String,
    required: false
  },
  tourId: {
    type: String,
    required: false
  }
})

const reviewLabels = ['Tuyệt vời', 'Rất tốt', 'Hài lòng', 'Trung bình', 'Kém']

function getPercentage(index) {
  const quantities = [
    overallReview.value.QuantityFantastic,
    overallReview.value.QuantityVeryGood,
    overallReview.value.QuantitySatisfying,
    overallReview.value.QuantityAverage,
    overallReview.value.QuantityPoor
  ]
  return totalItems.value === 0 ? 0 : ((quantities[index] / totalItems.value) * 100).toFixed(1)
}

function getQuantity(index) {
  const quantities = [
    overallReview.value.QuantityFantastic,
    overallReview.value.QuantityVeryGood,
    overallReview.value.QuantitySatisfying,
    overallReview.value.QuantityAverage,
    overallReview.value.QuantityPoor
  ]
  return quantities[index]
}

function handlePageChange(value) {
  pageNumber.value = value
  fetchReviews()
}
async function fetchReviews() {
  if (props.hotelId) {
    await reviewStore.fetchHotelReviews(props.hotelId, pageNumber.value)
  }
}

const reviews = computed(() => {
  if (props.hotelId) return reviewStore.getHotelReviews
  else return reviewStore.getTotalItems
})

const totalPages = computed(() => reviewStore.getTotalPages)
const totalItems = computed(() => reviewStore.getTotalItems)
const overallReview = computed(() => reviewStore.getOverallReview)

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

onMounted(() => {
  fetchReviews()
})
</script>

<style scoped>
.form-review {
  display: flex;
  flex-direction: row;
  align-items: center;
  position: relative;
  width: 100%;
}
.point {
  width: 32px;
  height: 32px;
  color: white;
  background-color: #9cdafe;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  border-radius: 12px;
  margin-right: 8px;
}

.point--selected {
  background-color: #0194f3;
  font-weight: bold;
}

#description {
  background-color: #f5f5f5;
  height: 120px;
  padding: 5px;
  line-height: 20px;
  font-size: 14px;
  width: 100%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

input[type='file'] {
  display: none;
}
.sending {
  width: 100%;
  height: 100%;
  position: absolute;
  background-color: rgba(0, 0, 0, 0.5);
  top: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #ffffff;
  border-radius: 6px;
}

.upload-button,
.submit-button {
  display: inline-block;
  background-color: #007bff;
  color: white;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 14px;
  font-weight: bold;
  text-align: center;
  transition: background-color 0.3s ease;
  border: none;
}

/* Hiệu ứng hover */
.upload-button:hover,
.submit-button:hover {
  background-color: #0056b3;
}

.submit-button {
  padding: 0 20px;
}

.submit-button img {
  width: 24px;
  height: 24px;
  filter: invert(1);
}

.preview {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 8px;
  height: auto;
  margin-left: 40px;
  max-height: 200px;
  overflow-y: scroll;
}

.image-preview img {
  width: 96px;
  height: 96px;
  border-radius: 6px;
  margin-right: 0;
}
</style>
