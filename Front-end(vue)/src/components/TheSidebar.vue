<template>
  <div class="sidebar" v-if="user">
    <div class="sidebar__item user-info">
      <div class="user-info" @click="triggerImageInput">
        <div class="sidebar-image">
          <img :src="getLinkImage(user.AvatarImage)" alt="anh_dai_dien" />
          <div class="hover-overlay">Thay ảnh</div>
        </div>
        <input
          type="file"
          accept="image/*"
          ref="fileInput"
          class="hidden-input"
          @change="handleImageChange"
        />
      </div>
      <div class="user-info">
        {{ user.DisplayName }}
      </div>
    </div>
    <div class="sidebar__item" v-if="checkRole(1)">
      <RouterLink :to="{ name: 'my-booking-room' }" class="sidebar__link">
        <img src="../assets/icon/booking.png" alt="icon" class="link-icon" />
        <p>Booking</p>
      </RouterLink>
      <RouterLink :to="{ name: 'profile' }" class="sidebar__link">
        <img src="../assets/icon/user.png" alt="icon" class="link-icon" />
        <p>Thông tin</p>
      </RouterLink>
      <RouterLink :to="{ name: 'myFavourite' }" class="sidebar__link">
        <img src="../assets/icon/love.png" alt="icon" class="link-icon" />
        <p>Yêu thích</p>
      </RouterLink>
    </div>
    <div class="sidebar__item" v-if="checkRole(5)">
      <RouterLink :to="{ name: 'statistical' }" class="sidebar__link">
        <img src="../assets/icon/graph.png" alt="icon" class="link-icon" />
        <p>Thống kê khách sạn</p></RouterLink
      >
      <RouterLink :to="{ name: 'my-hotel' }" class="sidebar__link">
        <img src="../assets/icon/hotel.png" alt="icon" class="link-icon" />
        <p>Quản lý khách sạn</p>
      </RouterLink>
    </div>
    <div class="sidebar__item" v-if="checkRole(6)">
      <RouterLink :to="{ name: 'statistical' }" class="sidebar__link">
        <img src="../assets/icon/graph.png" alt="icon" class="link-icon" />
        <p>Thống kê tour</p></RouterLink
      >
      <RouterLink :to="{ name: 'myTour' }" class="sidebar__link">
        <img src="../assets/icon/tour.png" alt="icon" class="link-icon" />
        <p>Quản lý tour</p>
      </RouterLink>
    </div>
    <div class="sidebar__item" v-if="checkRole(10)">
      <RouterLink :to="{ name: 'statistical' }" class="sidebar__link">
        <img src="../assets/icon/graph.png" alt="icon" class="link-icon" />
        <p>Thống kê</p></RouterLink
      >
      <RouterLink :to="{ name: 'userList' }" class="sidebar__link">
        <img src="../assets/icon/dic-employee.png" alt="icon" class="link-icon" />
        <p>Quản lý tài khoản</p></RouterLink
      >
      <RouterLink :to="{ name: 'walletList' }" class="sidebar__link">
        <img src="../assets/icon/payment.png" alt="icon" class="link-icon" />
        <p>Thanh toán cho đối tác</p></RouterLink
      >
    </div>
    <div class="sidebar__item" v-if="checkRole(5) || checkRole(6) || checkRole(10)">
      <RouterLink :to="{ name: 'myDiscount' }" class="sidebar__link">
        <img src="../assets/icon/discount.png" alt="icon" class="link-icon" />
        <p>Khuyến mãi</p></RouterLink
      >
    </div>
    <button @click="logout" class="sidebar__button">Đăng xuất</button>
  </div>
</template>

<script setup>
import { useUserStore } from '@/stores/user'
import '../styles/base/button.css'
import '../styles/layout/sidebar.css'
import { computed, ref } from 'vue'
import { getLinkImage } from '@/utils'
import { useRouter } from 'vue-router'

const userStore = useUserStore()
const router = useRouter()

const fileInput = ref(null)

function triggerImageInput() {
  fileInput.value.click()
}

const logout = () => {
  userStore.logout()
  router.push({
    name: 'login'
  })
}

async function handleImageChange(event) {
  const file = event.target.files[0]
  if (file) {
    const formData = new FormData()
    formData.append('file', file)
    const response = await userStore.changeImage(user.value.Id, formData, token.value)
    if (response.success) {
      console.log('success')
    } else {
      console.log(response.message)
    }
  }
}

function checkRole(value) {
  return user.value.Roles.some((role) => role.RoleValue === value)
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
</script>
