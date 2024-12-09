<template>
  <div class="container mx-auto px-4 py-8">
    <div class="bg-white rounded-lg shadow-md p-6">
      <div class="flex justify-between items-center mb-6">
        <div>
          <h2 class="text-2xl font-bold">{{ habit?.name }}</h2>
          <p class="text-gray-500">{{ habit?.description }}</p>
        </div>
        <div class="text-sm text-gray-500">
          Started: {{ formatDate(habit?.startDate) }}
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
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
        <div class="bg-gray-50 p-4 rounded-lg">
          <div class="text-sm text-gray-500">Total Check-ins</div>
          <div class="text-2xl font-bold">{{ stats?.totalCheckins }}</div>
        </div>
      </div>

      <div class="mb-8">
        <h3 class="text-lg font-semibold mb-4">Progress Calendar</h3>
        <div class="grid grid-cols-7 gap-2">
          <div 
            v-for="(day, index) in calendarDays" 
            :key="index"
            class="aspect-square"
          >
            <div 
              :class="[
                'w-full h-full rounded-lg flex items-center justify-center text-sm',
                getProgressClass(day)
              ]"
              :title="getTooltip(day)"
            >
              {{ new Date(day.date).getDate() }}
            </div>
          </div>
        </div>
      </div>

      <div>
        <h3 class="text-lg font-semibold mb-4">Recent Activity</h3>
        <div class="space-y-4">
          <div 
            v-for="progress in recentProgress" 
            :key="progress.date"
            class="bg-gray-50 p-4 rounded-lg"
          >
            <div class="flex justify-between items-start mb-2">
              <div>
                <div class="font-medium">{{ formatDate(progress.date) }}</div>
                <div class="text-sm text-gray-500">{{ progress.notes }}</div>
              </div>
              <div class="flex items-center gap-4">
                <span class="text-2xl" :title="progress.mood">
                  {{ getMoodEmoji(progress.mood) }}
                </span>
                <div 
                  :class="[
                    'px-3 py-1 rounded-full text-sm',
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
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import api from '@/utils/api'
import { formatDate } from '@/utils/dateUtils'

const route = useRoute()
const habit = ref<any>(null)
const stats = ref<any>(null)
const recentProgress = ref<any[]>([])
const calendarDays = ref<any[]>([])

const moodEmojis: Record<string, string> = {
  great: '😄',
  good: '🙂',
  okay: '😐',
  difficult: '😕',
  tough: '😫'
}

const getMoodEmoji = (mood: string) => moodEmojis[mood] || '😐'

const getProgressClass = (day: any) => {
  if (!day.hasProgress) return 'bg-gray-100'
  
  return day.value >= 100 
    ? 'bg-success text-white' 
    : day.value > 0 
      ? 'bg-success bg-opacity-25' 
      : 'bg-error bg-opacity-25'
}

const getTooltip = (day: any) => {
  if (!day.hasProgress) return 'No progress recorded'
  return `${formatDate(day.date)}\nProgress: ${day.value}%\n${day.notes || ''}`
}

const fetchHabitData = async () => {
  const habitId = route.params.id
  try {
    const [habitResponse, statsResponse, progressResponse, calendarResponse] = await Promise.all([
      api.get(`/habits/${habitId}`),
      api.get(`/habits/${habitId}/statistics`),
      api.get(`/habits/${habitId}/progress`),
      api.get(`/habits/${habitId}/calendar`)
    ])
    
    habit.value = habitResponse.data
    stats.value = statsResponse.data
    recentProgress.value = progressResponse.data
    calendarDays.value = calendarResponse.data
  } catch (error) {
    console.error('Failed to fetch habit data:', error)
  }
}

onMounted(fetchHabitData)
</script>

<style scoped>
.aspect-square {
  aspect-ratio: 1;
}
</style> 