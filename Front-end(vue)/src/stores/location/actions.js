import axios from 'axios'

export default {
  async fetchProvinces() {
    try {
      const apiServer = import.meta.env.VITE_API_HOST

      const response = await axios.get(`${apiServer}/Provinces`)
      const provinces = response.data

      this.provinces = provinces
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async fetchCities() {
    try {
      const apiServer = import.meta.env.VITE_API_HOST

      const citiesRes = await axios.get(`${apiServer}/Cities`)
      const cities = citiesRes.data

      this.cities = cities
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async fetchDestinations(cityId) {
    try {
      const apiServer = import.meta.env.VITE_API_HOST

      const destinationsRes = await axios.get(`${apiServer}/Destinations/${cityId}`)
      const destinations = destinationsRes.data

      this.destinations = destinations
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async fetchProvincesHotel() {
    try {
      const apiServer = import.meta.env.VITE_API_HOST

      const response = await axios.get(`${apiServer}/Provinces/hotel`)
      const provinces = response.data

      this.provincesHotel = provinces
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  },
  async fetchProvincesTour() {
    try {
      const apiServer = import.meta.env.VITE_API_HOST

      const response = await axios.get(`${apiServer}/Provinces/tour`)
      const provinces = response.data

      this.provincesTour = provinces
    } catch (error) {
      return { success: false, message: error.response.data }
    }
  }
}
