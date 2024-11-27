<template>
  <div class="pagination">
    <div class="pagination-section">
      <span>Tổng: {{ totalItems }}</span>
    </div>
    <div class="pagination-section">
      <div class="control_page">
        <button id="prev-page" class="button--prev" @click="previousPage">
          <img src="/src/assets/icon/btn-prev-page.svg" alt="logo" />
        </button>
        <div class="current-page">{{ localPageNumber }}</div>
        <button id="next-page" class="button--next" @click="nextPage">
          <img src="/src/assets/icon/btn-next-page.svg" alt="logo" />
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'

import '/src/styles/layout/pagination.css'
const props = defineProps({
  pageNumber: {
    type: Number,
    default: 1
  },
  totalItems: {
    type: Number,
    default: 1
  },
  totalPages: {
    type: Number,
    default: 1
  }
})

const emits = defineEmits(['pageChange'])

const localPageNumber = ref(props.pageNumber)

watch(
  () => props.pageNumber,
  (newValue) => {
    localPageNumber.value = newValue
  }
)

function nextPage() {
  if (localPageNumber.value < props.totalPages) {
    localPageNumber.value++
    emits('pageChange', localPageNumber.value)
  }
}

function previousPage() {
  if (localPageNumber.value > 1) {
    localPageNumber.value--
    emits('pageChange', localPageNumber.value)
  }
}
</script>
