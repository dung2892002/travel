<template>
  <div class="detail-section">
    <div class="content__header">
      <h4 class="content__title">Ngày {{ tourDay.DayNumber }}: {{ tourDay.Description }}</h4>
      <div
        @click="toggleShowActivity"
        style="cursor: pointer; font-size: 14px; font-weight: bold; color: #078cf8"
      >
        <span class="button--add-text" v-if="!showActivity">Chi tiết</span>
        <span class="button--add-text" v-else>Ẩn</span>
      </div>
    </div>
    <div class="content-header content--column" v-if="showActivity">
      <ul>
        <li v-for="activity in tourDay.Activity" :key="activity.Id" style="margin-bottom: 4px">
          - {{ activity.Description }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const showActivity = ref(false)

function toggleShowActivity() {
  showActivity.value = !showActivity.value
}

// eslint-disable-next-line no-unused-vars
const props = defineProps({
  tourDay: {
    type: Object,
    required: true
  }
})
</script>
