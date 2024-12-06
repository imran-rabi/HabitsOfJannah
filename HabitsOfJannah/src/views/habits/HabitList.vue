<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">My Habits</h2>
      <button 
        @click="showAddHabit = true"
        class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-opacity-90"
      >
        Add Habit
      </button>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="habit in habits" 
        :key="habit.id"
        class="bg-white rounded-lg shadow-md p-6"
      >
        <h3 class="text-xl font-semibold mb-2">{{ habit.name }}</h3>
        <p class="text-gray-600 mb-4">{{ habit.description }}</p>
        <div class="flex justify-between items-center">
          <span class="text-sm text-gray-500">
            Streak: {{ habit.currentStreak }} days
          </span>
          <button 
            @click="markComplete(habit.id)"
            class="bg-success text-white px-3 py-1 rounded hover:bg-opacity-90"
          >
            Complete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '@/utils/api'

interface Habit {
  id: number
  name: string
  description: string
  currentStreak: number
}

const habits = ref<Habit[]>([])
const showAddHabit = ref(false)

const fetchHabits = async () => {
  try {
    const response = await api.get('/habits')
    habits.value = response.data
  } catch (error) {
    console.error('Failed to fetch habits:', error)
  }
}

const markComplete = async (habitId: number) => {
  try {
    await api.post(`/habits/${habitId}/progress`, {
      date: new Date().toISOString(),
      value: 100
    })
    fetchHabits()
  } catch (error) {
    console.error('Failed to mark habit complete:', error)
  }
}

onMounted(fetchHabits)
</script> 