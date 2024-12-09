<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-2xl font-bold mb-6">Achievements</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="achievement in achievements" :key="achievement.id" 
           class="bg-white rounded-lg shadow-md p-6">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-semibold">{{ achievement.name }}</h3>
          <span class="text-2xl">🏆</span>
        </div>
        <p class="text-gray-600 mb-4">{{ achievement.description }}</p>
        <div class="text-sm text-gray-500">
          Awarded: {{ formatDate(achievement.awardedAt) }}
        </div>
      </div>
    </div>

    <!-- Show when no achievements -->
    <div v-if="achievements.length === 0" 
         class="text-center text-gray-500 py-8">
      No achievements yet. Keep working on your habits!
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '@/utils/api'
import { formatDate } from '@/utils/dateUtils'

const achievements = ref([])

const fetchAchievements = async () => {
  try {
    const response = await api.get('/achievements')
    achievements.value = response.data
  } catch (error) {
    console.error('Failed to fetch achievements:', error)
  }
}

onMounted(fetchAchievements)
</script> 