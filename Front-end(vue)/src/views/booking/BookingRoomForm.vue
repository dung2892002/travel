<template>
  <div style="margin: 20px auto; min-width: 1080px" v-if="bookingRoom">
    <div style="margin-bottom: 20px; display: flex; flex-direction: column">
      <h2 class="booking_label">Thông tin đặt phòng của bạn</h2>
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
              <input type="text" v-model="bookingRoom.ContactName" class="form__input" />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <label>Email liên hệ</label>
              <input type="text" v-model="bookingRoom.ContactEmail" class="form__input" />
            </div>
            <div class="form__item form__item--2">
              <label>Số điện thoại liên hệ</label>
              <input type="text" v-model="bookingRoom.ContactPhone" class="form__input" />
            </div>
          </div>
        </div>
        <div class="booking-section booking--discount" v-if="discounts && discounts.length > 0">
          <h3 class="booking_label">Chọn khuyến mãi</h3>
          <div v-for="discount in discounts" :key="discount.Id">
            <div
              class="discount-item"
              :class="{ 'discount-item--selected': checkDiscount(discount) }"
              @click="selectDiscount(discount)"
            >
              <span
                >Giảm giá {{ discount.Percent }}%, tối đa
                {{ formatNumber(discount.MaxDiscount) }} vnd từ {{ formatDate(discount.Start) }} đến
                {{ formatDate(discount.End) }}.</span
              >
              <span>Áp dụng cho đặt phòng từ {{ formatNumber(discount.MinPrice) }} vnd</span>
            </div>
          </div>
        </div>
        <div class="booking-section booking--price">
          <h3 class="booking_label">Chi tiết giá</h3>
          <div class="price-item">
            <span class="price__label">Giá phòng/đêm</span>
            <span class="price__value"> {{ formatNumber(bookingRoom.Room.Price) }} VND</span>
          </div>
          <div class="price-item">
            <span class="price__label">Số phòng</span>
            <span class="price__value"> {{ bookingRoom.Quantity }} phòng</span>
          </div>
          <div class="price-item">
            <span class="price__label">Số đêm</span>
            <span class="price__value">
              {{ calculateDay(bookingRoom.CheckInDate, bookingRoom.CheckOutDate) }} đêm</span
            >
          </div>
          <div class="price-item" v-if="bookingRoom.DiscountId">
            <span class="price__label">Giảm giá</span>
            <span class="price__value"> {{ formatNumber(bookingRoom.DiscountValue) }} VND</span>
          </div>
          <div class="price-item">
            <span class="price__label">Thuế</span>
            <span class="price__value">
              {{ formatNumber(Math.round(bookingRoom.Price * 0.15)) }} VND</span
            >
          </div>
          <div class="price-item">
            <span class="price__label">Tổng cộng</span>
            <span class="price__value">
              {{
                formatNumber(
                  bookingRoom.Price -
                    bookingRoom.DiscountValue +
                    Math.round(bookingRoom.Price * 0.15)
                )
              }}
              VND</span
            >
          </div>
        </div>
      </div>
      <div
        class="booking-section booking-info"
        style="width: 50%; height: auto; display: flex; flex-direction: column"
      >
        <div>
          <span class="info-value info--name">{{ bookingRoom.Hotel.Name }}</span>
          <span class="info-value info--rating">{{ renderRating(bookingRoom.Hotel.Rating) }}</span>
          <img
            :src="getLinkImage(bookingRoom.Hotel.Image[0].Path)"
            alt="anh ks"
            class="info-value info--image"
          />
        </div>
        <div>
          <div class="info-value info-group">
            <span>CheckIn</span>
            <span
              >Sau {{ bookingRoom.Hotel.CheckInTime.substring(0, 5) }} -
              {{ formatDate(bookingRoom.CheckInDate) }}</span
            >
          </div>
          <div class="info-value info-group">
            <span>CheckOut</span>
            <span
              >Trước {{ bookingRoom.Hotel.CheckOutTime.substring(0, 5) }} -
              {{ formatDate(bookingRoom.CheckOutDate) }}</span
            >
          </div>
        </div>
        <span class="info-value info-quantity"
          >({{ bookingRoom.Quantity }}x) {{ bookingRoom.Room.Name }}</span
        >
        <div class="info-value info--room">
          <div class="info-group">
            <img src="../../assets/icon/adult_people.png" alt="icon" class="icon-room" />
            <span>{{ bookingRoom.Room.MaxAdultPeople }} người lớn</span>
          </div>
          <div class="info-group">
            <img src="../../assets/icon/children.png" alt="icon" class="icon-room" />
            <span>{{ bookingRoom.Room.MaxChildrenPeople }} trẻ em</span>
          </div>
          <div class="info-group" v-if="bookingRoom.Room.SingleBed > 0">
            <img src="../../assets/icon/single_bed.png" alt="icon" class="icon-room" />
            <span>{{ bookingRoom.Room.SingleBed }} giuờng đơn</span>
          </div>
          <div class="info-group" v-if="bookingRoom.Room.DoubleBed > 0">
            <img src="../../assets/icon/double_bed.png" alt="icon" class="icon-room" />
            <span>{{ bookingRoom.Room.DoubleBed }} giuờng đôi</span>
          </div>
        </div>
        <div class="info-value info-group">
          <span>Tổng cộng:</span>
          <span class="info--price">
            {{
              formatNumber(
                bookingRoom.Price -
                  bookingRoom.DiscountValue +
                  Math.round((bookingRoom.Price * 15) / 100)
              )
            }}
            VND</span
          >
        </div>
        <span v-if="errorMessage" class="error-message"> {{ errorMessage }}</span>
        <span v-if="successMessage" class="success-message">{{ successMessage }}</span>
        <button class="btn btn--add" @click="handleBookingRoom">Đặt phòng</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue'
import '../../styles/layout/form.css'
import '../../styles/base/input.css'
import '../../styles/base/button.css'
import { calculateDay, formatDate, formatNumber, getLinkImage } from '@/utils'
import { useDiscountStore } from '@/stores/discount'
import { useUserStore } from '@/stores/user'
import { useBookingStore } from '@/stores/booking'

const discountStore = useDiscountStore()
const userStore = useUserStore()
const bookingStore = useBookingStore()

const bookingRoom = ref(null)
const discounts = computed(() => discountStore.getHotelDiscounts)
const errorMessage = ref(null)
const successMessage = ref(null)

function selectDiscount(discount) {
  bookingRoom.value.DiscountId = discount.Id
  const value = Math.round((discount.Percent * bookingRoom.value.Price) / 100)
  bookingRoom.value.DiscountValue = value <= discount.MaxDiscount ? value : discount.MaxDiscount
}

function checkDiscount(discount) {
  if (bookingRoom.value.DiscountId != discount.Id) return false
  return true
}

function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

const submitBooking = ref({})

function formatDataBooking() {
  submitBooking.value.CheckInDate = bookingRoom.value.CheckInDate
  submitBooking.value.CheckOutDate = bookingRoom.value.CheckOutDate
  submitBooking.value.ContactEmail = bookingRoom.value.ContactEmail
  submitBooking.value.ContactName = bookingRoom.value.ContactName
  submitBooking.value.ContactPhone = bookingRoom.value.ContactPhone
  submitBooking.value.Quantity = bookingRoom.value.Quantity
  submitBooking.value.RoomId = bookingRoom.value.Room.Id
  submitBooking.value.Price = bookingRoom.value.Price
  submitBooking.value.DiscountId = bookingRoom.value.DiscountId
    ? bookingRoom.value.DiscountId
    : null
  submitBooking.value.UserId = user.value.Id
}

async function handleBookingRoom() {
  formatDataBooking()
  const response = await bookingStore.createBookingRoom(submitBooking.value, token.value)
  if (response.success) {
    successMessage.value = 'Đặt phòng thành công, sau 15 phút nếu không thanh toán sẽ tự hủy'
  } else {
    errorMessage.value = response.message
  }
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

onMounted(() => {
  bookingRoom.value = JSON.parse(sessionStorage.getItem('bookingRoom'))
  discountStore.fetchHotelDiscounts(bookingRoom.value.Hotel.Id, bookingRoom.value.Price)
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
  margin-bottom: 30px;
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
