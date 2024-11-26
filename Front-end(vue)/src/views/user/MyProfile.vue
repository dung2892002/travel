<template>
  <div class="content">
    <div class="form__header">
      <h2>Thông tin cá nhân</h2>
      <span class="error-message" v-if="errorMessage.length > 0"> {{ errorMessage }}</span>
    </div>

    <div class="travelform">
      <div class="form-group">
        <div class="form__item form__item--1">
          <p class="form__label">Họ tên<span class="required">*</span></p>
          <input type="text" class="form__input" v-model="user.Fullname" required />
        </div>
      </div>
      <div class="form-group">
        <div class="form__item form__item--1">
          <p class="form__label">Tên hiển thị<span class="required">*</span></p>
          <input type="text" class="form__input" v-model="user.DisplayName" required />
        </div>
      </div>
      <div class="form-group">
        <div class="form__item form__item--1">
          <p class="form__label">Email<span class="required">*</span></p>
          <input type="text" class="form__input" v-model="user.Email" required />
        </div>
      </div>
      <div class="form-group">
        <div class="form__item form__item--1">
          <p class="form__label">Số điện thoại<span class="required">*</span></p>
          <input type="text" class="form__input" v-model="user.PhoneNumber" required />
        </div>
      </div>
    </div>
    <div class="form__footer">
      <button class="btn btn--add" id="submitButton" @click="submitForm">Lưu</button>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/form.css'

import { useUserStore } from '@/stores/user'
import { computed, ref } from 'vue'

const userStore = useUserStore()

const errorMessage = ref('')

async function submitForm() {
  if (!validateForm()) return
  const response = await userStore.updateUser(user.value.Id, user.value, token.value)
  if (response.success) {
    console.log('success')
    errorMessage.value = ''
  } else {
    errorMessage.value = response.message
  }
}

function validateForm() {
  if (
    !user.value.Fullname ||
    !user.value.DisplayName ||
    !user.value.Email ||
    !user.value.PhoneNumber
  ) {
    errorMessage.value = 'Vui lòng điền đầy đủ thông tin'
    return false
  }
  // Kiểm tra email hợp lệ
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(user.value.Email)) {
    errorMessage.value = 'Email không hợp lệ'
    return false
  }
  // Kiểm tra số điện thoại hợp lệ (tùy theo yêu cầu)
  if (!/^[0-9]{10,11}$/.test(user.value.PhoneNumber)) {
    errorMessage.value = 'Số điện thoại không hợp lệ'
    return false
  }
  return true
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
</script>
