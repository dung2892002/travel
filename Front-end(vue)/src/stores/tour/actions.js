import axios from 'axios'

export default {
  async fetchTourByPartner(partnerId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Tours/partner`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          id: partnerId
        }
      })

      const tours = response.data

      this.tours = tours
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchTour(id) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Tours/detail`, {
        params: {
          id: id
        }
      })
      const tour = response.data
      this.tour = tour
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createTour(tour, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Tours`, tour, {
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

  async updateTour(id, tour, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Tours/edit`, tour, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          tourId: id
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

  async addImageTour(id, formData, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Tours/edit/upload-image`, formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        },
        params: {
          tourId: id
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

  async fetchSearchTours(query) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Tours/search`, query)
      const data = response.data
      this.searchTours = data.Items
      this.totalPages = data.TotalPages
      this.totalItems = data.TotalItems
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  clearSearchTours() {
    this.searchTours = null
    this.totalPages = 1
    this.totalItems = 1
  },

  async fetchScheduleByTour(tourId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Tours/schedule/search`, {
        params: {
          tourId: tourId
        }
      })
      const data = response.data
      this.schedules = data
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async createSchedule(schedule, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Tours/schedule`, schedule, {
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

  async updatePriceSchedule(schedule, id, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/Tours/schedule`, schedule, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          id: id
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

  async fetchSearchSchedule(request) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Tours/schedule/search`, request)
      const data = response.data
      this.searchSchedules = data
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async fetchScheduleDetail(id) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Tours/schedule/detail`, {
        params: {
          id: id
        }
      })
      const data = response.data
      this.schedule = data
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
