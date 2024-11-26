<template>
  <div class="form-container" id="hotelForm">
    <div class="form__content">
      <div class="form__header">
        <h2 class="form__title">Thông tin phòng</h2>
        <span class="error-message" v-if="errorMessage.length > 0"> {{ errorMessage }}</span>
        <button class="form__button" @click="closeForm">
          <img src="/src/assets/icon/close-48.png" alt="logo" />
        </button>
      </div>
      <div class="content--row">
        <div class="facilities">
          <div class="facility">
            <span class="facility__label">Chọn tiện nghi</span>
            <div v-for="facility in roomFacilities" :key="facility.Id" class="facility__item">
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
              <p class="form__label">Tên phòng <span class="required">*</span></p>
              <input type="text" class="form__input" v-model="room.Name" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Số lượng <span class="required">*</span></p>
              <input type="number" class="form__input" v-model="room.Quantity" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Diện tích <span class="required">*</span></p>
              <div><input type="text" class="form__input" v-model="room.Area" required />m2</div>
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Giá phòng <span class="required">*</span></p>
              <input type="text" class="form__input" v-model="room.Price" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Số lượng người lớn tối đa <span class="required">*</span></p>
              <input type="number" class="form__input" v-model="room.MaxAdultPeople" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Số lượng trẻ em tối đa <span class="required">*</span></p>
              <input type="number" class="form__input" v-model="room.MaxChildrenPeople" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--2">
              <p class="form__label">Số lượng giường đơn <span class="required">*</span></p>
              <input type="number" class="form__input" v-model="room.SingleBed" required />
            </div>
            <div class="form__item form__item--2">
              <p class="form__label">Số lượng giường đôi <span class="required">*</span></p>
              <input type="number" class="form__input" v-model="room.DoubleBed" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__item form__item--1">
              <p class="form__label">Hướng</p>
              <input type="text" class="form__input" v-model="room.Dirention" required />
            </div>
          </div>
          <div class="form-group">
            <div class="form__section--right content--column">
              <div class="form__item form__item--1 form-facility">
                <p class="form__label">Tiện nghi</p>
                <div class="multichoice">
                  <div v-for="facility in room.RoomFacility" :key="facility.Id">
                    <div class="facility__hotel">
                      <span>{{ facility.Facility.Name }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="form__footer">
            <button class="btn btn--add" @click="triggerFileInput">Thêm ảnh</button>
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
import { useUserStore } from '@/stores/user'

const hotelStore = useHotelStore()

const room = ref({})
const errorMessage = ref('')

const submitRoom = ref({})

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

  console.log(formData)

  const response = await hotelStore.addImageRoom(props.id, formData, userStore.getToken)

  if (response.success) {
    closeForm()
  } else {
    errorMessage.value = response.message
  }
}

const props = defineProps({
  id: {
    type: String,
    default: null
  },
  hotelId: {
    type: String,
    required: true
  }
})

const emits = defineEmits(['closeForm'])

function closeForm() {
  emits('closeForm')
}

function removeFacility(facility) {
  const newRoomFacility = room.value.RoomFacility.filter(
    (roomFacility) => roomFacility.Facility.Id != facility.Id
  )
  room.value.RoomFacility = newRoomFacility
}

function addFacility(facility) {
  room.value.RoomFacility.push({
    Facility: facility
  })
}

function checkContainFacility(id) {
  return (
    room.value &&
    Array.isArray(room.value.RoomFacility) &&
    room.value.RoomFacility.some((facility) => facility.Facility.Id === id)
  )
}
async function fetchRoomData() {
  if (props.id) {
    await hotelStore.fetchRoom(props.id)
    room.value = hotelStore.getRoom
  } else {
    room.value = { RoomFacility: [] }
  }
}

function formatRoomData() {
  submitRoom.value.Name = room.value.Name
  submitRoom.value.Area = room.value.Area
  submitRoom.value.Price = room.value.Price
  submitRoom.value.Quantity = room.value.Quantity
  submitRoom.value.MaxAdultPeople = room.value.MaxAdultPeople
  submitRoom.value.MaxChildrenPeople = room.value.MaxChildrenPeople
  submitRoom.value.SingleBed = room.value.SingleBed
  submitRoom.value.DoubleBed = room.value.DoubleBed
  submitRoom.value.Dirention = room.value.Dirention
  submitRoom.value.HotelId = props.hotelId

  console.log(submitRoom.value.HotelId)

  submitRoom.value.RoomFacility = room.value.RoomFacility.map((roomFacility) => ({
    FacilityId: roomFacility.Facility.Id
  }))
}

async function submitForm() {
  formatRoomData()
  if (!props.id) {
    const response = await hotelStore.createRoom(submitRoom.value, userStore.getToken)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  } else {
    const response = await hotelStore.updateRoom(props.id, submitRoom.value, userStore.getToken)
    if (response.success) {
      closeForm()
    } else {
      errorMessage.value = response.message
    }
  }
  console.log(submitRoom)
}

const userStore = useUserStore()

const roomFacilities = computed(() => hotelStore.getRoomFacilities)

onMounted(() => {
  fetchRoomData()
  hotelStore.fetchFacilities(3)
})
</script>
