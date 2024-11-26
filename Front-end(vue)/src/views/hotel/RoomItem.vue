<template>
  <div class="content--row content-item">
    <div class="item-img">
      <div v-if="room.Image.length > 0" class="item-img--hasimage">
        <img :src="getLinkImage(room.Image[0].Path)" alt="anh ks" class="item-img" />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div class="content--column item-info">
      <div class="info-value info__name">{{ room.Name }}</div>
      <div class="content--row">
        <div class="info-value">Số lượng: {{ room.Quantity }}</div>
        <div class="info-value">Diện tích: {{ room.Area }} m²</div>
      </div>
      <div class="content--row">
        <div class="info-value">Người lớn: {{ room.MaxAdultPeople }}</div>
        <div class="info-value">Trẻ em: {{ room.MaxChildrenPeople }}</div>
      </div>
      <div class="content--row">
        <div class="info-value">Giường đơn: {{ room.SingleBed }}</div>
        <div class="info-value">Giường đôi:{{ room.DoubleBed }}</div>
      </div>
      <div class="info-value">Hướng: {{ room.Dirention }}</div>
      <div class="info-value">
        <div
          v-for="facility in room.RoomFacility.slice(0, 5)"
          :key="facility.Id"
          class="info__facility"
        >
          {{ facility.Facility.Name }}
        </div>
        <div
          v-if="room.RoomFacility.length > 5"
          class="info__facility info__facility--more"
          @mouseover="showAllFacilities = true"
          @mouseleave="showAllFacilities = false"
        >
          +{{ room.RoomFacility.length - 5 }}
          <div v-if="showAllFacilities" class="all-facilities-popup">
            <div
              v-for="facility in room.RoomFacility.slice(5)"
              :key="facility.Id"
              class="popup-facility"
            >
              {{ facility.Facility.Name }}
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="item-action" v-if="mode == 1">
      <button @click="handleUpdateRoom" class="action-button action--first">Chỉnh sửa</button>
    </div>

    <ImageGallery :images="room.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
</template>

<script setup>
import { getLinkImage } from '@/utils'
import '../../styles/main.css'
import { ref } from 'vue'
import ImageGallery from '@/components/ImageGallery.vue'

const showImagePopup = ref(false)
const showAllFacilities = ref(false)

const emit = defineEmits(['updateRoom'])

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

function handleUpdateRoom() {
  emit('updateRoom')
}
// eslint-disable-next-line no-unused-vars
const props = defineProps({
  room: {
    type: Object,
    required: true
  },
  mode: {
    type: Number,
    required: true
  }
})
</script>
