<template>
  <nav class="bg-white shadow">
    <div class="container mx-auto px-4">
      <div class="flex justify-between items-center h-16">
        <!-- Logo/Home Link -->
        <router-link to="/" class="text-xl font-bold text-primary">
          Habits of Jannah
        </router-link>

        <!-- Navigation Links -->
        <div class="flex items-center space-x-6">
          <router-link 
            to="/habits" 
            class="text-gray-700 hover:text-primary transition-colors"
          >
            My Habits
          </router-link>
          
          <router-link 
            to="/achievements" 
            class="text-gray-700 hover:text-primary transition-colors"
          >
            Achievements
          </router-link>
          
          <router-link 
            to="/profile" 
            class="text-gray-700 hover:text-primary transition-colors"
          >
            Profile
          </router-link>
          
          <button 
            @click="handleLogout" 
            class="text-gray-700 hover:text-error transition-colors"
          >
            Logout
          </button>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()

const handleLogout = () => {
  localStorage.removeItem('token')
  router.push('/auth/login')
}
</script>
