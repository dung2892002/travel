<template>
  <div style="margin: 20px auto; width: 1080px" v-if="bookingTour && bookingTourPeople">
    <div style="margin-bottom: 20px; display: flex; flex-direction: column">
      <h2 class="booking_label">Thông tin đặt tour của bạn</h2>
      <span style="color: #687176; font-size: 16px; font-weight: 500"
        >Chắc chắn các thông tin bên dưới là chính xác trước khi đặt tour</span
      >
      <span v-if="errorMessage" class="error-message"> {{ errorMessage }}</span>
      <span v-if="successMessage" class="success-message">{{ successMessage }}</span>
    </div>
    <div class="content--row" style="width: 100%; overflow-x: hidden">
      <div class="content--column" style="width: 100%">
        <div class="booking-section">
          <h3 class="booking_label">Thông tin liên hệ</h3>
          <div class="form-group">
            <div class="form__item form__item--1">
              <label>Tên liên hệ</label>
              <input type="text" v-model="bookingTour.ContactName" class="form__input" />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <label>Email liên hệ</label>
              <input type="text" v-model="bookingTour.ContactEmail" class="form__input" />
            </div>
            <div class="form__item form__item--2">
              <label>Số điện thoại liên hệ</label>
              <input type="text" v-model="bookingTour.ContactPhone" class="form__input" />
            </div>
          </div>
        </div>
        <div class="booking-section" style="max-width: 100%">
          <h3 class="booking_label">Chi tiết số khách</h3>
          <div
            v-for="(bookingPeople, index) in bookingTourPeople"
            :key="index"
            style="
              margin-bottom: 6px;
              display: flex;
              flex-direction: row;
              justify-content: space-between;
            "
          >
            <div>
              <span>Số lượng khách từ {{ bookingPeople.AgeStart }}</span>
              <span v-if="bookingPeople.AgeEnd" style="margin-left: 4px"
                >đến dưới {{ bookingPeople.AgeEnd }}</span
              >
              <span style="margin-left: 4px">tuổi</span>
              <input
                type="number"
                v-model="bookingPeople.QuantityPeople"
                class="form__input"
                style="margin-left: 6px; width: 64px"
              />
              <span style="margin-left: 4px"
                >x
                {{
                  formatNumber(
                    Math.round((bookingTour.Schedule.Price * bookingPeople.Percent) / 100)
                  )
                }}
                vnd
              </span>
            </div>
            <span style="font-weight: bold; color: #078cf8">
              {{
                formatNumber(
                  Math.round((bookingTour.Schedule.Price * bookingPeople.Percent) / 100) *
                    bookingPeople.QuantityPeople
                )
              }}
              vnd</span
            >
          </div>
          <div class="booking--discount" v-if="discounts && discounts.length > 0">
            <h3 class="booking_label">Chọn khuyến mãi</h3>
            <div v-for="discount in discounts" :key="discount.Id">
              <div
                class="discount-item"
                :class="{ 'discount-item--selected': checkDiscount(discount) }"
                @click="selectDiscount(discount)"
              >
                <span
                  >Giảm giá {{ discount.Percent }}%, tối đa
                  {{ formatNumber(discount.MaxDiscount) }} vnd từ
                  {{ formatDate(discount.Start) }} đến {{ formatDate(discount.End) }}.</span
                >
                <span>Áp dụng cho đặt tour từ {{ formatNumber(discount.MinPrice) }} vnd</span>
              </div>
            </div>
          </div>
        </div>

        <div class="booking-section booking--price">
          <h3 class="booking_label">Chi tiết giá</h3>
          <div class="price-item">
            <span class="price__label">Giá khách/tour</span>
            <span class="price__value"> {{ formatNumber(bookingTour.Schedule.Price) }} VND</span>
          </div>
          <div class="price-item" v-if="bookingTour.DiscountId">
            <span class="price__label">Giảm giá</span>
            <span class="price__value"> {{ formatNumber(bookingTour.DiscountValue) }} VND</span>
          </div>
          <div class="price-item">
            <span class="price__label">Thuế</span>
            <span class="price__value">
              {{ formatNumber(Math.round(bookingTour.Price * 0.15)) }} VND</span
            >
          </div>
          <div class="price-item">
            <span class="price__label">Tổng cộng</span>
            <span class="price__value">
              {{
                formatNumber(
                  bookingTour.Price -
                    bookingTour.DiscountValue +
                    Math.round(bookingTour.Price * 0.15)
                )
              }}
              VND</span
            >
          </div>
        </div>
      </div>
      <div
        class="booking-section booking-info"
        style="width: 50%; height: 560px; display: flex; flex-direction: column"
      >
        <div>
          <span class="info-value info--name">{{ bookingTour.TourName }}</span>
          <span class="info-value info--rating">{{ renderRating(bookingTour.TourRating) }}</span>
          <img
            :src="getLinkImage(bookingTour.TourImage.Path)"
            alt="anh ks"
            class="info-value info--image"
          />
        </div>
        <div>
          <span class="info-value info-quantity"
            >Khởi hành vào: {{ formatDate(bookingTour.Schedule.DateStart) }}</span
          >
          <span class="info-value info-quantity"
            >Giá cơ bản: {{ formatNumber(bookingTour.Schedule.Price) }}</span
          >
        </div>
        <div class="info-value info--room">
          <div
            v-for="(bookingPeople, index) in bookingTourPeople"
            :key="index"
            style="
              margin-bottom: 6px;
              display: flex;
              flex-direction: row;
              justify-content: space-between;
            "
          >
            <div>
              <span>Số lượng khách từ {{ bookingPeople.AgeStart }}</span>
              <span v-if="bookingPeople.AgeEnd" style="margin-left: 4px"
                >đến &#60;{{ bookingPeople.AgeEnd }}</span
              >
              <span style="margin-left: 4px">tuổi:</span>
              <span style="margin-left: 4px">{{ bookingPeople.QuantityPeople }}</span>
            </div>
          </div>
        </div>
        <div class="info-value info-group">
          <span>Tổng cộng:</span>
          <span class="info--price">
            {{
              formatNumber(
                bookingTour.Price - bookingTour.DiscountValue + Math.round(bookingTour.Price * 0.15)
              )
            }}
            VND</span
          >
        </div>
        <button class="btn btn--add" @click="handleBookingTour">Đặt tour</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref, watch } from 'vue'
import '../../styles/layout/form.css'
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import { formatDate, formatNumber, getLinkImage } from '@/utils'
import { useDiscountStore } from '@/stores/discount'
import { useUserStore } from '@/stores/user'
import { useBookingStore } from '@/stores/booking'

const discountStore = useDiscountStore()
const userStore = useUserStore()
const bookingStore = useBookingStore()

const bookingTour = ref(null)
const bookingTourPeople = ref(null)
const discounts = computed(() => discountStore.getTourDiscounts)

const errorMessage = ref(null)
const successMessage = ref(null)

watch(
  bookingTourPeople,
  (newBookingTourPeople) => {
    const totalPrice = newBookingTourPeople.reduce((total, bookingPeople) => {
      const unitPrice = Math.round((bookingTour.value.Schedule.Price * bookingPeople.Percent) / 100)
      return total + unitPrice * bookingPeople.QuantityPeople
    }, 0)

    bookingTour.value.Price = totalPrice
    resetDiscount()
    fetchDiscounts()
  },
  { deep: true }
)

function resetDiscount() {
  bookingTour.value.DiscountId = null
  bookingTour.value.DiscountValue = 0
}

function selectDiscount(discount) {
  bookingTour.value.DiscountId = discount.Id
  const value = Math.round((discount.Percent * bookingTour.value.Price) / 100)
  bookingTour.value.DiscountValue = value <= discount.MaxDiscount ? value : discount.MaxDiscount
}

function checkDiscount(discount) {
  if (bookingTour.value.DiscountId != discount.Id) return false
  return true
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

const submitBooking = ref({})

function formatDataBooking() {
  submitBooking.value.TourScheduleId = bookingTour.value.Schedule.Id
  submitBooking.value.ContactEmail = bookingTour.value.ContactEmail
  submitBooking.value.ContactName = bookingTour.value.ContactName
  submitBooking.value.ContactPhone = bookingTour.value.ContactPhone
  submitBooking.value.Price = bookingTour.value.Price
  submitBooking.value.DiscountId = bookingTour.value.DiscountId
    ? bookingTour.value.DiscountId
    : null
  submitBooking.value.UserId = user.value.Id
  submitBooking.value.BookingTourPeople = bookingTourPeople.value
}

async function handleBookingTour() {
  formatDataBooking()
  const response = await bookingStore.createBookingTour(submitBooking.value, token.value)
  if (response.success) {
    successMessage.value = 'Đặt phòng thành công, sau 15 phút nếu không thanh toán sẽ tự hủy'
  } else {
    errorMessage.value = response.message
  }
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

async function fetchDiscounts() {
  await discountStore.fetchTourDiscounts(bookingTour.value.TourId, bookingTour.value.Price)
}

onMounted(() => {
  bookingTour.value = JSON.parse(sessionStorage.getItem('bookingTour'))
  bookingTourPeople.value = JSON.parse(sessionStorage.getItem('bookingTourPeople'))
  console.log(bookingTour.value)
  console.log(bookingTourPeople.value)
})
</script>

<style scoped>
.booking-section {
  overflow-x: hidden;
  background-color: #ffffff;
  padding: 20px;
  border-radius: 6px;
  box-shadow: 4px 4px 4px #0000001a;
  margin-right: 20px;
  margin-bottom: 10px;
}

.input_contact {
  display: flex;
  flex-direction: column;
  justify-content: left;
}

.form-group {
  margin-bottom: 20px;
}

.form__item label {
  font-size: 14px;
  line-height: 20px;
  font-weight: 500;
  color: #687176;
}

.booking_label {
  margin-bottom: 10px;
}

.price-item {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 10px;
}

.price__label,
.price__value {
  color: #354148;
  line-height: 24px;
  font-size: 16px;
  font-weight: 500;
}

.price__value {
  font-weight: 600;
}

.booking--discount {
  max-height: 240px;
}

.discount-item {
  border: 1px solid black;
  padding: 6px;
  font-size: 14px;
  border-radius: 12px;
  margin-bottom: 6px;
  cursor: pointer;
  display: flex;
  flex-direction: column;
}

.discount-item--selected {
  background-color: #078cf8;
  color: white;
}

.discount-item:hover {
  color: white;
  background-color: #078cf8;
}

.booking-info {
  font-size: 16px;
  line-height: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.info-value {
  margin-bottom: 10px;
}

.info--name {
  font-weight: bold;
}

.info--rating {
  font-size: 20px;
  color: #d6af03;
}

.info-group {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  color: #687176;
  margin-bottom: 4px;
}

.info-group span:first-child,
.info-quantity {
  font-weight: bold;
  color: #000000;
}

.info--price {
  color: #078cf8;
  font-weight: bold;
  font-size: 18px;
}

.info--room {
  display: flex;
  flex-direction: column;
  justify-content: left;
  align-items: self-start;
  font-weight: 300;
  font-size: 14px;
  color: #687176;
}

.icon-room {
  margin-right: 10px;
  width: 20px;
  height: 20px;
}
</style>
