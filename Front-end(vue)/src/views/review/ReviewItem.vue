<template>
  <div class="content-item content--row review-item">
    <div class="review-value review__user">
      <img :src="getLinkImage(review.User.AvatarImage)" alt="" />
      <span>{{ review.User.DisplayName }}</span>
    </div>
    <div class="review-value content--column">
      <div class="review-section">
        <div class="review__point">
          <span>{{ review.Point }}</span>
          <span> / 10</span>
        </div>
        <div class="review__date">{{ formatDate(review.CreatedAt) }}</div>
      </div>
      <div class="review__description">{{ review.Description }}</div>
      <div class="review-section review__image">
        <img
          v-for="image in review.Image.slice(0, 7)"
          :key="image.Id"
          :src="getLinkImage(image.Path)"
          alt="anh review"
          @click="toggleImagePopup"
        />
      </div>
    </div>
  </div>
  <ImageGallery :images="review.Image" :visible="showImagePopup" @close="toggleImagePopup" />
</template>
<script setup>
import ImageGallery from '@/components/ImageGallery.vue'
import { formatDate, getLinkImage } from '@/utils'
import { ref } from 'vue'

const showImagePopup = ref(false)

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

// eslint-disable-next-line no-unused-vars
const props = defineProps({
  review: {
    type: Object,
    required: true
  }
})
</script>
