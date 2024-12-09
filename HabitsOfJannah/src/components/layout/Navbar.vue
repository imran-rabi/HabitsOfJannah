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

          <!-- Profile Dropdown -->
          <div class="relative" v-click-outside="closeMenu">
            <button 
              @click="toggleMenu"
              class="flex items-center space-x-2 text-gray-700 hover:text-primary transition-colors"
            >
              <span>{{ auth.user?.name || 'Profile' }}</span>
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
              </svg>
            </button>

            <!-- Dropdown Menu -->
            <div 
              v-if="showMenu"
              class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-2"
            >
              <router-link 
                to="/profile"
                class="block px-4 py-2 text-gray-700 hover:bg-gray-100"
                @click="closeMenu"
              >
                Settings
              </router-link>
              <button 
                @click="handleLogout"
                class="block w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-100"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '@/stores/auth'

const router = useRouter()
const auth = useAuth()
const showMenu = ref(false)

const toggleMenu = () => {
  showMenu.value = !showMenu.value
}

const closeMenu = () => {
  showMenu.value = false
}

const handleLogout = () => {
  auth.clearAuth()
  router.push('/auth/login')
  closeMenu()
}

// Click outside directive
const vClickOutside = {
  mounted(el: any, binding: any) {
    el.clickOutsideEvent = (event: Event) => {
      if (!(el === event.target || el.contains(event.target))) {
        binding.value()
      }
    }
    document.addEventListener('click', el.clickOutsideEvent)
  },
  unmounted(el: any) {
    document.removeEventListener('click', el.clickOutsideEvent)
  }
}
</script>
