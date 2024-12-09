import { createRouter, createWebHistory } from 'vue-router'
import { useAuth } from '@/stores/auth'
import Home from '../views/Home.vue'
import Achievements from '@/views/achievements/Achievements.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: Home,
    meta: { requiresAuth: true }
  },
  {
    path: '/auth/login',
    name: 'login',
    component: () => import('../views/auth/Login.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/auth/register',
    name: 'register',
    component: () => import('../views/auth/Register.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/habits',
    name: 'habits',
    component: () => import('../views/habits/HabitList.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'profile',
    component: () => import('../views/profile/UserProfile.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/achievements',
    name: 'achievements',
    component: Achievements,
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Navigation guard
router.beforeEach((to, from, next) => {
  const auth = useAuth()
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const requiresGuest = to.matched.some(record => record.meta.requiresGuest)

  if (requiresAuth && !auth.isAuthenticated) {
    next('/auth/login')
  } else if (requiresGuest && auth.isAuthenticated) {
    next('/habits')
  } else {
    next()
  }
})

export default router