<template>
  <div>
    <h3>Quản lý mục yêu thích</h3>
    <div v-if="favourites && favourites.length > 0">
      <div v-for="favourite in favourites" :key="favourite.Id">
        <HotelItem
          v-if="favourite.Hotel"
          :hotel="favourite.Hotel"
          :mode="3"
          @selectHotel="deleteFavouriteHotel(favourite.Id)"
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

const favouriteStore = useFavouriteStore()
const userStore = useUserStore()

const favourites = computed(() => favouriteStore.getUserFavourites)
const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)

async function deleteFavouriteHotel(id) {
  const response = await favouriteStore.deleteFavourite(id, token.value)

  if (response.success) {
    fetchFavourties()
  } else {
    console.log('co loi xay ra')
  }
}

async function fetchFavourties() {
  await favouriteStore.fetchUserFavourite(user.value.Id)
}

onMounted(() => {
  fetchFavourties()
})
</script>
