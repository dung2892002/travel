<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý phòng</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div class="content-main">
      <RoomItem
        v-for="room in rooms"
        :room="room"
        :mode="1"
        :key="room.Id"
        @updateRoom="updateRoom(room.Id)"
      ></RoomItem>
    </div>
  </div>
  <RoomForm
    v-if="showDetail"
    @closeForm="resetDefault"
    :id="roomUpdateId"
    :hotelId="hotelId"
  ></RoomForm>
</template>

<script setup>
import { useHotelStore } from '@/stores/hotel'
import { computed, onMounted, ref } from 'vue'
import '../../styles/base/button.css'
import RoomItem from './RoomItem.vue'
import { useRoute } from 'vue-router'
import RoomForm from './RoomForm.vue'

const showDetail = ref(false)
const roomUpdateId = ref(null)

const route = useRoute()
const hotelId = route.params.hotelId

const hotelStore = useHotelStore()

function addNew() {
  showDetail.value = true
  roomUpdateId.value = null
}

function updateRoom(id) {
  showDetail.value = true
  roomUpdateId.value = id
}

function resetDefault() {
  showDetail.value = false
  roomUpdateId.value = null
  fetchRooms()
}

function fetchRooms() {
  hotelStore.fetchRoomByHotel(hotelId)
}

const rooms = computed(() => hotelStore.getRooms)

onMounted(() => {
  fetchRooms()
})
</script>
