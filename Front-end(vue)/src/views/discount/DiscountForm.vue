<template>
  <div class="form-container">
    <div class="form__content">
      <div class="form__header">
        <h2 class="form__title">Thông tin khuyến mãi</h2>
        <span class="error" v-if="errorMessage.length > 0"> {{ errorMessage }}</span>
        <button class="form__button" @click="closeForm">
          <img src="/src/assets/icon/close-48.png" alt="logo" />
        </button>
      </div>
      <div class="content--row">
        <div class="travel-form">
          <div class="form-group">
            <div class="form__section--left content--column" style="width: 30%">
              <div class="form-group">
                <div class="form__item form__item--1">
                  <p class="form__label">Tỉ lệ khuyến mại(%) <span class="required">*</span></p>
                  <input
                    type="number"
                    class="form__input"
                    v-model="discount.Percent"
                    max="100"
                    required
                    @input="validateDiscount"
                  />
                </div>
              </div>
              <div class="form-group">
                <div class="form__item form__item--1">
                  <p class="form__label">
                    Mức giá tối thiểu được áp dụng(vnd) <span class="required">*</span>
                  </p>
                  <input type="number" class="form__input" v-model="discount.MinPrice" required />
                </div>
              </div>
              <div class="form-group">
                <div class="form__item form__item--1">
                  <p class="form__label">Khuyến mại tối đa(vnd) <span class="required">*</span></p>
                  <input
                    type="number"
                    class="form__input"
                    v-model="discount.MaxDiscount"
                    required
                  />
                </div>
              </div>

              <div class="form-group">
                <div class="form__item form__item--2">
                  <p class="form__label">Ngày bắt đầu<span class="required">*</span></p>
                  <input type="date" class="form__input" v-model="discount.Start" required />
                </div>
                <div class="form__item form__item--2">
                  <p class="form__label">Kết thúc sau<span class="required">*</span></p>
                  <input type="date" class="form__input" v-model="discount.End" required />
                </div>
              </div>
            </div>
            <div class="form__item" style="width: 65%; height: auto">
              <div class="content--column form-location" v-if="checkRole(5)">
                <div class="form__item form__item--1 form-destination">
                  <div
                    class="multichoice"
                    style="
                      height: auto;
                      display: flex;
                      flex-direction: column;
                      align-items: self-start;
                    "
                  >
                    <div
                      v-for="hotel in discount.DiscountHotel"
                      :key="hotel.Id"
                      style="
                        display: flex;
                        flex-direction: row;
                        align-items: center;
                        background-color: #ffffff;
                        margin-left: 10px;
                        border-radius: 12px;
                        overflow: hidden;
                        margin-bottom: 6px;
                      "
                    >
                      <img
                        :src="getLinkImage(hotel.Hotel.Image[0].Path)"
                        alt=""
                        style="width: 48px; height: 36px"
                      />
                      <span class="destination__name">{{ hotel.Hotel.Name }}</span>
                    </div>
                  </div>
                  <div class="content--row">
                    <p class="form__label">Danh sách khách sạn áp dụng</p>
                    <button class="destination__button btn--add" @click="showSelectHotel = 1">
                      <span class="button--add-text">+</span>
                    </button>
                    <div v-if="showSelectHotel === 1" class="form-show-select select--destination">
                      <div class="destination">
                        <span class="destination__label">Chọn khách sạn</span>
                        <div
                          v-for="hotel in hotels"
                          :key="hotel.Id"
                          class="destination__item"
                          style="display: flex; flex-direction: row; align-items: center"
                        >
                          <button
                            class="destination__button btn--remove"
                            v-if="checkContainHotel(hotel.Id)"
                            @click="removeHotel(hotel)"
                          >
                            -
                          </button>
                          <button
                            class="destination__button btn--add"
                            v-else
                            @click="addHotel(hotel)"
                          >
                            +
                          </button>
                          <div
                            style="
                              display: flex;
                              flex-direction: row;
                              align-items: center;
                              background-color: #ffffff;
                              margin-left: 10px;
                              border-radius: 12px;
                              overflow: hidden;
                            "
                          >
                            <img
                              :src="getLinkImage(hotel.Image[0].Path)"
                              alt=""
                              style="width: 48px; height: 36px"
                            />
                            <span class="destination__name">{{ hotel.Name }}</span>
                          </div>
                        </div>
                      </div>
                      <button class="btn btn--close w-4" @click="closeSelect">Ok</button>
                    </div>
                  </div>
                </div>
              </div>
              <div class="content--column form-location" v-if="checkRole(6)">
                <div class="form__item form__item--1 form-destination">
                  <div class="multichoice content--column">
                    <div v-for="hotel in discount.DiscountHotel" :key="hotel.Id">
                      <div class="destination__hotel">
                        {{ hotel.Hotel.Name }}
                        <button
                          class="destination__button btn--remove"
                          @click="removeHotel(hotel.Hotel)"
                        >
                          -
                        </button>
                      </div>
                    </div>
                  </div>
                  <div class="content--row">
                    <p class="form__label">Danh sách khách sạn áp dụng</p>
                    <button class="destination__button btn--add" @click="showSelectHotel = 1">
                      <span class="button--add-text">+</span>
                    </button>
                    <div v-if="showSelectHotel === 1" class="form-show-select select--destination">
                      <div class="destination">
                        <span class="destination__label">Chọn khách sạn</span>
                        <div v-for="hotel in hotels" :key="hotel.Id" class="destination__item">
                          <button
                            class="destination__button btn--remove"
                            v-if="checkContainHotel(hotel.Id)"
                            @click="removeHotel(hotel)"
                          >
                            -
                          </button>
                          <button
                            class="destination__button btn--add"
                            v-else
                            @click="addHotel(hotel)"
                          >
                            +
                          </button>
                          <span class="destination__name">{{ hotel.Name }}</span>
                        </div>
                      </div>
                      <button class="btn btn--close w-4" @click="closeSelect">Ok</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form__footer">
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
import { computed, onMounted, ref } from 'vue'
// import { getLinkImage } from '@/utils'
import { useUserStore } from '@/stores/user'
import { useDiscountStore } from '@/stores/discount'
import { formatDateForm, getLinkImage } from '@/utils'

const hotelStore = useHotelStore()
const userStore = useUserStore()
const discountStore = useDiscountStore()

const discount = ref({
  Percent: 0,
  MinPrice: 100000,
  MaxDiscount: 100000,
  DiscountHotel: [],
  DiscountTour: [],
  Start: formatDateForm(new Date()),
  End: formatDateForm(new Date())
})

const showSelectHotel = ref(0)
const errorMessage = ref('')
const submitDiscount = ref({})

const emits = defineEmits(['closeForm'])

function closeForm() {
  emits('closeForm')
}

function validateDiscount() {
  if (discount.value.Percent > 100) {
    discount.value.Percent = 100
  } else if (discount.value.Percent < 0) {
    discount.value.Percent
  }
}

function closeSelect() {
  showSelectHotel.value = 0
}

function checkRole(value) {
  return user.value.Roles.some((role) => role.RoleValue === value)
}

function removeHotel(hotel) {
  const newDiscountHotel = discount.value.DiscountHotel.filter(
    (DiscountHotel) => DiscountHotel.Hotel.Id != hotel.Id
  )
  discount.value.DiscountHotel = newDiscountHotel
}

function addHotel(hotel) {
  discount.value.DiscountHotel.push({
    Hotel: hotel
  })
}

function checkContainHotel(id) {
  return (
    discount.value &&
    Array.isArray(discount.value.DiscountHotel) &&
    discount.value.DiscountHotel.some((hotel) => hotel.Hotel.Id === id)
  )
}

function formatDiscountData() {
  submitDiscount.value.Percent = discount.value.Percent
  submitDiscount.value.MinPrice = discount.value.MinPrice
  submitDiscount.value.MaxDiscount = discount.value.MaxDiscount
  submitDiscount.value.Start = discount.value.Start
  submitDiscount.value.End = discount.value.End
  submitDiscount.value.UserId = userStore.getUser.Id
  submitDiscount.value.Type = 1
  submitDiscount.value.DiscountHotel = discount.value.DiscountHotel.map((discountHotel) => ({
    HotelId: discountHotel.Hotel.Id
  }))

  submitDiscount.value.DiscountTour = discount.value.DiscountTour.map((discountTour) => ({
    TourId: discountTour.Tour.Id
  }))
}

async function submitForm() {
  formatDiscountData()
  if (checkRole(10)) {
    submitDiscount.value.Type = 0
    const response = await discountStore.createDiscount(submitDiscount.value, token.value)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  } else if (checkRole(5)) {
    const response = await discountStore.createHotelDiscount(submitDiscount.value, token.value)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  } else {
    const response = await discountStore.createTourDiscount(submitDiscount.value, token.value)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  }
  console.log(submitDiscount.value)
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
const hotels = computed(() => hotelStore.getHotels)

onMounted(() => {
  if (checkRole(5)) {
    hotelStore.fetchHotelByPartner(userStore.getUser.Id, userStore.getToken)
  }
})
</script>

<style scoped>
.form__label {
  font-size: 14px;
}
.select--destination {
  top: 100%;
}
</style>
