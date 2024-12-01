<template>
  <div class="content content--column">
    <div class="content__header">
      <h1 class="content__title">Quản lý khuyến mãi</h1>
      <button class="content__button btn btn--add" @click="addNew">
        <span class="button--add-text">Thêm mới</span>
      </button>
    </div>
    <div class="content--column">
      <div v-for="discount in discounts" :key="discount.Id" class="discount">
        <span>Giảm giá {{ discount.Percent }}%</span>
        <span
          >Giảm tối đa {{ formatNumber(discount.MaxDiscount) }} vnd cho hóa đơn từ
          {{ formatNumber(discount.MinPrice) }} vnd</span
        >
        <span
          >Áp dụng từ ngày {{ formatDate(discount.Start) }} đến hết
          {{ formatDate(discount.End) }}</span
        >
      </div>
    </div>
  </div>

  <DiscountForm @close-form="closeForm" v-if="showDetail" />
</template>

<script setup>
import { useDiscountStore } from '@/stores/discount'
import { useUserStore } from '@/stores/user'
import { computed, onMounted, ref } from 'vue'
import DiscountForm from './DiscountForm.vue'
import { formatDate, formatNumber } from '@/utils'

const userStore = useUserStore()
const discountStore = useDiscountStore()

const showDetail = ref(false)

function addNew() {
  showDetail.value = true
}

function closeForm() {
  showDetail.value = false
  fetchDiscounts()
}

async function fetchDiscounts() {
  await discountStore.fetchUserDiscounts(user.value.Id)
}

const discounts = computed(() => discountStore.getUserDiscounts)
const user = computed(() => userStore.getUser)
// const token = computed(() => userStore.getToken)
onMounted(() => {
  fetchDiscounts(userStore.getUser.Id)
})
</script>

<style scoped>
.discount {
  display: flex;
  flex-direction: column;
  padding: 6px;
  border-radius: 6px;
  line-height: 20px;
  border: 1px solid gainsboro;
  box-shadow: 4px 8px 4px rgba(0, 0, 0, 0.2);
}
</style>
