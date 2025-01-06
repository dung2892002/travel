<template>
  <div>
    <h2>Đăng nhập</h2>
    <input v-model="username" placeholder="Username" />
    <input v-model="password" type="password" placeholder="Password" />
    <button @click="login">Login</button>
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
      <span @click="goRegisterPage" style="font-size: 14px; color: #078cf8; cursor: pointer"
        >Đăng ký</span
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
const username = ref('')
const password = ref('')
const errorMessage = ref('')

function goRegisterPage() {
  router.push({
    name: 'register'
  })
}

function goChangPasswordPage() {
  router.push({
    name: 'change-password'
  })
}

const login = async () => {
  const credentials = {
    Username: username.value,
    Password: password.value
  }
  const response = await userStore.login(credentials)

  if (response.success) {
    router.push({
      name: 'profile'
    })
  } else {
    errorMessage.value = response.message
  }
}
</script>

<style scoped>
div {
  max-width: 400px;
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
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #0056b3;
}

p {
  margin-top: 10px;
  text-align: center;
  font-size: 14px;
}
</style>
