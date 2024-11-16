<template>
  <div>
    <h2>Đăng ký</h2>
    <input v-model="fullname" placeholder="Họ tên" />
    <input v-model="phoneNumber" placeholder="Số điện thoại" />
    <input v-model="email" placeholder="Email" />
    <input v-model="username" placeholder="Username" />
    <input v-model="password" type="password" placeholder="Mật khẩu" />
    <input v-model="rePassword" type="password" placeholder="Nhập lại mật khẩu" />

    <button @click="register">Đăng ký</button>
    <p v-if="errorMessage" style="color: red">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useUserStore } from '../../stores/user/index'
import { useRouter } from 'vue-router'

const userStore = useUserStore()
const router = useRouter()
const fullname = ref('')
const phoneNumber = ref('')
const username = ref('')
const password = ref('')
const rePassword = ref('')
const email = ref('')
const errorMessage = ref('')

const register = async () => {
  const credentials = {
    Fullname: fullname.value,
    PhoneNumber: phoneNumber.value,
    Username: username.value,
    Password: password.value,
    RePassword: rePassword.value,
    Email: email.value
  }
  const response = await userStore.register(credentials)

  if (response.success) {
    router.push('/login')
  } else {
    errorMessage.value = response.message
  }
}
</script>
