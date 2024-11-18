import axios from 'axios'

export default {
  async fetchHotelByPartner(partnerId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Hotels/partner/${partnerId}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })

      const hotels = response.data

      this.hotels = hotels
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchHotel(id) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Hotels/${id}`)

      const hotel = response.data
      this.hotel = hotel
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchFacilities(type) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Facilities`, {
        params: {
          type: type
        }
      })

      const facilities = response.data

      if (type === 1) this.popularFacilities = facilities
      if (type === 2) this.uniqueFacilities = facilities
      if (type === 3) this.roomFacilities = facilities
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createHotel(hotel, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Hotels`, hotel, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })
      if (response.status === 201) {
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
