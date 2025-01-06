<template>
  <div class="content-item content--row review-item">
    <div class="review-value review__user">
      <img :src="getLinkImage(review.User.AvatarImage)" alt="" />
      <span>{{ review.User.DisplayName }}</span>
    </div>
    <div class="review-value content--column">
      <div class="review-section">
        <div class="review__point">
          <div>
            <span>{{ review.Point }}</span> <span> / 10</span>
          </div>
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

      <button
        v-if="user && review.UserId === user.Id"
        style="background-color: red; width: 64px; padding: 4px; border-radius: 4px"
        @click="deleteReview(review)"
        v-loading="deleteLoading"
      >
        Xóa
      </button>
    </div>
  </div>
  <ImageGallery :images="review.Image" :visible="showImagePopup" @close="toggleImagePopup" />
</template>
<script setup>
import ImageGallery from '@/components/ImageGallery.vue'
import { useReviewStore } from '@/stores/review'
import { useUserStore } from '@/stores/user'
import { formatDate, getLinkImage } from '@/utils'
import { computed, ref } from 'vue'

const deleteLoading = ref(false)

const showImagePopup = ref(false)

const userStore = useUserStore()
const reviewStore = useReviewStore()
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

const emits = defineEmits(['delete'])

async function deleteReview(review) {
  deleteLoading.value = true
  await reviewStore.deleteReview(review.Id, user.value.Id, token.value)
  deleteLoading.value = false
  emits('delete')
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
</script>
