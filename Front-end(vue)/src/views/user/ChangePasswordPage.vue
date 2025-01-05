<template>
  <div>
    <h2>Đổi mật khẩu</h2>
    <input v-model="username" placeholder="Username" />
    <input v-model="password" placeholder="Mật khẩu hiện tại" type="password" />
    <input v-model="newPassword" placeholder="Mật khẩu mới" type="password" />
    <input v-model="reNewPassword" placeholder="Xác nhận mật khẩu" type="password" />

    <button @click="register">Đổi mật khẩu</button>
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
      <span @click="goRegisterPage" style="font-size: 14px; color: #078cf8; cursor: pointer"
        >Đăng ký</span
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
const newPassword = ref('')
const reNewPassword = ref('')
const errorMessage = ref('')

function goLoginPage() {
  router.push({
    name: 'login'
  })
}

function goRegisterPage() {
  router.push({
    name: 'register'
  })
}

const register = async () => {
  const credentials = {
    Username: username.value,
    Password: password.value,
    NewPassword: newPassword.value,
    ReNewPassword: reNewPassword.value
  }
  const response = await userStore.changePassword(credentials)

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
