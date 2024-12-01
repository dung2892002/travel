import axios from 'axios'

export default {
  async fetchHotelDiscounts(hotelId, price) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Discounts/hotel`, {
        params: {
          hotelId: hotelId,
          price: price
        }
      })
      const discounts = response.data
      this.hotelDiscounts = discounts
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchTourDiscounts(tourId, price) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Discounts/tour`, {
        params: {
          tourId: tourId,
          price: price
        }
      })
      const discounts = response.data
      this.tourDiscounts = discounts
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchUserDiscounts(userId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Discounts/user`, {
        params: {
          userId: userId
        }
      })
      const discounts = response.data
      this.userDiscounts = discounts
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createHotelDiscount(discounts, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Discounts/hotel`, discounts, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })
      if (response.status === 201) {
        return { success: true }
      }
    } catch (error) {
      console.error('Error in create favourite:', error)
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async createTourDiscount(discounts, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Discounts/tour`, discounts, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      if (response.status === 201) {
        return { success: true }
      }
    } catch (error) {
      console.error('Error in create favourite:', error)
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async createDiscount(discounts, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Discounts`, discounts, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      if (response.status === 201) {
        return { success: true }
      }
    } catch (error) {
      console.error('Error in create favourite:', error)
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
