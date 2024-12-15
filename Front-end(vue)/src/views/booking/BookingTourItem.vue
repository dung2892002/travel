<template>
  <div
    class="content--row content-item"
    style="height: 240px; overflow-x: hidden; min-width: 820px"
  >
    <div class="item-img">
      <div v-if="booking.TourSchedule.Tour.Image.length > 0" class="item-img--hasimage">
        <img
          :src="getLinkImage(booking.TourSchedule.Tour.Image[0].Path)"
          alt="anh ks"
          class="item-img"
        />
        <div class="item-hover-overlay" @click="toggleImagePopup">Xem ảnh</div>
      </div>
      <img src="../../assets/image/hotel.jpg" alt="" v-else class="item-img" />
    </div>
    <div style="width: 100%">
      <div class="content--column item-info">
        <div
          class="info-value info__name"
          style="
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            margin-top: 10px;
          "
        >
          <span
            >{{ booking.TourSchedule.Tour.Name }} - Khởi hành:
            {{ formatDate(booking.TourSchedule.DateStart) }}
          </span>
          <div>
            <span v-if="booking.Status === 0" class="booking-status booking-status--notpaid">
              Chưa thanh toán</span
            >
            <span v-if="booking.Status === 1" class="booking-status booking-status--paid">
              Đã thanh toán</span
            >
            <span v-if="booking.Status === 2" class="booking-status booking-status--cancel">
              Đã hủy</span
            >
            <span v-if="booking.Status === 3" class="booking-status booking-status--cancel">
              Chờ hoàn tiền</span
            >
            <span v-if="booking.Status === 4" class="booking-status booking-status--cancel">
              Đã hoàn tiền</span
            >
          </div>
        </div>
        <div class="info-value info__rating">
          {{ renderRating(booking.TourSchedule.Tour.Rating) }}
        </div>
        <div class="info-value info--discount">
          <span>Khuyến mãi áp dụng: </span>
          <span v-if="booking.Discount">
            Giảm {{ booking.Discount.Percent }}%, tối đa
            {{ formatNumber(booking.Discount.MaxDiscount) }}vnd</span
          >
          <span v-else>Không</span>
        </div>
        <div class="info-value info--discount" v-if="booking.CancelReason">
          <span>Lý do hủy: </span>
          <span>
            {{ booking.CancelReason }}
          </span>
        </div>
        <div class="content--row" style="margin-top: 20px; justify-content: space-between">
          <div class="info-value info--price">
            <span>Tổng tiền: </span>
            <div v-if="!booking.Discount">{{ formatNumber(booking.Price) }} vnd</div>
            <div v-else>
              <span style="text-decoration: line-through"
                >{{ formatNumber(booking.Price) }} vnd</span
              >
              <span>{{ formatNumber(canculatePrice(booking)) }} vnd</span>
            </div>
          </div>
          <div class="action">
            <div
              class="btn btn--remove"
              @click="toggleCancelPopup"
              v-if="
                booking.Status == 0 ||
                (booking.Status == 1 &&
                  new Date(booking.TourSchedule.DateStart) >= new Date().setHours(0, 0, 0, 0))
              "
            >
              Hủy
            </div>
            <div
              class="btn btn--add"
              @click="handlePaymentBooking(booking.Id)"
              v-if="booking.Status === 0"
            >
              Thanh toán
            </div>
            <div class="popup-cancel" v-if="showCancelBooking">
              <div
                @click="toggleCancelPopup"
                style="
                  display: flex;
                  flex-direction: row;
                  justify-content: space-between;
                  align-items: center;
                "
              >
                <span style="font-size: 14px">Nhập lý do hủy</span>
                <img
                  src="../../assets/icon/close-48.png"
                  alt=""
                  style="width: 24px; height: 24px"
                />
              </div>
              <div>
                <textarea
                  type="text"
                  v-model="cancelReason"
                  rows="4"
                  style="border: 1px solid black; font-size: 14px"
                ></textarea>
              </div>
              <div style="display: flex; justify-content: end">
                <div
                  class="btn btn--add"
                  @click="handleCancelBooking(booking.Id)"
                  style="width: 32px; padding: 6px"
                >
                  Ok
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <ImageGallery
    :images="booking.TourSchedule.Tour.Image"
    :visible="showImagePopup"
    @close="toggleImagePopup"
  />
</template>

<script setup>
import '../../styles/base/button.css'

import ImageGallery from '@/components/ImageGallery.vue'
import { formatDate, formatNumber, getLinkImage } from '@/utils'
import { ref } from 'vue'

const showImagePopup = ref(false)
const showCancelBooking = ref(false)

function canculatePrice(booking) {
  const value = Math.round((booking.Discount.Percent * booking.Price) / 100)
  const discount = value <= booking.Discount.MaxDiscount ? value : booking.Discount.MaxDiscount
  return Math.round(booking.Price - discount)
}

const cancelReason = ref('')

// eslint-disable-next-line no-unused-vars
const props = defineProps({
  booking: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['cancelBooking', 'paymentBooking'])

function toggleCancelPopup() {
  showCancelBooking.value = !showCancelBooking.value
}

function toggleImagePopup() {
  showImagePopup.value = !showImagePopup.value
}
function renderRating(rating) {
  const fullStar = '★'
  const emptyStar = '☆'

  return fullStar.repeat(rating) + emptyStar.repeat(5 - rating)
}

function handleCancelBooking(id) {
  showCancelBooking.value = false
  emit('cancelBooking', id, cancelReason.value)
}

function handlePaymentBooking(id) {
  emit('paymentBooking', id)
}
</script>

<style scoped>
.booking-status {
  border-radius: 9999px;
  padding: 6px 12px;
  font-weight: 500;
  font-size: 16px;
  color: #ffffff;
}

.booking-status--cancel {
  background-color: #ff0000;
}

.booking-status--notpaid {
  background-color: #808081;
}

.booking-status--paid {
  background-color: #078cf8;
}

.info-time {
  width: 50%;
  font-size: 14px;
  justify-content: space-between;
}

.info--discount {
  font-size: 14px;
  font-weight: 500;
  color: #000000;
}

.info--discount span:first-child {
  margin-right: 6px;
  font-weight: bold;
}

.info--price {
  font-size: 14px;
  font-weight: bold;
  color: #078cf8;
}

.info--price span:first-child {
  font-size: 14px;
  color: #000000;
  margin-right: 6px;
}

.action {
  display: flex;
  flex-direction: row;
  width: 33%;
  gap: 10px;
  position: absolute;
  bottom: 20px;
  right: -80px;
}

.popup-cancel {
  position: absolute;
  border: 1px solid #808081;
  bottom: 110%;
  left: 0;
  border-radius: 6px;
  padding: 10px;
  box-shadow: 0 0 4px #0000001a;
}
</style>
