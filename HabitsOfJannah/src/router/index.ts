import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/auth/login',
    name: 'Login',
    component: () => import('../views/auth/Login.vue')
  },
  {
    path: '/auth/register',
    name: 'Register',
    component: () => import('../views/auth/Register.vue')
  },
  {
    path: '/habits',
    name: 'Habits',
    component: () => import('../views/habits/HabitList.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/achievements',
    name: 'Achievements',
    component: () => import('../views/achievements/AchievementList.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('../views/profile/UserProfile.vue'),
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  if (to.meta.requiresAuth && !token) {
    next('/auth/login')
  } else {
    next()
  }
})

export default router