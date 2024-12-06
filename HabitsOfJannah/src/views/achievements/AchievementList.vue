<template>
  <div class="container mx-auto px-4 py-8">
    <h2 class="text-2xl font-bold mb-6">Achievements</h2>
    
    <!-- Achievement Stats -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <div class="bg-white p-6 rounded-lg shadow-md">
        <div class="text-sm text-gray-500">Total Achievements</div>
        <div class="text-2xl font-bold">{{ stats.totalAchievements }}</div>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md">
        <div class="text-sm text-gray-500">Completed</div>
        <div class="text-2xl font-bold">{{ stats.completedAchievements }}</div>
      </div>
      <div class="bg-white p-6 rounded-lg shadow-md">
        <div class="text-sm text-gray-500">Completion Rate</div>
        <div class="text-2xl font-bold">{{ stats.overallCompletion }}%</div>
      </div>
    </div>

    <!-- Recent Achievements -->
    <div class="bg-white rounded-lg shadow-md p-6 mb-8">
      <h3 class="text-xl font-semibold mb-4">Recent Achievements</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <div 
          v-for="achievement in recentAchievements" 
          :key="achievement.id"
          class="bg-gray-50 p-4 rounded-lg flex items-center space-x-4"
        >
          <div class="bg-accent p-3 rounded-full">
            🏆
          </div>
          <div>
            <div class="font-medium">{{ achievement.name }}</div>
            <div class="text-sm text-gray-500">{{ formatDate(achievement.awardedAt) }}</div>
          </div>
        </div>
      </div>
    </div>

    <!-- Achievement Categories -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <div 
        v-for="(achievements, type) in achievementsByType" 
        :key="type"
        class="bg-white rounded-lg shadow-md p-6"
      >
        <h3 class="text-xl font-semibold mb-4">{{ type }}</h3>
        <div class="space-y-4">
          <div 
            v-for="achievement in achievements" 
            :key="achievement.id"
            class="flex items-center justify-between p-4 bg-gray-50 rounded-lg"
          >
            <div>
              <div class="font-medium">{{ achievement.name }}</div>
              <div class="text-sm text-gray-500">{{ achievement.description }}</div>
            </div>
            <div 
              :class="[
                'px-3 py-1 rounded-full text-sm',
                achievement.isCompleted ? 'bg-success text-white' : 'bg-gray-200'
              ]"
            >
              {{ achievement.isCompleted ? 'Completed' : 'In Progress' }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '@/utils/api'

const stats = ref({
  totalAchievements: 0,
  completedAchievements: 0,
  overallCompletion: 0
})

const recentAchievements = ref([])
const achievementsByType = ref({})

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const fetchAchievements = async () => {
  try {
    const response = await api.get('/achievements/progress')
    stats.value = {
      totalAchievements: response.data.totalAchievements,
      completedAchievements: response.data.completedAchievements,
      overallCompletion: response.data.overallCompletion
    }
    recentAchievements.value = response.data.recentAchievements
    achievementsByType.value = response.data.achievementsByType
  } catch (error) {
    console.error('Failed to fetch achievements:', error)
  }
}

onMounted(fetchAchievements)
</script> 