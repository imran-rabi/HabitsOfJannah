<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-4xl font-bold text-center mb-8">Habits of Jannah</h1>
    <div class="text-center" v-if="!isAuthenticated">
      <router-link 
        to="/auth/login"
        class="bg-primary text-white px-6 py-2 rounded-lg mr-4 hover:bg-opacity-90"
      >
        Login
      </router-link>
      <router-link 
        to="/auth/register"
        class="bg-secondary text-white px-6 py-2 rounded-lg hover:bg-opacity-90"
      >
        Register
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
const isAuthenticated = computed(() => !!localStorage.getItem('token'))
</script> 