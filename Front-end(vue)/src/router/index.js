import { createRouter, createWebHistory } from 'vue-router'

import LoginPage from '@/views/user/LoginPage.vue'
import HomeView from '@/views/HomeView.vue'
import RegisterPage from '@/views/user/RegisterPage.vue'
import MyBooking from '@/views/booking/MyBooking.vue'
import UserPage from '@/views/user/UserPage.vue'
import HotelPage from '@/views/hotel/HotelPage.vue'
import TourPage from '@/views/tour/TourPage.vue'
import MyProfile from '@/views/user/MyProfile.vue'
import MyHotel from '@/views/hotel/MyHotel.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/hotel',
      name: 'hotel',
      component: HotelPage
    },
    {
      path: '/tour',
      name: 'tour',
      component: TourPage
    },
    {
      path: '/user',
      name: 'user',
      component: UserPage,
      children: [
        {
          path: '/profile',
          name: 'profile',
          component: MyProfile
        },
        {
          path: '/my-booking',
          name: 'my-booking',
          component: MyBooking
        },
        {
          path: '/my-hotel',
          name: 'my-hotel',
          component: MyHotel
        }
      ]
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterPage
    }
  ]
})

export default router