<template>
  <div>
    <h3>Quản lý mục yêu thích</h3>
    <div v-if="favourites && favourites.length > 0">
      <div v-for="favourite in favourites" :key="favourite.Id">
        <HotelItem
          v-if="favourite.HotelId"
          :hotel="favourite.Hotel"
          :mode="3"
          @selectHotel="deleteFavourite(favourite.Id)"
        />
        <TourItem
          v-if="favourite.TourId"
          :tour="favourite.Tour"
          :mode="3"
          @selectHotel="deleteFavourite(favourite.Id)"
        />
      </div>
    </div>
  </div>
</template>

<script setup>
import { useFavouriteStore } from '@/stores/favourite'
import { useUserStore } from '@/stores/user'
import { computed, onMounted } from 'vue'
import HotelItem from '../hotel/HotelItem.vue'
import TourItem from '../tour/TourItem.vue'

const favouriteStore = useFavouriteStore()
const userStore = useUserStore()

const favourites = computed(() => favouriteStore.getUserFavourites)
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

async function deleteFavourite(id) {
  await favouriteStore.deleteFavourite(id, token.value)

  fetchFavourties()
}

async function fetchFavourties() {
  await favouriteStore.fetchUserFavourite(user.value.Id)
}

onMounted(() => {
  fetchFavourties()
})
</script>
