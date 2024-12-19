import axios from 'axios'

export default {
  async adminStatistical(start, end, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Statiscals/admin`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          start: start,
          end: end
        }
      })

      this.statiscal = response.data
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async hotelStatistical(start, end, userId, hotelId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Statiscals/hotel`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          start: start,
          end: end,
          userId: userId,
          hotelId: hotelId
        }
      })

      this.statiscal = response.data
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async tourStatistical(start, end, userId, tourId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Statiscals/tour`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          start: start,
          end: end,
          userId: userId,
          tourId: tourId
        }
      })

      this.statiscal = response.data
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  reset() {
    this.statiscal = null
  }
}
