<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý tài khoản</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
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

const userStore = useUserStore()

function handlePageChange(newPageNumber) {
  pageNumber.value = newPageNumber
  fetchUsers()
}

async function handleLockAccount(user) {
  const response = await userStore.lockUsers(user.Id, token.value)
  if (response.success) fetchUsers()
}

async function handleUnlockAccount(user) {
  const response = await userStore.unlockUsers(user.Id, token.value)
  if (response.success) fetchUsers()
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
