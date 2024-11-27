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
  },
  async updateHotel(id, hotel, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Hotels/edit`, hotel, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          hotelId: id
        }
      })
      if (response.status === 200) {
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async addImageHotel(id, formData, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Hotels/edit/upload-image`, formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        },
        params: {
          hotelId: id
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
  },

  async fetchRoomByHotel(hotelId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Rooms`, {
        params: {
          hotelId: hotelId
        }
      })

      const rooms = response.data
      this.rooms = rooms
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async fetchRoom(id) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Rooms/${id}`)

      const room = response.data
      this.room = room
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createRoom(room, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Rooms`, room, {
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
  },
  async updateRoom(id, room, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Rooms/edit`, room, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          roomId: id
        }
      })
      if (response.status === 200) {
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async addImageRoom(id, formData, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Rooms/edit/upload-image`, formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        },
        params: {
          roomId: id
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
  },

  async fetchSearchHotels(query) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Hotels/search`, query)
      const data = response.data
      this.searchHotels = data.Items
      this.totalPages = data.TotalPages
      this.totalItems = data.TotalItems
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  clearSearchHotels() {
    this.searchHotels = null
    this.totalPages = 1
    this.totalItems = 1
  },

  async fetchSearchRooms(query) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Rooms/search`, query)
      const data = response.data
      this.searchRooms = data
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
