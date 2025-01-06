<template>
  <div class="content content--column" style="gap: 20px">
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
    <div class="content" v-if="checkRole(5) || checkRole(6)">
      <div class="form__header">
        <div>
          <h2>Thông tin ví</h2>
          <span class="error-message" v-if="errorMessageWallet.length > 0">
            {{ errorMessageWallet }}</span
          >
          <span class="success-message" v-if="successMessageWallet.length > 0">
            {{ successMessageWallet }}</span
          >
        </div>
        <div v-if="!wallet"><button class="btn btn--add" @click="showForm = 1">Tạo ví</button></div>
      </div>

      <div class="travelform" v-if="wallet">
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Số dư<span class="required">*</span></p>
            <input type="text" class="form__input" :value="formatNumber(wallet.Balance)" disabled />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Số tài khoản<span class="required">*</span></p>
            <input type="text" class="form__input" v-model="wallet.BankNumber" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Ngân hàng<span class="required">*</span></p>
            <input type="text" class="form__input" v-model="wallet.BankName" required />
          </div>
        </div>
        <div class="form__footer">
          <button class="btn btn--add" id="submitButton" @click="updateWallet">Lưu</button>
        </div>
      </div>
      <div class="travelform" v-if="!wallet && showForm">
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Số dư<span class="required">*</span></p>
            <input type="text" class="form__input" value="0" disabled />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Số tài khoản<span class="required">*</span></p>
            <input type="text" class="form__input" v-model="submitWallet.BankNumber" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Ngân hàng<span class="required">*</span></p>
            <input type="text" class="form__input" v-model="submitWallet.BankName" required />
          </div>
        </div>
        <div class="form__footer">
          <button class="btn btn--add" id="submitButton" @click="createWallet">Lưu</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/form.css'

import { useUserStore } from '@/stores/user'
import { formatNumber } from '@/utils'
import { computed, onMounted, ref } from 'vue'

const userStore = useUserStore()

const errorMessage = ref('')
const errorMessageWallet = ref('')
const successMessageWallet = ref('')
const showForm = ref(false)

const submitWallet = ref({
  BankName: '',
  BankNumber: ''
})

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

async function updateWallet() {
  var resposne = await userStore.updateWallet(wallet.value, user.value.Id)
  if (resposne.success) {
    successMessageWallet.value = 'Cập nhật thông tin ví thành công'
  } else errorMessageWallet.value = resposne.message
}

async function createWallet() {
  submitWallet.value.UserId = user.value.Id
  var resposne = await userStore.createWallet(submitWallet.value)
  if (resposne.success) {
    successMessageWallet.value = 'Tạo mới ví thành công'
  } else errorMessageWallet.value = resposne.message
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

function checkRole(value) {
  return user.value.Roles.some((role) => role.RoleValue === value)
}

const user = computed(() => userStore.getUser)
const token = computed(() => userStore.getToken)
const wallet = computed(() => userStore.getWallet)
onMounted(() => {
  userStore.fetchWallet(userStore.getUser.Id)
})
</script>
