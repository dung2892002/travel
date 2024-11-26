<template>
  <div v-if="visible" class="image-gallery-popup">
    <div class="popup-overlay" @click="closePopup"></div>
    <div class="popup-content">
      <button class="popup-close" @click="closePopup">
        <img src="../assets/icon/close-48.png" alt="" />
      </button>
      <div class="popup-images">
        <div class="primary-image">
          <button @click="beforeImage" class="popup-action">
            <img src="../assets/icon/btn-prev-page.svg" alt="" />
          </button>
          <img :src="getLinkImage(images[index].Path)" alt="Gallery Image" class="popup-image" />
          <button @click="nextImage" class="popup-action">
            <img src="../assets/icon/btn-next-page.svg" alt="" />
          </button>
        </div>
        <div class="index">{{ index + 1 }} / {{ images.length }}</div>
        <div class="list-image">
          <img
            v-for="(image, index) in images"
            :key="index"
            :src="getLinkImage(image.Path)"
            @click="selectImage(index)"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getLinkImage } from '@/utils'
import { defineProps, defineEmits, ref } from 'vue'
import '../styles/layout/image-gallery.css'

const index = ref(0)

function nextImage() {
  if (index.value < props.images.length) index.value = index.value + 1
}

function beforeImage() {
  if (index.value > 0) index.value--
}

function selectImage(value) {
  index.value = value
}

const props = defineProps({
  images: {
    type: Array,
    required: true
  },
  visible: {
    type: Boolean,
    required: true
  }
})

const emit = defineEmits(['close'])

function closePopup() {
  emit('close')
}
</script>
