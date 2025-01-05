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

  async changePassword(credentials) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Users/change-password`, credentials)

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
  },

  async fetchUsers(keyword, pageSize, pageNumber, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Users`, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        params: {
          keyword: keyword,
          pageSize: pageSize,
          pageNumber: pageNumber
        }
      })

      const data = response.data
      this.users = data.Items
      this.totalItems = data.TotalItems
      this.totalPages = data.TotalPages
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async lockUsers(id, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/Users/lock`, id, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 200) return { success: true }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async unlockUsers(id, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/Users/unlock`, id, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 200) return { success: true }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async createAdmin(user, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Users/create-admin`, user, {
        headers: {
          Authorization: `Bearer ${token}`
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
  async createHotelPartner(user, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Users/create-hotel-account`, user, {
        headers: {
          Authorization: `Bearer ${token}`
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
  async createTourPartner(user, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Users/create-tour-account`, user, {
        headers: {
          Authorization: `Bearer ${token}`
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

  async fetchWallet(userId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Wallets/${userId}`)

      if (response.status === 200) {
        this.wallet = response.data
        return { success: true }
      }
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async fetchWalletPositive(token, pageNumber) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.get(`${apiServer}/Wallets`, {
        headers: {
          Authorization: `Bearer ${token}`
        },
        params: {
          pageNumber: pageNumber
        }
      })

      const data = response.data
      this.wallets = data.Items
      this.totalItems = data.TotalItems
      this.totalPages = data.TotalPages
    } catch (error) {
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  },
  async createWallet(wallet) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.post(`${apiServer}/Wallets`, wallet)

      if (response.status === 201) {
        return { success: true }
      }
    } catch (error) {
      console.error(error)
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: error.message || 'Lỗi kết nối đến server' }
    }
  },
  async updateWallet(wallet, id) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.put(`${apiServer}/Wallets/${id}`, wallet)

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

  async paymentForWallet(id, token) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST
      const response = await axios.patch(`${apiServer}/Wallets`, id, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      })

      if (response.status === 200) {
        return { success: true }
      }
    } catch (error) {
      console.error(error)
      if (error.response) {
        return { success: false, message: error.response.data }
      }
      return { success: false, message: 'Lỗi kết nối đến server' }
    }
  }
}
