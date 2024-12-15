import axios from 'axios'

export default {
  async fetchMyBookingRoom(userId, status, pageNumber, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/BookingsRoom/user`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: userId,
          status: status,
          pageNumber: pageNumber
        }
      })
      const data = response.data

      this.myBookingsRoom = data.Items
      this.totalPages = data.TotalPages
      this.totalItems = data.TotalItems
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
  },

  resetBookingRooms() {
    this.myBookingsRoom = null
  },

  resetBookingTours() {
    this.myBookingsTour = null
  },

  async fetchMyBookingTour(userId, status, pageNumber, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/BookingsTour/user`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: userId,
          status: status,
          pageNumber: pageNumber
        }
      })
      const data = response.data

      this.myBookingsTour = data.Items
      this.totalPages = data.TotalPages
      this.totalItems = data.TotalItems
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createBookingTour(booking, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/BookingsTour`, booking, {
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

  async cancelBookingTour(id, reason, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/BookingsTour/cancel`, reason, {
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
  },

  async paymentForBookingRoom(request, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const returnUrl = import.meta.env.VITE_RETURN_PAYMENT_BOOKINGROOM_URL
      request.ReturnUrl = returnUrl
      const response = await axios.post(`${apiServer}/Payments/pay-room`, request, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 200) return { success: true, url: response.data.Url }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async paymentForBookingTour(request, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const returnUrl = import.meta.env.VITE_RETURN_PAYMENT_BOOKINGTOUR_URL
      request.ReturnUrl = returnUrl
      const response = await axios.post(`${apiServer}/Payments/pay-tour`, request, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 200) return { success: true, url: response.data.Url }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  }
}
