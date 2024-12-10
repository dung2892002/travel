import axios from 'axios'

export default {
  async login(credentials) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Users/login`, credentials)

      const { User, Token } = response.data

      if (User && Token) {
        this.user = User
        this.token = Token

        localStorage.setItem('user', JSON.stringify(User))
        localStorage.setItem('jwt', Token)

        return { success: true }
      } else {
        return { success: false, message: response.error }
      }
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },

  logout() {
    this.user = null
    this.token = null

    localStorage.removeItem('user')
    localStorage.removeItem('jwt')
  },

  async register(credentials) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Users/register`, credentials)

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

  async updateUser(id, user, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Users/update-info`, user, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          id: id
        }
      })

      if (response.status === 200) {
        this.user = response.data
        localStorage.setItem('user', JSON.stringify(response.data))
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },

  async changeImage(id, formData, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Users/change-avatar`, formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        },
        params: {
          id: id
        }
      })

      if (response.status === 200) {
        this.user = response.data
        localStorage.setItem('user', JSON.stringify(response.data))
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
