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
  }
}
