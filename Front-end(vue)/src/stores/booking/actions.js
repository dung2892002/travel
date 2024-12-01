import axios from 'axios'

export default {
  async fetchMyBooking(userId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response1 = await axios.get(`${apiServer}/BookingsRoom/user`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: userId
        }
      })
      const bookingsRoom = response1.data

      const response2 = await axios.get(`${apiServer}/BookingsTour/user`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: userId
        }
      })
      const bookingsTour = response2.data

      this.myBookingsRoom = bookingsRoom
      this.myBookingsTour = bookingsTour
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createBookingRoom(booking, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/BookingsRoom`, booking, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 201) return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async cancelBookingRoom(id, reason, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/BookingsRoom/cancel`, reason, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          id: id
        }
      })

      if (response.status === 200) return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  }
}
