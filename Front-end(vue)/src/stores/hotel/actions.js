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
      console.log(hotels)
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  }
}
