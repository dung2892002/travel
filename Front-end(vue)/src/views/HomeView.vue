<template>
  <main>
    <img
      src="https://ik.imagekit.io/tvlk/image/imageResource/2025/01/05/1736039105980-f92c5570060d22e1a5d244eed8bc7f87.png?tr=dpr-2,q-75"
      alt=""
      style="height: 420px; width: 100%"
    />
    <div style="padding: 0 100px; margin-top: 20px">
      <h3>Đặt phòng khách sạn</h3>
      <div class="content--row" style="margin-top: 10px">
        <div v-for="(hotel, index) in hotels" :key="index" class="item-ads" @click="goHotel">
          <span>{{ hotel.Name }}</span>
          <span>{{ hotel.TotalHotel }} khách sạn</span>
        </div>
      </div>
    </div>
    <div style="padding: 0 100px; margin-top: 20px">
      <h3>Đặt tour du lịch</h3>
      <div class="content--row" style="margin-top: 10px">
        <div v-for="(tour, index) in tours" :key="index" class="item-ads" @click="goTour">
          <span>{{ tour.Name }}</span> <span>{{ tour.TotalTour }} tour</span>
        </div>
      </div>
    </div>
  </main>
</template>
<script setup>
import { useLocationStore } from '@/stores/location'
import { computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

function goHotel() {
  router.push({
    name: 'hotel'
  })
}

function goTour() {
  router.push({
    name: 'tour'
  })
}

const locationStore = useLocationStore()

const hotels = computed(() => locationStore.getProvincesHotel)
const tours = computed(() => locationStore.getProvincesTour)

onMounted(() => {
  locationStore.fetchProvincesTour()
  locationStore.fetchProvincesHotel()
})
</script>

<style scoped>
.item-ads {
  width: 240px;
  height: 120px;
  border: 1px solid #078cf8;
  border-radius: 12px;
  padding: 6px;
  margin-right: 20px;
  background-color: #30c5f7;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
}

.item-ads span {
  font-size: 20px;
  font-weight: bold;
  color: white;
}
</style>
