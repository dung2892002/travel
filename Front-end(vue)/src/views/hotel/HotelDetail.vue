<template>
  <div class="content--column content-detail" v-if="hotel">
    <div class="detail-section detail-img">
      <div v-if="hotel.Image.length > 0" class="detail-img--hasimage">
        <img
          :src="getLinkImage(hotel.Image[0].Path)"
          alt="anh ks"
          class="detail-image detail-image--first"
          @click="toggleImagePopup"
        />
        <div class="list-detail-image">
          <img
            v-for="image in hotel.Image.slice(1, 7)"
            :key="image.Id"
            :src="getLinkImage(image.Path)"
            alt="anh ks"
            class="detail-image"
            @click="toggleImagePopup"
          />
        </div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="detail-img" />
    </div>
    <div class="detail-section detail-overview">
      <div class="detail-item detail--name">{{ hotel.Name }}</div>
      <div class="detail-item content--row">
        <div class="detail--type">{{ getTypeHotel(hotel.Type) }}</div>
        <div class="detail--rating">{{ renderRating(hotel.Rating) }}</div>
      </div>
      <div class="content--row">
        <div class="detail-item detail--policy">
          <span class="detail-item__label">Chính sách của khách sạn</span>
          <div class="policy__value">
            <span>Thời gian nhận phòng</span>
            <span> Sau {{ hotel.CheckInTime }}</span>
          </div>
          <div class="policy__value">
            <span>Thời gian trả phòng</span>
            <span> Trước {{ hotel.CheckOutTime }}</span>
          </div>
        </div>
        <div class="detail-item detail--destination">
          <span class="detail-item__label">Các điểm đến lân cận</span>
          <div>
            <div
              v-for="destination in hotel.HotelDestination.slice(0, 6)"
              :key="destination.Id"
              class="destination__value"
            >
              <img src="../../assets/icon/location.png" alt="" />
              {{ destination.Destination.Name }}
            </div>
            <div
              v-if="hotel.HotelDestination.length > 6"
              class="detail__destination detail__destination--more"
              @mouseover="showAllDestinations = true"
              @mouseleave="showAllDestinations = false"
            >
              +{{ hotel.HotelDestination.length - 6 }}
              <div v-if="showAllDestinations" class="detail-destinations-popup">
                <div
                  v-for="destination in hotel.HotelDestination.slice(6)"
                  :key="destination.Id"
                  class="popup-facility"
                >
                  <div class="destination__value">
                    <img src="../../assets/icon/location.png" alt="" />
                    {{ destination.Destination.Name }}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="detail-item detail--facility">
          <span class="detail-item__label">Các tiện nghi khách sạn</span>
          <div class="detail-facility">
            <div
              v-for="facility in hotel.HotelFacility.slice(0, 11)"
              :key="facility.Id"
              class="facility__value"
            >
              {{ facility.Facility.Name }}
            </div>
            <div
              v-if="hotel.HotelFacility.length > 11"
              class="detail__facility detail__facility--more"
              @mouseover="showAllFacilities = true"
              @mouseleave="showAllFacilities = false"
            >
              +{{ hotel.HotelFacility.length - 11 }}
              <div v-if="showAllFacilities" class="detail-facilities-popup">
                <div
                  v-for="facility in hotel.HotelFacility.slice(11)"
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
      <div class="detail-item detail--description" v-html="formatText(hotel.Description)"></div>
    </div>
    <div class="detail-section detail-room">
      <h2>Các phòng còn trống ở khách sạn</h2>
      <RoomItem
        v-for="room in rooms"
        :room="room"
        :mode="0"
        :key="room.Id"
        @selectRoom="handleSelectRoom(room.Id)"
      ></RoomItem>
    </div>
    <div class="detail-section detail-review">
      <h2>Các đánh giá từ khách hàng với {{ hotel.Name }}</h2>
      <ListReview :hotelId="id" />
    </div>

    <ImageGallery :images="hotel.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
  <div v-else>chua co du lieu khach san</div>
</template>

<script setup>
import ImageGallery from '@/components/ImageGallery.vue'
import { formatText, getLinkImage } from '@/utils'
import { computed, onMounted, ref } from 'vue'
import RoomItem from './RoomItem.vue'
import { useHotelStore } from '@/stores/hotel'
import { useRoute } from 'vue-router'
import { useReviewStore } from '@/stores/review'
import ListReview from '../review/ListReview.vue'

const hotelStore = useHotelStore()
const reviewStore = useReviewStore()

const route = useRoute()

const id = ref(route.params.id)
const showAllFacilities = ref(false)
const showAllDestinations = ref(false)
const showImagePopup = ref(false)

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
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

function handleSelectRoom(id) {
  console.log(id)
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

const queryRoom = JSON.parse(sessionStorage.getItem('queryRoom'))

onMounted(() => {
  if (!id.value) {
    console.error('ID is missing in route params.')
    return
  }
  hotelStore.fetchHotel(id.value)
  reviewStore.fetchHotelReviews(id.value)
  hotelStore.fetchSearchRooms(queryRoom)
})

const hotel = computed(() => hotelStore.getHotel)
const rooms = computed(() => hotelStore.getSearchRooms)
</script>
