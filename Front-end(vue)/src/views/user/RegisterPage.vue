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
    <div
      style="
        padding: 0;
        margin: 10px 0;
        border: 0;
        box-shadow: none;
        display: flex;
        justify-content: space-between;
      "
    >
      <span @click="goLoginPage" style="font-size: 14px; color: #078cf8; cursor: pointer"
        >Đăng nhập</span
      >
      <span @click="goChangPasswordPage" style="font-size: 14px; color: #078cf8; cursor: pointer"
        >Đổi mật khẩu</span
      >
    </div>
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

function goLoginPage() {
  router.push({
    name: 'login'
  })
}

function goChangPasswordPage() {
  router.push({
    name: 'change-password'
  })
}

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

<style scoped>
div {
  max-width: 500px;
  margin: 50px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  background-color: #f9f9f9;
}

h2 {
  text-align: center;
  margin-bottom: 20px;
  color: #333;
  font-size: 24px;
}

input {
  width: 100%;
  padding: 10px;
  margin: 10px 0;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 14px;
}

button {
  width: 100%;
  padding: 10px;
  background-color: #078cf8;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s ease;
}

p {
  margin-top: 10px;
  text-align: center;
  font-size: 14px;
  color: red;
}

@media (max-width: 768px) {
  div {
    padding: 15px;
  }

  h2 {
    font-size: 20px;
  }

  input,
  button {
    font-size: 14px;
  }
}
</style>
