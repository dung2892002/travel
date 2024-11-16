<template>
  <div>
    <h2>Đăng nhập</h2>
    <input v-model="username" placeholder="Username" />
    <input v-model="password" type="password" placeholder="Password" />
    <button @click="login">Login</button>
    <p v-if="errorMessage" style="color: red">{{ errorMessage }}</p>
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

const login = async () => {
  const credentials = {
    Username: username.value,
    Password: password.value
  }
  const response = await userStore.login(credentials)

  if (response.success) {
    router.push('/')
  } else {
    errorMessage.value = response.message
  }
}
</script>
