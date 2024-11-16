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
  }
}
