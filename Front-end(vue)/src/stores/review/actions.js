import axios from 'axios'

export default {
  async fetchHotelReviews(hotelId, pageNumber) {
    try {
      if (!pageNumber) return { success: false }
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Reviews/hotel`, {
        params: {
          id: hotelId,
          pageNumber: pageNumber
        }
      })
      const data = response.data
      this.hotelReviews = data.Items
      this.overallReview = data.OverallReview
      this.totalItems = data.TotalItems
      this.totalPages = data.TotalPages
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async fetchTourReviews(tourId, pageNumber) {
    try {
      if (!pageNumber) return { success: false }
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Reviews/tour`, {
        params: {
          id: tourId,
          pageNumber: pageNumber
        }
      })
      const data = response.data
      this.tourReviews = data.Items
      this.overallReview = data.OverallReview
      this.totalItems = data.TotalItems
      this.totalPages = data.TotalPages
      return { success: true }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  async createReview(formData, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Reviews`, formData, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      if (response.status === 201) {
        console.log('Review created successfully:', response)
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async deleteReview(id, userId, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      await axios.delete(`${apiServer}/Reviews/delete`, {
        params: {
          reviewId: id,
          userId: userId
        },
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
      return { success: true }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
