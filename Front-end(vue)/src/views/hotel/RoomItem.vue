<template>
  <div class="content--row content-item hotel-item">
    <div class="item-img">
      <div v-if="room.Image.length > 0" class="item-img--hasimage">
        <img :src="getLinkImage(room.Image[0].Path)" alt="anh ks" class="item-img" />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div v-if="mode == 1">
      <div class="content--column item-info">
        <div class="info-value info__name">{{ room.Name }}</div>
        <div class="content--row">
          <div class="info-value">Số lượng: {{ room.Quantity }}</div>
          <div class="info-value">Diện tích: {{ room.Area }} m²</div>
          <div class="info-value">Giá tiền: {{ formatNumber(room.Price) }} vnd/phòng/đêm</div>
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
    </div>
    <div v-if="mode == 0">
      <div class="content--row item-info">
        <div class="room-section room-overview">
          <div class="info-value info__name">{{ room.Name }}</div>
          <div class="info-value">
            <img src="../../assets/icon/ruler.png" alt="" class="info-icon" />
            {{ room.Area }} m²
          </div>
          <div class="info-value" v-if="room.SingleBed > 0">
            <img src="../../assets/icon/single_bed.png" alt="" class="info-icon" />
            {{ room.SingleBed }} giường đơn
          </div>
          <div class="info-value" v-if="room.DoubleBed > 0">
            <img src="../../assets/icon/double_bed.png" alt="" class="info-icon" />{{
              room.DoubleBed
            }}
            giường đôi
          </div>
          <div class="info-value" v-if="room.MaxAdultPeople > 0">
            <img src="../../assets/icon/adult_people.png" alt="" class="info-icon" />{{
              room.MaxAdultPeople
            }}
            người lớn
          </div>
          <div class="info-value" v-if="room.MaxChildrenPeople > 0">
            <img src="../../assets/icon/children.png" alt="" class="info-icon" />
            {{ room.MaxChildrenPeople }} trẻ em
          </div>
        </div>
        <div class="room-section room-facility">
          <h4 style="margin-bottom: 6px">Các tiện nghi phòng</h4>
          <div class="detail-facility">
            <div v-for="facility in room.RoomFacility" :key="facility.Id" class="facility__value">
              {{ facility.Facility.Name }}
            </div>
          </div>
        </div>
        <div class="room-section room-price">
          <h4 style="margin-bottom: 6px; display: block">Giá phòng / đêm</h4>
          <div>
            <span class="price-value">{{ formatNumber(room.Price) }} VND</span>
            <span class="available-room"> Chỉ còn {{ room.AvailableQuantity }} phòng</span>
          </div>
          <button @click="handleSelectRoom" class="btn btn--add">Đặt phòng</button>
        </div>
      </div>
    </div>
    <div class="item-action" v-if="mode == 1">
      <button @click="handleSelectRoom" class="action-button action--first">Chỉnh sửa</button>
    </div>

    <ImageGallery :images="room.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
</template>

<script setup>
import { formatNumber, getLinkImage } from '@/utils'
import '../../styles/main.css'
import '../../styles/base/button.css'
import { ref } from 'vue'
import ImageGallery from '@/components/ImageGallery.vue'

const showImagePopup = ref(false)
const showAllFacilities = ref(false)

const emit = defineEmits(['selectRoom'])

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

function handleSelectRoom() {
  emit('selectRoom')
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

<style scoped>
.hotel-item:hover {
  border: 1px solid #078cf8;
}
</style>
