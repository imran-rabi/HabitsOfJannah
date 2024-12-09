<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-3xl mx-auto">
      <!-- Profile Header -->
      <div class="bg-white rounded-lg shadow-md p-6 mb-8">
        <div class="flex items-center space-x-4">
          <div class="relative">
            <div v-if="profile.profilePictureUrl" 
                 class="w-16 h-16 rounded-full overflow-hidden">
              <img :src="profile.profilePictureUrl" 
                   :alt="profile.name"
                   class="w-full h-full object-cover">
            </div>
            <div v-else
                 class="bg-primary text-white text-2xl font-bold rounded-full w-16 h-16 flex items-center justify-center">
              {{ userInitials }}
            </div>
            <button 
              @click="$refs.fileInput.click()"
              class="absolute bottom-0 right-0 bg-white rounded-full p-1 shadow-md hover:bg-gray-100"
            >
              <span class="sr-only">Change profile picture</span>
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path d="M12 4v16m8-8H4" stroke-width="2" stroke-linecap="round"/>
              </svg>
            </button>
            <input
              ref="fileInput"
              type="file"
              accept="image/*"
              class="hidden"
              @change="handleProfilePictureChange"
            >
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
              required
            >
          </div>

          <div class="mb-6">
            <label class="block text-gray-700 mb-2">Email</label>
            <input 
              v-model="form.email"
              type="email"
              class="w-full px-3 py-2 border rounded-lg"
              required
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
                required
              >
            </div>

            <div class="mb-4">
              <label class="block text-gray-700 mb-2">New Password</label>
              <input 
                v-model="passwordForm.newPassword"
                type="password"
                class="w-full px-3 py-2 border rounded-lg"
                required
                minlength="6"
              >
            </div>

            <div class="mb-6">
              <label class="block text-gray-700 mb-2">Confirm New Password</label>
              <input 
                v-model="passwordForm.confirmPassword"
                type="password"
                class="w-full px-3 py-2 border rounded-lg"
                required
              >
            </div>

            <button 
              type="submit"
              class="bg-secondary text-white px-4 py-2 rounded-lg hover:bg-opacity-90"
              :disabled="!isPasswordFormValid"
            >
              Change Password
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
  <ImageCropper
    v-if="showCropper"
    :image-url="selectedImageUrl"
    @close="showCropper = false"
    @crop="handleCroppedImage"
  />
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import api from '@/utils/api'
import ImageCropper from '@/components/profile/ImageCropper.vue'
import { useAuth } from '@/stores/auth'
import { formatDate } from '@/utils/dateUtils'

const profile = ref<any>({
  name: '',
  email: '',
  totalHabits: 0,
  activeHabits: 0,
  createdAt: new Date().toISOString(),
  profilePictureUrl: null
})
const auth = useAuth()

const form = ref({
  name: '',
  email: '',
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: '',
})

const fileInput = ref<HTMLInputElement | null>(null)

const userInitials = computed(() => {
  if (!profile.value.name) return ''
  
  return profile.value.name
    .split(' ')
    .map((n: string) => n[0])
    .join('')
    .toUpperCase()
})

const isPasswordFormValid = computed(() => {
  return passwordForm.value.newPassword === passwordForm.value.confirmPassword &&
         passwordForm.value.newPassword.length >= 6
})

const fetchProfile = async () => {
  try {
    console.log('Fetching profile...') // Debug log
    const response = await api.get('/users/profile')
    console.log('Profile response:', response.data) // Debug log
    
    if (response.data) {
      profile.value = response.data
      form.value.name = response.data.name
      form.value.email = response.data.email
      auth.updateUser(response.data)
    }
  } catch (error) {
    console.error('Failed to fetch profile:', error)
  }
}

const updateProfile = async () => {
  try {
    await api.put('/users/profile', form.value)
    await fetchProfile()
    alert('Profile updated successfully')
  } catch (error: any) {
    alert(error.response?.data?.message || 'Failed to update profile')
  }
}

const changePassword = async () => {
  try {
    if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
      alert('Passwords do not match')
      return
    }

    await api.post('/users/change-password', passwordForm.value)
    passwordForm.value = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    }
    alert('Password changed successfully')
  } catch (error: any) {
    alert(error.response?.data?.message || 'Failed to change password')
  }
}

const showCropper = ref(false)
const selectedImageUrl = ref('')

const handleProfilePictureChange = async (event: Event) => {
  const input = event.target as HTMLInputElement
  if (!input.files?.length) return

  const file = input.files[0]
  
  // Validate file type
  if (!['image/jpeg', 'image/png'].includes(file.type)) {
    alert('Please select a JPEG or PNG image')
    return
  }

  // Validate file size (5MB)
  if (file.size > 5 * 1024 * 1024) {
    alert('Image size should be less than 5MB')
    return
  }

  selectedImageUrl.value = URL.createObjectURL(file)
  showCropper.value = true
}

const handleCroppedImage = async (blob: Blob) => {
  try {
    const formData = new FormData()
    formData.append('file', blob, 'profile.jpg')

    const response = await api.post('/users/profile-picture', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    
    if (response.data) {
      await fetchProfile()
      auth.updateUser(response.data)
      showCropper.value = false
      alert('Profile picture updated successfully')
    }
  } catch (error: any) {
    const errorMessage = error.response?.data?.message || 'Failed to update profile picture'
    alert(errorMessage)
  }
}

onMounted(() => {
  console.log('UserProfile mounted') // Debug log
  fetchProfile()
})
</script> 