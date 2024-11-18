<template>
  <div class="form-container" id="hotelForm">
    <div class="form__content">
      <div class="form__header">
        <h2 class="form__title">Thông tin khách sạn</h2>
        <span class="error" v-if="errorMessage.length > 0"> {{ errorMessage }}</span>
        <button class="form__button" @click="closeForm">
          <img src="/src/assets/icon/close-48.png" alt="logo" />
        </button>
      </div>
      <div class="content--row">
        <div class="facilities">
          <div class="facility">
            <span class="facility__label">Chọn tiện nghi phổ biến</span>
            <div v-for="facility in popularFacilities" :key="facility.Id" class="facility__item">
              <button
                class="facility__button btn--remove"
                v-if="checkContainFacility(facility.Id)"
                @click="removeFacility(facility)"
              >
                -
              </button>
              <button class="facility__button btn--add" v-else @click="addFacility(facility)">
                +
              </button>
              <span>{{ facility.Name }}</span>
            </div>
          </div>
          <div class="facility">
            <span class="facility__label">Chọn tiện nghi đặc biệt</span>
            <div v-for="facility in uniqueFacilities" :key="facility.Id" class="facility__item">
              <button
                class="facility__button btn--remove"
                v-if="checkContainFacility(facility.Id)"
                @click="removeFacility(facility)"
              >
                -
              </button>
              <button class="facility__button btn--add" v-else @click="addFacility(facility)">
                +
              </button>
              <span>{{ facility.Name }}</span>
            </div>
          </div>
        </div>
        <div class="travel-form">
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Tên khách sạn <span class="required">*</span></p>
              <input type="text" class="form__input" v-model="hotel.Name" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Đánh giá<span class="required">*</span></p>
              <div class="stars">
                <span
                  v-for="star in 5"
                  :key="star"
                  class="star"
                  :class="{ 'star--filled': hotel.Rating >= star }"
                  @click="setRating(star)"
                >
                  ★
                </span>
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--1">
              <p class="form__label">Mô tả<span class="required">*</span></p>
              <textarea
                type="text"
                class="form__input"
                rows="5"
                v-model="hotel.Description"
                required
              ></textarea>
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Email liên hệ<span class="required">*</span></p>
              <input type="text" class="form__input" v-model="hotel.Email" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Sdt liên hệ<span class="required">*</span></p>
              <input type="text" class="form__input" v-model="hotel.PhoneNumber" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Check in<span class="required">*</span></p>
              <input type="time" class="form__input" v-model="hotel.CheckInTime" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Check out<span class="required">*</span></p>
              <input type="time" class="form__input" v-model="hotel.CheckOutTime" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__section--left content--column form-location" ref="searchLocationRef">
              <p class="form__label">Vị trí khách sạn</p>
              <input
                type="text"
                class="form__input"
                v-model="keyword"
                placeholder="Địa điểm khách sạn"
                @focus="(showSelectLocation = 1), (showSelectDestination = 0)"
              />
              <div v-if="showSelectLocation === 1" class="show-select">
                <ul v-if="citiesFilter.length > 0">
                  <li
                    v-for="city in citiesFilter"
                    :key="city.Id"
                    @click="selectCity(city)"
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

              <div class="form__item form__item--1 form-destination" ref="showDestinationRef">
                <div class="content--row">
                  <p class="form__label">Điểm đến lân cận</p>
                  <button
                    class="destination__button btn--add"
                    @click="(showSelectDestination = 1), (showSelectLocation = 0)"
                    v-if="hotel.City"
                  >
                    <span class="button--add-text">+</span>
                  </button>
                  <div v-if="showSelectDestination === 1" class="show-select select--destination">
                    <div class="destination">
                      <span class="destination__label">Chọn điểm đến lân cận</span>
                      <div
                        v-for="destination in destinations"
                        :key="destination.Id"
                        class="destination__item"
                      >
                        <button
                          class="destination__button btn--remove"
                          v-if="checkContainDestination(destination.Id)"
                          @click="removeDestination(destination)"
                        >
                          -
                        </button>
                        <button
                          class="destination__button btn--add"
                          v-else
                          @click="addDestination(destination)"
                        >
                          +
                        </button>
                        <span>{{ destination.Name }}</span>
                      </div>
                    </div>
                    <button class="btn btn--close w-4" @click="closeSelect">Ok</button>
                  </div>
                </div>
                <div class="multichoice">
                  <div v-for="destination in hotel.HotelDestination" :key="destination.Id">
                    <div class="destination__hotel">
                      <span>{{ destination.Destination.Name }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="form__section--right content--column">
              <div class="form__item form__item--1">
                <p class="form__label">Loại hình<span class="required">*</span></p>
                <select v-model="hotel.Type" class="form__input">
                  <option value="1">Khu nghỉ dưỡng (Resort)</option>
                  <option value="2">Biệt thự (Villa)</option>
                  <option value="3">Khách sạn (Hotel)</option>
                  <option value="4">Căn hộ (Apartment)</option>
                  <option value="5">Nhà nghỉ (Guesthouse)</option>
                </select>
              </div>
              <div class="form__item form__item--1 form-facility">
                <p class="form__label">Tiện nghi phổ biến</p>
                <div class="multichoice">
                  <div v-for="facility in hotel.HotelFacility" :key="facility.Id">
                    <div v-if="facility.Facility.Type === 1" class="facility__hotel">
                      <span>{{ facility.Facility.Name }}</span>
                    </div>
                  </div>
                </div>
              </div>
              <div class="form__item form__item--1 form-facility">
                <p class="form__label">Tiện nghi đặc biệt</p>
                <div class="multichoice">
                  <div v-for="facility in hotel.HotelFacility" :key="facility.Id">
                    <div v-if="facility.Facility.Type === 2" class="facility__hotel">
                      <span>{{ facility.Facility.Name }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form__footer">
            <button class="btn btn--close" @click="closeForm">Hủy</button>
            <button class="btn btn--add" id="submitButton" @click="submitForm">Lưu</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/form.css'
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import '../../styles/utils.css'

import { useHotelStore } from '@/stores/hotel'
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { useLocationStore } from '@/stores/location'
import { getLinkImage } from '@/utils'
import { useUserStore } from '@/stores/user'

const hotelStore = useHotelStore()

const hotel = ref({})
const keyword = ref('')
const showSelectLocation = ref(0)
const showSelectDestination = ref(0)
const citiesFilter = ref([])
const searchLocationRef = ref(null)
const showDestinationRef = ref(null)
const errorMessage = ref('')

const submitHotel = ref({})

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

async function selectCity(city) {
  hotel.value.CityId = city.Id
  hotel.value.City = city
  keyword.value = `${city.Name}, ${city.Province.Name}`
  await locationStore.fetchDestinations(hotel.value.CityId)
  showSelectLocation.value = 0
}

function closeSelect() {
  showSelectLocation.value = 0
  showSelectDestination.value = 0
}

function setRating(rating) {
  hotel.value.Rating = rating
}

function removeFacility(facility) {
  const newHotelFacility = hotel.value.HotelFacility.filter(
    (hotelFacility) => hotelFacility.Facility.Id != facility.Id
  )
  hotel.value.HotelFacility = newHotelFacility
}

function addFacility(facility) {
  hotel.value.HotelFacility.push({
    Facility: facility
  })
}

function removeDestination(destination) {
  const newHotelDestination = hotel.value.HotelDestination.filter(
    (HotelDestination) => HotelDestination.Destination.Id != destination.Id
  )
  hotel.value.HotelDestination = newHotelDestination
}

function addDestination(destination) {
  hotel.value.HotelDestination.push({
    Destination: destination
  })
}

function checkContainFacility(id) {
  return (
    hotel.value &&
    Array.isArray(hotel.value.HotelFacility) &&
    hotel.value.HotelFacility.some((facility) => facility.Facility.Id === id)
  )
}

function checkContainDestination(id) {
  return (
    hotel.value &&
    Array.isArray(hotel.value.HotelDestination) &&
    hotel.value.HotelDestination.some((destination) => destination.Destination.Id === id)
  )
}

async function fetchHotelData() {
  if (props.id) {
    await hotelStore.fetchHotel(props.id)
    hotel.value = hotelStore.getHotel
    await locationStore.fetchDestinations(hotel.value.CityId)
    keyword.value = `${hotel.value.City.Name}, ${hotel.value.City.Province.Name}`
  } else {
    hotel.value = { HotelFacility: [], HotelDestination: [] }
  }
}

function formatHotelData() {
  submitHotel.value.Name = hotel.value.Name
  submitHotel.value.Rating = hotel.value.Rating
  submitHotel.value.Description = hotel.value.Description
  submitHotel.value.Email = hotel.value.Email
  submitHotel.value.PhoneNumber = hotel.value.PhoneNumber
  submitHotel.value.CheckInTime = `${hotel.value.CheckInTime}:00`
  submitHotel.value.CheckOutTime = `${hotel.value.CheckOutTime}:00`
  submitHotel.value.CityId = hotel.value.CityId
  submitHotel.value.Type = hotel.value.Type
  submitHotel.value.UserId = userStore.getUser.Id

  submitHotel.value.HotelDestination = hotel.value.HotelDestination.map((hotelDestination) => ({
    DestinationId: hotelDestination.Destination.Id
  }))

  submitHotel.value.HotelFacility = hotel.value.HotelFacility.map((hotelFacility) => ({
    FacilityId: hotelFacility.Facility.Id
  }))
}

async function submitForm() {
  formatHotelData()
  if (!props.id) {
    const response = await hotelStore.createHotel(submitHotel.value, userStore.getToken)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  } else {
    console.log('chinh sua hotel')
  }
  console.log(submitHotel)
}

const locationStore = useLocationStore()
const userStore = useUserStore()
const cities = computed(() => locationStore.getCities)
const destinations = computed(() => locationStore.getDestinations)

const popularFacilities = computed(() => hotelStore.getPopularFacilities)
const uniqueFacilities = computed(() => hotelStore.getUniqueFacilities)

onMounted(() => {
  fetchHotelData()
  hotelStore.fetchFacilities(1)
  hotelStore.fetchFacilities(2)
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
