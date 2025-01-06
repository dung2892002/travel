<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Thanh toán cho đối tác</h1>
    </div>
    <div class="content-main">
      <div class="toolbar">
        <span class="error-message" v-if="errorMessage">{{ errorMessages }}</span>
        <span class="success-message" v-if="successMesage">{{ successMesage }}</span>
      </div>
      <div class="main-container">
        <table class="travel-table">
          <thead>
            <tr>
              <th class="w-6">STT</th>
              <th class="w-16">Email</th>
              <th class="w-16">Số điện thoại</th>
              <th class="w-16">Số tài khoản</th>
              <th class="w-16">Ngân hàng</th>
              <th>Số dư</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(wallet, index) in wallets" :key="wallet.UserId">
              <td>{{ index + 1 }}</td>
              <td>{{ wallet.Email }}</td>
              <td>{{ wallet.PhoneNumber }}</td>
              <td>{{ wallet.BankNumber }}</td>
              <td>{{ wallet.BankName }}</td>
              <td>
                <div class="action">
                  <span>{{ formatNumber(wallet.Balance) }} vnd</span>
                  <div class="action-buttons">
                    <button
                      class="action-button btn--add"
                      @click="handlePaymentAccount(wallet.UserId)"
                    >
                      <img src="../../assets/icon/payment.png" alt="lock" />
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
import { errorMessages } from 'vue/compiler-sfc'
import { formatNumber } from '@/utils'

const pageNumber = ref(1)
const errorMessage = ref(null)
const successMesage = ref(null)

const userStore = useUserStore()

function handlePageChange(newPageNumber) {
  pageNumber.value = newPageNumber
  fetchWallets()
}

async function handlePaymentAccount(id) {
  const result = await userStore.paymentForWallet(id, token.value)
  if (result.success) {
    errorMessage.value = null
    successMesage.value = 'Thanh toán cho đối tác thành công'
    fetchWallets()
  } else {
    successMesage.value = null
    errorMessage.value = result.message
  }
}

async function fetchWallets() {
  await userStore.fetchWalletPositive(token.value, pageNumber.value)
}

const token = computed(() => userStore.getToken)
const totalPages = computed(() => userStore.getTotalPages)
const totalItems = computed(() => userStore.getTotalItems)
const wallets = computed(() => userStore.getWallets)

onMounted(() => {
  fetchWallets()
})
</script>

<style scoped>
.role--selected {
  background-color: red;
}
</style>
