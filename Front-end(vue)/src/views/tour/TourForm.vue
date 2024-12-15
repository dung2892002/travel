<template>
  <div class="form-container" id="hotelForm">
    <div class="form__content">
      <div class="form__header">
        <h2 class="form__title">Thông tin tour</h2>
        <span class="error" v-if="errorMessage.length > 0"> {{ errorMessage }}</span>
        <button class="form__button" @click="closeForm">
          <img src="/src/assets/icon/close-48.png" alt="logo" />
        </button>
      </div>
      <div class="content--row" v-if="tour">
        <div class="travel-form">
          <div class="form-group">
            <div class="form__item form__item--5">
              <p class="form__label">Tên tour <span class="required">*</span></p>
              <input type="text" class="form__input" v-model="tour.Name" required />
            </div>
            <div class="form__item form__item--5">
              <p class="form__label">Thời gian <span class="required">*</span></p>
              <span style="display: flex; flex-direction: row; align-items: center"
                ><input
                  type="number"
                  class="form__input"
                  v-model="tour.NumberOfDay"
                  required
                  style="width: 52px; margin-right: 4px"
                />
                ngày,<input
                  type="number"
                  class="form__input"
                  v-model="tour.NumberOfNight"
                  required
                  style="width: 52px; margin-right: 4px"
                />
                đêm</span
              >
            </div>
            <div class="form__item form__item--5">
              <p class="form__label">Đánh giá<span class="required">*</span></p>
              <div class="stars">
                <span
                  v-for="star in 5"
                  :key="star"
                  class="star"
                  :class="{ 'star--filled': tour.Rating >= star }"
                  @click="setRating(star)"
                >
                  ★
                </span>
              </div>
            </div>
            <div
              class="form__item form__item--5 content--column form-location"
              ref="searchLocationRef"
            >
              <p class="form__label">Thành phố khởi hành</p>
              <input
                type="text"
                class="form__input"
                v-model="tour.DepartureCity.Name"
                placeholder="Địa điểm khách sạn"
                @focus="(showSelectLocation = 1), (showSelectDestination = 0)"
              />
              <div
                v-if="showSelectLocation === 1"
                class="form-show-select"
                style="width: 100%; left: 0"
              >
                <input type="text" v-model="keyword" placeholder="Nhập để tìm kiếm..." />
                <ul v-if="citiesFilter.length > 0">
                  <li
                    v-for="city in citiesFilter"
                    :key="city.Id"
                    @click="selectDepartureCity(city)"
                    class="location-item"
                  >
                    <img :src="getLinkImage(city.Image)" alt="image" class="location--image" />
                    <div>
                      <p class="location--name">{{ city.Name }}</p>
                      <p class="location--name-province">{{ city.Province.Name }}</p>
                    </div>
                    <p class="location--type">Thành phố</p>
                  </li>
                </ul>
              </div>
            </div>
            <div class="form__item form__item--5">
              <p class="form__label">Phương tiện di chuyển<span class="required">*</span></p>
              <input type="text" class="form__input" v-model="tour.Transport" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--1">
              <p class="form__label">Mô tả<span class="required">*</span></p>
              <textarea
                type="text"
                class="form__input"
                rows="5"
                v-model="tour.Description"
                placeholder="Nhập mô tả"
                required
              ></textarea>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--1">
              <div class="group-header">
                <p class="form__label">Lộ trình<span class="required">*</span></p>
                <button class="btn btn--add" @click="addTourCity">Thêm</button>
              </div>

              <div>
                <div v-for="(city, index) in tour.TourCity" :key="index" style="margin-bottom: 4px">
                  <span style="margin-right: 60px; position: relative"
                    >Thành phố:
                    <input
                      type="text"
                      class="form__input"
                      v-model="city.City.Name"
                      disabled
                      style="margin-right: 4px"
                    />
                    <button @click="showSelectTourCity = index" class="btn btn--add">Chọn</button>
                    <div
                      class="form-show-select"
                      style="width: 100%; left: 0"
                      v-if="showSelectTourCity === index"
                    >
                      <input type="text" v-model="keyword" placeholder="Nhập để tìm kiếm ..." />
                      <ul v-if="citiesFilter.length > 0">
                        <li
                          v-for="city in citiesFilter"
                          :key="city.Id"
                          @click="selectTourCity(city, index)"
                          class="location-item"
                        >
                          <img
                            :src="getLinkImage(city.Image)"
                            alt="image"
                            class="location--image"
                          />
                          <div>
                            <p class="location--name">{{ city.Name }}</p>
                            <p class="location--name-province">{{ city.Province.Name }}</p>
                          </div>
                        </li>
                      </ul>
                    </div>
                  </span>
                  <span
                    >Thứ tự tham quan:
                    <input class="form__input" type="text" v-model="city.VisitOrder"
                  /></span>
                  <button
                    class="btn btn--remove"
                    style="margin-left: 10px"
                    @click="deleteTourCiy(index)"
                  >
                    Xóa
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--1">
              <div class="group-header">
                <p class="form__label">Lịch trình tour<span class="required">*</span></p>
                <button class="btn btn--add" @click="addTourDay">Thêm</button>
              </div>
              <div>
                <div
                  v-for="(tourDay, index) in tour.TourDay"
                  :key="index"
                  style="margin-bottom: 4px; border-radius: 6px"
                >
                  <div
                    style="
                      margin-bottom: 6px;
                      background-color: aliceblue;
                      border-radius: 6px;
                      padding: 4px;
                    "
                  >
                    <div class="group-header">
                      <span
                        >Ngày:
                        <input
                          type="text"
                          style="width: 48px; margin-right: 60px"
                          v-model="tourDay.DayNumber"
                      /></span>
                      <span style="width: 50%"
                        >Mô tả:
                        <input
                          type="text"
                          style="width: 80%"
                          v-model="tourDay.Description"
                          placeholder="Nhập mô tả"
                      /></span>
                      <button class="btn btn--add" @click="addActivity(index)">
                        Thêm hoạt động
                      </button>
                    </div>
                  </div>
                  <div
                    v-for="(activity, activityIndex) in tourDay.Activity"
                    :key="activityIndex"
                    class="group-header"
                  >
                    <textarea
                      type="text"
                      class="form__input"
                      style="width: 90%; padding: 2px 0"
                      rows="2"
                      v-model="activity.Description"
                      required
                    ></textarea>
                    <button class="btn btn--remove" @click="deleteActivity(index, activityIndex)">
                      Xóa
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <div class="group-header">
                <p class="form__label">Giá bao gồm<span class="required">*</span></p>
                <button class="btn btn--add" @click="addTourDetail(1)">Thêm</button>
              </div>
              <div v-for="(detail, index) in tour.TourDetail" :key="index">
                <div v-if="detail.Type === 1" class="group-header" style="width: 100%">
                  <input
                    type="text"
                    class="form__input"
                    style="width: 90%; padding: 2px 0"
                    v-model="detail.Description"
                    required
                  />
                  <button class="btn btn--remove" @click="deleteTourDetail(index)">Xóa</button>
                </div>
              </div>
            </div>
            <div class="form__item form__item--2">
              <div class="group-header">
                <p class="form__label">Giá không bao gồm<span class="required">*</span></p>
                <button class="btn btn--add" @click="addTourDetail(0)">Thêm</button>
              </div>
              <div v-for="(detail, index) in tour.TourDetail" :key="index">
                <div v-if="detail.Type === 0" class="group-header" style="width: 100%">
                  <input
                    type="text"
                    class="form__input"
                    style="width: 90%; padding: 2px 0"
                    v-model="detail.Description"
                    required
                  />
                  <button class="btn btn--remove" @click="deleteTourDetail(index)">Xóa</button>
                </div>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <div class="group-header">
                <p class="form__label">Lưu ý<span class="required">*</span></p>
                <button class="btn btn--add" @click="addTourNotice">Thêm</button>
              </div>
              <div v-for="(notice, index) in tour.TourNotice" :key="index">
                <div class="group-header" style="width: 100%">
                  <textarea
                    type="text"
                    class="form__input"
                    rows="2"
                    style="width: 90%; padding: 2px 0"
                    v-model="notice.Description"
                    required
                  >
                  </textarea>
                  <button class="btn btn--remove" @click="deleteTourNotice(index)">Xóa</button>
                </div>
              </div>
            </div>
            <div class="form__item form__item--2">
              <div>
                <div class="group-header">
                  <p class="form__label">Chính sách giá<span class="required">*</span></p>
                  <button class="btn btn--add" @click="addTourPrice">Thêm</button>
                </div>
                <div v-for="(price, index) in tour.TourPrice" :key="index">
                  <div class="group-header" style="width: 100%">
                    <span>
                      Trẻ từ đủ
                      <input type="number" v-model="price.AgeStart" style="width: 52px" required />
                      tuổi đến dưới tuổi
                      <input type="number" v-model="price.AgeEnd" style="width: 52px" />, chịu
                      <input type="number" v-model="price.Percent" style="width: 60px" required />%
                      giá vé
                    </span>
                    <button class="btn btn--remove" @click="deleteTourPrice(index)">Xóa</button>
                  </div>
                </div>
              </div>
              <div>
                <div class="group-header">
                  <p class="form__label">
                    Chính sách hủy phòng, hoàn tiền<span class="required">*</span>
                  </p>
                  <button class="btn btn--add" @click="addTourRefund">Thêm</button>
                </div>
                <div v-for="(refund, index) in tour.Refund" :key="index">
                  <div class="group-header" style="width: 100%">
                    <span>
                      Hủy trước ngày khởi hành
                      <input type="number" v-model="refund.DayBefore" style="width: 52px" /> ngày,
                      hoàn
                      <input type="number" v-model="refund.RefundPercent" style="width: 60px" />%
                      giá vé
                    </span>
                    <button class="btn btn--remove" @click="deleteTourRefund(index)">Xóa</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form__footer">
            <button class="btn btn--add" @click="triggerFileInput" v-if="id">Thêm ảnh</button>
            <input
              type="file"
              ref="fileInput"
              multiple
              accept="image/*"
              @change="handleFileChange"
              style="display: none"
            />
            <button class="btn btn--add" id="submitButton" @click="submitForm">Lưu</button>
          </div>
        </div>
      </div>
      <div v-else style="margin: auto; display: flex; justify-content: center; align-items: center">
        Loading
      </div>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/form.css'
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import '../../styles/utils.css'

import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { useLocationStore } from '@/stores/location'
import { getLinkImage } from '@/utils'
import { useUserStore } from '@/stores/user'
import { useTourStore } from '@/stores/tour'

const tourStore = useTourStore()

const tour = ref(null)
const keyword = ref('')
const showSelectLocation = ref(0)
const showSelectTourCity = ref(null)
const citiesFilter = ref([])
const searchLocationRef = ref(null)
const errorMessage = ref('')
const submitTour = ref({})

const fileInput = ref(null)
const selectedFiles = ref([])

function triggerFileInput() {
  fileInput.value.click()
}

function handleFileChange(event) {
  const files = event.target.files
  selectedFiles.value = Array.from(files)

  uploadFiles()
}

async function uploadFiles() {
  const formData = new FormData()

  selectedFiles.value.forEach((file) => {
    formData.append('files', file)
  })

  const response = await tourStore.addImageTour(props.id, formData, userStore.getToken)

  if (response.success) {
    closeForm()
  } else {
    errorMessage.value = response.message
  }
}

function selectDepartureCity(city) {
  ;(tour.value.DepartureCity = city), (tour.value.DepartureCityId = city.Id)
  keyword.value = ``
  showSelectLocation.value = null
}

function addTourCity() {
  tour.value.TourCity.push({
    CityId: null,
    VisitOrder: tour.value.TourCity.length + 1,
    City: {}
  })
}

function deleteTourCiy(index) {
  tour.value.TourCity.splice(index, 1)
}

function selectTourCity(city, index) {
  ;(tour.value.TourCity[index].CityId = city.Id), (tour.value.TourCity[index].City = city)
  showSelectTourCity.value = null
  keyword.value = ''
}

function addTourDay() {
  if (tour.value.TourDay.length < tour.value.NumberOfDay)
    tour.value.TourDay.push({
      DayNumber: tour.value.TourDay.length + 1,
      Activity: []
    })
  else console.log('Số ngày trong lịch trình không thể lớn hơn số ngày của tour')
}

function addActivity(index) {
  tour.value.TourDay[index].Activity.push({
    Description: ''
  })
}

function deleteActivity(tourDayIndex, activityIndex) {
  tour.value.TourDay[tourDayIndex].Activity.splice(activityIndex, 1)
}

function addTourDetail(type) {
  tour.value.TourDetail.push({
    Type: type,
    Description: ''
  })
}

function deleteTourDetail(index) {
  tour.value.TourDetail.splice(index, 1)
}

function addTourNotice() {
  tour.value.TourNotice.push({
    Description: ''
  })
}

function deleteTourNotice(index) {
  tour.value.TourNotice.splice(index, 1)
}

function addTourPrice() {
  tour.value.TourPrice.push({
    AgeStart: 0,
    AgeEnd: null,
    Percent: 100
  })
}

function deleteTourPrice(index) {
  tour.value.TourPrice.splice(index, 1)
}

function addTourRefund() {
  tour.value.Refund.push({
    DayBefore: 0,
    RefundPercent: 0
  })
}

function deleteTourRefund(index) {
  tour.value.Refund.splice(index, 1)
}

const props = defineProps({
  id: {
    type: String,
    default: null
  }
})

const emits = defineEmits(['closeForm'])

function closeForm() {
  emits('closeForm')
}

function filterLocation() {
  const query = keyword.value.toLowerCase().split(',')[0]

  citiesFilter.value = cities.value
    ? cities.value.filter((city) => city.Name.toLowerCase().includes(query))
    : []
}

watch(keyword, filterLocation)

function closeSelect() {
  showSelectLocation.value = 0
}

function setRating(rating) {
  tour.value.Rating = rating
}

async function fetchTourData() {
  if (props.id) {
    await tourStore.fetchTour(props.id)
    tour.value = tourStore.getTour
  } else {
    tour.value = {
      NumberOfDay: 1,
      NumberOfNight: 0,
      DepartureCity: {},
      TourDay: [
        {
          DayNumber: 1,
          Activity: []
        }
      ],
      Refund: [],
      TourDetail: [],
      TourNotice: [],
      TourCity: [],
      TourPrice: []
    }
  }
}

function formatTourData() {
  submitTour.value.Name = tour.value.Name
  submitTour.value.NumberOfDay = tour.value.NumberOfDay
  submitTour.value.NumberOfNight = tour.value.NumberOfNight
  submitTour.value.Rating = tour.value.Rating
  submitTour.value.Transport = tour.value.Transport
  submitTour.value.Description = tour.value.Description
  submitTour.value.DepartureCityId = tour.value.DepartureCityId
  submitTour.value.UserId = userStore.getUser.Id
  submitTour.value.TourCity = tour.value.TourCity.map((tourCity) => ({
    Id: tourCity.Id,
    CityId: tourCity.CityId,
    VisitOrder: tourCity.VisitOrder
  }))
  submitTour.value.TourDay = tour.value.TourDay
  submitTour.value.TourDetail = tour.value.TourDetail
  submitTour.value.TourNotice = tour.value.TourNotice
  submitTour.value.TourPrice = tour.value.TourPrice.map((tourPrice) => ({
    Id: tourPrice.Id,
    TourId: tourPrice.TourId,
    AgeStart: tourPrice.AgeStart,
    AgeEnd: tourPrice.AgeEnd === '' ? null : tourPrice.AgeEnd,
    Percent: tourPrice.Percent
  }))
  submitTour.value.Refund = tour.value.Refund
}

async function submitForm() {
  formatTourData()
  console.log(submitTour)
  if (!props.id) {
    const response = await tourStore.createTour(submitTour.value, userStore.getToken)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  } else {
    const response = await tourStore.updateTour(props.id, submitTour.value, userStore.getToken)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  }
}

const locationStore = useLocationStore()
const userStore = useUserStore()
const cities = computed(() => locationStore.getCities)

onMounted(() => {
  fetchTourData()
  locationStore.fetchCities()
  document.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})

function handleClickOutside(event) {
  if (
    showSelectLocation.value === 1 &&
    searchLocationRef.value &&
    !searchLocationRef.value.contains(event.target)
  ) {
    closeSelect()
  }
}
</script>
