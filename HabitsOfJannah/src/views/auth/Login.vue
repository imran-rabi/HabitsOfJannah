<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white p-8 rounded-lg shadow-md w-96">
      <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Email</label>
          <input 
            v-model="form.email"
            type="email"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Password</label>
          <input 
            v-model="form.password"
            type="password"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <button 
          type="submit"
          class="w-full bg-primary text-white py-2 rounded-lg hover:bg-opacity-90"
        >
          Login
        </button>

        <p class="mt-4 text-center text-gray-600">
          Don't have an account?
          <router-link to="/auth/register" class="text-primary hover:underline">
            Register
          </router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '@/stores/auth'
import api from '@/utils/api'

const router = useRouter()
const auth = useAuth()

const form = ref({
  email: '',
  password: ''
})

const handleLogin = async () => {
  try {
    const response = await api.post('/users/login', form.value)
    console.log('Login response:', response.data)
    
    if (response.data) {
      auth.setAuth(response.data.user, response.data.token)
      console.log('Token stored:', localStorage.getItem('token'))
      router.push('/habits')
    }
  } catch (error: any) {
    console.error('Login error:', error)
    alert(error.response?.data?.message || 'Failed to login')
  }
}
</script> 