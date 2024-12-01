import { defineStore } from 'pinia'
import state from './state'
import actions from './actions'
import getters from './getters'

export const useDiscountStore = defineStore('discount', {
  state,
  actions,
  getters
})
