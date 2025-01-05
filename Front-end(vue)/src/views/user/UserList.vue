<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý tài khoản</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text" @click="showForm = true">Thêm mới</span>
      </button>
    </div>
    <div class="content-main">
      <div class="toolbar">
        <div class="toolbar__search">
          <input
            class="form__input"
            type="text"
            id="search-employee"
            placeholder="Nhập để tìm kiếm..."
            v-model="keyword"
          />
          <img src="../../assets/icon/search.png" class="search-logo" @click="fetchUsers()" />
        </div>
        <div class="toolbar__actions">
          <button class="toolbar-action" @click="fetchUsers">
            <img src="/src/assets/icon/refresh.png" alt="logo" />
          </button>
        </div>
      </div>
      <div class="main-container">
        <table class="travel-table">
          <thead>
            <tr>
              <th class="w-6">STT</th>
              <th class="w-18">Username</th>
              <th class="w-26">Email</th>
              <th class="w-12">Số điện thoại</th>
              <th class="w-20">Tên hiển thị</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, index) in users" :key="user.Id">
              <td>{{ index + 1 }}</td>
              <td>{{ user.Username }}</td>
              <td>{{ user.Email }}</td>
              <td>{{ user.PhoneNumber }}</td>
              <td>{{ user.DisplayName }}</td>
              <td>
                <div class="action">
                  <div class="action-buttons">
                    <button
                      class="action-button btn--remove"
                      @click="handleLockAccount(user)"
                      v-if="!user.IsLocked"
                    >
                      <img src="../../assets/icon/lock.png" alt="lock" />
                    </button>
                    <button
                      class="action-button btn--add"
                      @click="handleUnlockAccount(user)"
                      v-else
                    >
                      <img src="../../assets/icon/unlock.png" alt="unlock" />
                    </button>
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <ThePagnigation
      :pageNumber="pageNumber"
      :totalItems="totalItems"
      :totalPages="totalPages"
      @pageChange="handlePageChange"
    />
  </div>
  <div class="form-container" v-if="showForm">
    <div class="form__content" style="width: 480px; height: auto; overflow-y: auto">
      <div class="form__header">
        <h2 class="form__title">Thông tin tài khoản</h2>
        <button class="form__button" @click="showForm = false">
          <img src="/src/assets/icon/close-48.png" alt="logo" />
        </button>
      </div>
      <span class="error-message" v-if="errorMessage"> {{ errorMessage }}</span>
      <div class="travel-form">
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Username <span class="required">*</span></p>
            <input type="text" class="form__input" v-model="user.Username" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Email <span class="required">*</span></p>
            <input type="email" class="form__input" v-model="user.Email" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Số điện thoại <span class="required">*</span></p>
            <input type="text" class="form__input" v-model="user.PhoneNumber" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Họ tên<span class="required">*</span></p>
            <input type="text" class="form__input" v-model="user.Fullname" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Mật khẩu<span class="required">*</span></p>
            <input type="password" class="form__input" v-model="user.Password" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Nhập lại mật khẩu<span class="required">*</span></p>
            <input type="password" class="form__input" v-model="user.RePassword" required />
          </div>
        </div>
        <div class="form-group">
          <div class="form__item form__item--1">
            <p class="form__label">Vai trò<span class="required">*</span></p>
            <div style="display: flex; flex-direction: row; gap: 10px">
              <button
                class="btn btn--add"
                @click="selectRole(5)"
                :class="{ 'role--selected': user.Role === 5 }"
              >
                Đối tác khách sạn
              </button>
              <button
                class="btn btn--add"
                @click="selectRole(6)"
                :class="{ 'role--selected': user.Role === 6 }"
              >
                Đối tác tour
              </button>
              <button
                class="btn btn--add"
                @click="selectRole(10)"
                :class="{ 'role--selected': user.Role === 10 }"
              >
                Admin
              </button>
            </div>
          </div>
        </div>
        <div class="form__footer">
          <button class="btn btn--add" id="submitButton" @click="submitForm">Lưu</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import '../../styles/layout/toolbar.css'
import '../../styles/layout/table.css'
import '../../styles/base/input.css'
import '../../styles/base/button.css'

import ThePagnigation from '@/components/ThePagnigation.vue'
import { useUserStore } from '@/stores/user'
import { computed, onMounted, ref } from 'vue'

const keyword = ref(null)
const pageSize = ref(20)
const pageNumber = ref(1)
const showForm = ref(false)
const errorMessage = ref('')

const user = ref({
  Username: '',
  Email: '',
  PhoneNumber: '',
  Fullname: '',
  Password: '',
  RePassword: '',
  Role: 5
})

const userStore = useUserStore()

function handlePageChange(newPageNumber) {
  pageNumber.value = newPageNumber
  fetchUsers()
}

function validateForm(user) {
  // const usernameRegex = /^[a-z]{6,}$/
  // if (!usernameRegex.test(user.Username)) {
  //   errorMessage.value = 'Username chỉ được chứa chữ thường và phải từ 6 ký tự trở lên.'
  //   return false
  // }

  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/ // Chữ hoa, thường, số, ký tự đặc biệt, ít nhất 8 ký tự
  if (!passwordRegex.test(user.Password)) {
    errorMessage.value =
      'Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.'
    return false
  }

  if (user.Password != user.RePassword) {
    errorMessage.value = 'Mật khẩu xác nhận phải giống với mật khẩu'
    return false
  }
  return true
}

function selectRole(value) {
  user.value.Role = value
}

async function handleLockAccount(user) {
  const response = await userStore.lockUsers(user.Id, token.value)
  if (response.success) fetchUsers()
}

async function handleUnlockAccount(user) {
  const response = await userStore.unlockUsers(user.Id, token.value)
  if (response.success) fetchUsers()
}

async function submitForm() {
  if (validateForm(user.value)) {
    if (user.value.Role === 5) {
      const response = await userStore.createHotelPartner(user.value, token.value)
      if (response.success) {
        showForm.value = false
        fetchUsers()
      } else {
        errorMessage.value = response.message
      }
    }
    if (user.value.Role === 6) {
      const response = await userStore.createTourPartner(user.value, token.value)
      if (response.success) {
        showForm.value = false
        fetchUsers()
      } else {
        errorMessage.value = response.message
      }
    }
    if (user.value.Role === 10) {
      const response = await userStore.createAdmin(user.value, token.value)
      if (response.success) {
        showForm.value = false
        fetchUsers()
      } else {
        errorMessage.value = response.message
      }
    }
    console.log('create')
  } else {
    console.log('error')
  }
}

async function fetchUsers() {
  await userStore.fetchUsers(keyword.value, pageSize.value, pageNumber.value, token.value)
}

const users = computed(() => userStore.getUsers)
const token = computed(() => userStore.getToken)
const totalPages = computed(() => userStore.getTotalPages)
const totalItems = computed(() => userStore.getTotalItems)

onMounted(() => {
  fetchUsers()
})
</script>

<style scoped>
.role--selected {
  background-color: red;
}
</style>
