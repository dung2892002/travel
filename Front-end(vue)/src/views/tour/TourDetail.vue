<template>
  <div class="content--column content-detail" v-if="tour">
    <div class="detail-section detail-img">
      <div v-if="tour.Image.length > 0" class="detail-img--hasimage">
        <img
          :src="getLinkImage(tour.Image[0].Path)"
          alt="anh ks"
          class="detail-image detail-image--first"
          @click="toggleImagePopup"
        />
        <div class="list-detail-image">
          <img
            v-for="image in tour.Image.slice(1, 7)"
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
      <div class="detail-item detail--name">
        {{ tour.Name }}:
        <span
          v-for="city in tour.TourCity.slice(0, tour.TourCity.length - 1)"
          :key="city.Id"
          style="margin-left: 4px"
          >{{ city.City.Name }} -</span
        >
        <span style="margin-left: 4px">{{
          tour.TourCity[tour.TourCity.length - 1].City.Name
        }}</span>
      </div>
      <div class="detail-item detail--rating">{{ renderRating(tour.Rating) }}</div>
      <div class="detail-item detail--time">
        Thời gian: {{ tour.NumberOfDay }} ngày,
        <span v-if="tour.NumberOfNight > 0">{{ tour.NumberOfNight }} đêm</span>
      </div>
      <div class="detail-item detail--transport">Phương tiện di chuyển: {{ tour.Transport }}</div>
      <div class="detail-item detail--description" v-html="formatText(tour.Description)"></div>
    </div>
    <div class="detail-section detail-overview">
      <h2 style="margin-bottom: 6px">Lịch trình tour</h2>
      <div v-for="tourDay in tour.TourDay" :key="tourDay.Id">
        <TourDay :tourDay="tourDay"></TourDay>
      </div>
    </div>
    <div class="detail-section">
      <h2>Lịch khởi hành và giá tour</h2>

      <div
        v-for="schedule in schedules"
        :key="schedule.Id"
        style="
          height: 42px;
          display: flex;
          align-items: center;
          justify-content: space-between;
          border: 1px solid gainsboro;
          padding: 0 10px;
        "
      >
        <span style="margin-right: 4px">Ngày khởi hành:{{ formatDate(schedule.DateStart) }}.</span>
        <span>Giá: {{ formatNumber(schedule.Price) }} vnd/khách</span>
        <button class="btn btn--add" @click="handleSelectSchedule(schedule)">Đặt tour</button>
      </div>
    </div>
    <div class="detail-section detail-overview">
      <h2>Thông tin cần chú ý</h2>
    </div>
    <div class="detail-section detail-review">
      <div
        style="
          display: flex;
          flex-direction: row;
          justify-content: space-between;
          align-items: center;
        "
      >
        <h2>Các đánh giá từ khách hàng với {{ tour.Name }}</h2>
        <div style="display: flex; flex-direction: row; align-items: center">
          <div
            style="
              display: flex;
              flex-direction: column;
              justify-content: center;
              align-items: center;
              margin-right: 10px;
            "
          >
            <img src="../../assets/icon/love.png" alt="" style="width: 24px; height: 24px" />
            <span style="font-size: 14px; font-weight: 400; color: #808080">{{
              quantityFavourites
            }}</span>
          </div>
          <div v-if="user && token">
            <button v-if="userFavourite" @click="deleteTourFavourite" class="btn btn--add">
              Xóa
            </button>
            <button v-else @click="addTourFavourite" class="btn btn--remove">Thêm</button>
          </div>
        </div>
      </div>
      <ListReview :tourId="id" />
    </div>

    <ImageGallery :images="tour.Image" :visible="showImagePopup" @close="toggleImagePopup" />
  </div>
  <div v-else>chua co du lieu tour</div>
</template>

<script setup>
import ImageGallery from '@/components/ImageGallery.vue'
import { formatDate, formatNumber, formatText, getLinkImage } from '@/utils'
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useReviewStore } from '@/stores/review'
import ListReview from '../review/ListReview.vue'
import { useFavouriteStore } from '@/stores/favourite'
import { useUserStore } from '@/stores/user'
import { useTourStore } from '@/stores/tour'
import TourDay from './TourDay.vue'

const tourStore = useTourStore()
const reviewStore = useReviewStore()
const favouriteStore = useFavouriteStore()
const userStore = useUserStore()

const route = useRoute()
const router = useRouter()

const id = ref(route.params.id)
const showImagePopup = ref(false)

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}

async function handleSelectSchedule(schedule) {
  const bookingTour = {
    Schedule: schedule,
    TourId: tour.value.Id,
    TourName: tour.value.Name,
    TourRating: tour.value.Rating,
    TourImage: tour.value.Image[0],
    ContactName: user.value.Fullname,
    ContactEmail: user.value.Email,
    ContactPhone: user.value.PhoneNumber,
    Price: 0,
    DiscountValue: 0
  }

  const bookingTourPeople = tour.value.TourPrice.map((price) => ({
    QuantityPeople: 0,
    TourPriceId: price.Id,
    AgeStart: price.AgeStart,
    AgeEnd: price.AgeEnd,
    Percent: price.Percent
  }))

  sessionStorage.setItem('bookingTour', JSON.stringify(bookingTour))
  sessionStorage.setItem('bookingTourPeople', JSON.stringify(bookingTourPeople))

  router.push({
    name: 'createBookingTour'
  })
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

// async function addTourFavourite() {
//   const favourite = {
//     UserId: user.value.Id,
//     HotelId: null,
//     TourId: id.value,
//     CityId: null,
//     DestinationId: null
//   }
//   await favouriteStore.createFavourite(favourite, token.value)

//   await fetchFavourites()
// }

// async function deleteTourFavourite() {
//   await favouriteStore.deleteFavourite(userFavourite.value.Id, token.value)

//   await fetchFavourites()
// }

// async function fetchFavourites() {
//   await favouriteStore.fetchHotelFavourites(id.value)
//   if (user.value && token.value) {
//     await favouriteStore.fetchUserFavouriteHotel(user.value.Id, token.value, id.value)
//   }
// }

const querySchedule = JSON.parse(sessionStorage.getItem('querySchedule'))
console.log(querySchedule)

const tour = computed(() => tourStore.getTour)
const schedules = computed(() => tourStore.getSearchSchedules)
const quantityFavourites = computed(() => favouriteStore.getHotelFavourites)
const token = computed(() => userStore.getToken)
const user = computed(() => userStore.getUser)
const userFavourite = computed(() => favouriteStore.getUserFavouriteHotel)

onMounted(() => {
  if (!id.value) {
    console.error('ID is missing in route params.')
    return
  }
  tourStore.fetchTour(id.value)
  reviewStore.fetchTourReviews(id.value)
  tourStore.fetchSearchSchedule(querySchedule)
  //   fetchFavourites()
})
</script>
