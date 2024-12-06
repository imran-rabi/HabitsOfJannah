<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white p-8 rounded-lg shadow-md w-96">
      <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Email</label>
          <input 
            type="email" 
            v-model="email"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Password</label>
          <input 
            type="password" 
            v-model="password"
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
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/utils/api'

const router = useRouter()
const email = ref('')
const password = ref('')

const handleLogin = async () => {
  try {
    const response = await api.post('/auth/login', {
      email: email.value,
      password: password.value
    })
    localStorage.setItem('token', response.data.token)
    router.push('/habits')
  } catch (error) {
    console.error('Login failed:', error)
  }
}
</script> 