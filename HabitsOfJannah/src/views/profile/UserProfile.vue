<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-3xl mx-auto">
      <!-- Profile Header -->
      <div class="bg-white rounded-lg shadow-md p-6 mb-8">
        <div class="flex items-center space-x-4">
          <div class="bg-primary text-white text-2xl font-bold rounded-full w-16 h-16 flex items-center justify-center">
            {{ userInitials }}
          </div>
          <div>
            <h2 class="text-2xl font-bold">{{ profile.name }}</h2>
            <p class="text-gray-500">{{ profile.email }}</p>
          </div>
        </div>
      </div>

      <!-- Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-sm text-gray-500">Total Habits</div>
          <div class="text-2xl font-bold">{{ profile.totalHabits }}</div>
        </div>
        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-sm text-gray-500">Active Habits</div>
          <div class="text-2xl font-bold">{{ profile.activeHabits }}</div>
        </div>
        <div class="bg-white p-6 rounded-lg shadow-md">
          <div class="text-sm text-gray-500">Member Since</div>
          <div class="text-2xl font-bold">{{ formatDate(profile.createdAt) }}</div>
        </div>
      </div>

      <!-- Settings -->
      <div class="bg-white rounded-lg shadow-md p-6">
        <h3 class="text-xl font-semibold mb-6">Profile Settings</h3>
        <form @submit.prevent="updateProfile">
          <div class="mb-4">
            <label class="block text-gray-700 mb-2">Name</label>
            <input 
              v-model="form.name"
              type="text"
              class="w-full px-3 py-2 border rounded-lg"
            >
          </div>

          <div class="mb-6">
            <label class="block text-gray-700 mb-2">Email</label>
            <input 
              v-model="form.email"
              type="email"
              class="w-full px-3 py-2 border rounded-lg"
            >
          </div>

          <button 
            type="submit"
            class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-opacity-90"
          >
            Save Changes
          </button>
        </form>

        <div class="mt-8 pt-8 border-t">
          <h4 class="text-lg font-semibold mb-4">Change Password</h4>
          <form @submit.prevent="changePassword">
            <div class="mb-4">
              <label class="block text-gray-700 mb-2">Current Password</label>
              <input 
                v-model="passwordForm.currentPassword"
                type="password"
                class="w-full px-3 py-2 border rounded-lg"
              >
            </div>

            <div class="mb-4">
              <label class="block text-gray-700 mb-2">New Password</label>
              <input 
                v-model="passwordForm.newPassword"
                type="password"
                class="w-full px-3 py-2 border rounded-lg"
              >
            </div>

            <div class="mb-6">
              <label class="block text-gray-700 mb-2">Confirm New Password</label>
              <input 
                v-model="passwordForm.confirmPassword"
                type="password"
                class="w-full px-3 py-2 border rounded-lg"
              >
            </div>

            <button 
              type="submit"
              class="bg-secondary text-white px-4 py-2 rounded-lg hover:bg-opacity-90"
            >
              Change Password
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import api from '@/utils/api'

const profile = ref({
  name: '',
  email: '',
  totalHabits: 0,
  activeHabits: 0,
  createdAt: '',
})

const form = ref({
  name: '',
  email: '',
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: '',
})

const userInitials = computed(() => {
  return profile.value.name
    .split(' ')
    .map(n => n[0])
    .join('')
    .toUpperCase()
})

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const fetchProfile = async () => {
  try {
    const response = await api.get('/users/profile')
    profile.value = response.data
    form.value.name = response.data.name
    form.value.email = response.data.email
  } catch (error) {
    console.error('Failed to fetch profile:', error)
  }
}

const updateProfile = async () => {
  try {
    await api.put('/users/profile', form.value)
    await fetchProfile()
  } catch (error) {
    console.error('Failed to update profile:', error)
  }
}

const changePassword = async () => {
  try {
    await api.put('/users/password', passwordForm.value)
    passwordForm.value = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    }
  } catch (error) {
    console.error('Failed to change password:', error)
  }
}

onMounted(fetchProfile)
</script> 