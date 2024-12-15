import { createRouter, createWebHistory } from 'vue-router'

import LoginPage from '@/views/user/LoginPage.vue'
import HomeView from '@/views/HomeView.vue'
import RegisterPage from '@/views/user/RegisterPage.vue'
import UserPage from '@/views/user/UserPage.vue'
import HotelPage from '@/views/hotel/HotelPage.vue'
import TourPage from '@/views/tour/TourPage.vue'
import MyProfile from '@/views/user/MyProfile.vue'
import MyHotel from '@/views/hotel/MyHotel.vue'
import HotelsRoom from '@/views/hotel/HotelsRoom.vue'
import HotelDetail from '@/views/hotel/HotelDetail.vue'
import MyDiscount from '@/views/discount/MyDiscount.vue'
import BookingRoomForm from '@/views/booking/BookingRoomForm.vue'
import MyBookingRoom from '@/views/booking/MyBookingRoom.vue'
import MyFavourite from '@/views/user/MyFavourite.vue'
import MyTour from '@/views/tour/MyTour.vue'
import TourSchedules from '@/views/tour/TourSchedules.vue'
import TourDetail from '@/views/tour/TourDetail.vue'
import BookingTourForm from '@/views/booking/BookingTourForm.vue'
import MyBookingTour from '@/views/booking/MyBookingTour.vue'
import UserList from '@/views/user/UserList.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/hotels',
      name: 'hotel',
      component: HotelPage
    },
    {
      path: '/hotel/:id',
      name: 'hotelDetail',
      component: HotelDetail
    },
    {
      path: '/create-booking-room',
      name: 'createBookingRoom',
      component: BookingRoomForm
    },
    {
      path: '/tours',
      name: 'tour',
      component: TourPage
    },
    {
      path: '/tour/:id',
      name: 'tourDetail',
      component: TourDetail
    },
    {
      path: '/create-booking-tour',
      name: 'createBookingTour',
      component: BookingTourForm
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
          path: '/my-booking-room',
          name: 'my-booking-room',
          component: MyBookingRoom
        },
        {
          path: '/my-booking-tour',
          name: 'my-booking-tour',
          component: MyBookingTour
        },
        {
          path: '/my-hotel',
          name: 'my-hotel',
          component: MyHotel
        },
        {
          path: 'rooms/hotel/:hotelId',
          name: 'RoomsList',
          component: HotelsRoom
        },
        {
          path: '/discount',
          name: 'myDiscount',
          component: MyDiscount
        },
        {
          path: '/my-favourites',
          name: 'myFavourite',
          component: MyFavourite
        },
        {
          path: '/my-tour',
          name: 'myTour',
          component: MyTour
        },
        {
          path: 'schedules/tour/:tourId',
          name: 'ScheduleList',
          component: TourSchedules
        },
        {
          path: 'users',
          name: 'userList',
          component: UserList
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
