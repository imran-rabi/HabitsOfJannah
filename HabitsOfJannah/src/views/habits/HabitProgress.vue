<template>
  <div class="container mx-auto px-4 py-8">
    <div class="bg-white rounded-lg shadow-md p-6">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-bold">{{ habit?.name }}</h2>
        <div class="text-sm text-gray-500">
          Started: {{ formatDate(habit?.startDate) }}
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div class="bg-gray-50 p-4 rounded-lg">
          <div class="text-sm text-gray-500">Current Streak</div>
          <div class="text-2xl font-bold">{{ stats?.currentStreak }} days</div>
        </div>
        <div class="bg-gray-50 p-4 rounded-lg">
          <div class="text-sm text-gray-500">Best Streak</div>
          <div class="text-2xl font-bold">{{ stats?.bestStreak }} days</div>
        </div>
        <div class="bg-gray-50 p-4 rounded-lg">
          <div class="text-sm text-gray-500">Completion Rate</div>
          <div class="text-2xl font-bold">{{ stats?.completionRate }}%</div>
        </div>
      </div>

      <div class="mb-8">
        <h3 class="text-lg font-semibold mb-4">Daily Progress</h3>
        <div class="grid grid-cols-7 gap-2">
          <div 
            v-for="day in stats?.dailyProgress" 
            :key="day.date"
            :class="[
              'h-8 rounded',
              day.completed ? 'bg-success' : 'bg-gray-200'
            ]"
            :title="formatDate(day.date)"
          ></div>
        </div>
      </div>

      <div>
        <h3 class="text-lg font-semibold mb-4">Recent Activity</h3>
        <div class="space-y-4">
          <div 
            v-for="progress in recentProgress" 
            :key="progress.date"
            class="flex justify-between items-center p-4 bg-gray-50 rounded-lg"
          >
            <div>
              <div class="font-medium">{{ formatDate(progress.date) }}</div>
              <div class="text-sm text-gray-500">{{ progress.notes }}</div>
            </div>
            <div 
              :class="[
                'px-2 py-1 rounded text-sm',
                progress.value >= 100 ? 'bg-success text-white' : 'bg-gray-200'
              ]"
            >
              {{ progress.value }}%
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '@/utils/api'

const route = useRoute()
const habit = ref<any>(null)
const stats = ref<any>(null)
const recentProgress = ref<any[]>([])

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const fetchHabitData = async () => {
  const habitId = route.params.id
  try {
    const [habitResponse, statsResponse, progressResponse] = await Promise.all([
      api.get(`/habits/${habitId}`),
      api.get(`/habits/${habitId}/statistics`),
      api.get(`/habits/${habitId}/progress`)
    ])
    
    habit.value = habitResponse.data
    stats.value = statsResponse.data
    recentProgress.value = progressResponse.data
  } catch (error) {
    console.error('Failed to fetch habit data:', error)
  }
}

onMounted(fetchHabitData)
</script> 