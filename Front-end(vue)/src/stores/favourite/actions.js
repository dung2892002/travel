import axios from 'axios'

export default {
  async fetchHotelFavourites(hotelId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Favourites/hotel`, {
        params: {
          id: hotelId
        }
      })
      const favourites = response.data
      this.hotelFavourites = favourites
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchUserFavouriteHotel(userId, token, hotelId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Favourites/hotel/check`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          userId: userId,
          hotelId: hotelId
        }
      })
      const favourite = response.data
      this.userFavouriteHotel = favourite
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createFavourite(favourite, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Favourites`, favourite, {
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

  async deleteFavourite(id, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.delete(`${apiServer}/Favourites/delete`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: id
        }
      })

      if (response.status === 200) {
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
