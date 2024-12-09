<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white p-8 rounded-lg shadow-md w-96">
      <h2 class="text-2xl font-bold mb-6 text-center">Register</h2>
      <form @submit.prevent="handleRegister">
        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Name</label>
          <input 
            v-model="form.name"
            type="text"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Email</label>
          <input 
            v-model="form.email"
            type="email"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Password</label>
          <input 
            v-model="form.password"
            type="password"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Confirm Password</label>
          <input 
            v-model="form.confirmPassword"
            type="password"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <button 
          type="submit"
          class="w-full bg-primary text-white py-2 rounded-lg hover:bg-opacity-90"
        >
          Register
        </button>

        <p class="mt-4 text-center text-gray-600">
          Already have an account?
          <router-link to="/auth/login" class="text-primary hover:underline">
            Login
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
  name: '',
  email: '',
  password: '',
  confirmPassword: ''
})

const handleRegister = async () => {
  try {
    if (form.value.password !== form.value.confirmPassword) {
      alert('Passwords do not match')
      return
    }

    const response = await api.post('/users/register', {
      name: form.value.name,
      email: form.value.email,
      password: form.value.password
    })

    auth.setAuth(response.data.token, response.data.user)
    router.push('/habits')
  } catch (error: any) {
    const message = error.response?.data?.message || 'Registration failed'
    alert(message)
  }
}
</script>
