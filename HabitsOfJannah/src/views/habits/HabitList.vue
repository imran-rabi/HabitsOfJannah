<template>
  <div class="container mx-auto px-4 py-8">
    <!-- Header with Add Habit Button -->
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-2xl font-bold">My Habits</h1>
      <button 
        @click="showAddHabit = true"
        class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-opacity-90"
      >
        Add New Habit
      </button>
    </div>

    <!-- Habits Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="habit in habits" 
        :key="habit.id"
        class="bg-white rounded-lg shadow-md overflow-hidden"
      >
        <!-- Habit Card Header -->
        <div class="p-4 border-b">
          <div class="flex justify-between items-start">
            <div>
              <h3 class="text-lg font-semibold">{{ habit.name }}</h3>
              <p class="text-sm text-gray-500">{{ habit.description }}</p>
            </div>
            <div class="flex items-center space-x-2">
              <button 
                @click="editHabit(habit)"
                class="text-gray-500 hover:text-primary"
              >
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                </svg>
              </button>
              <button 
                @click="deleteHabit(habit.id)"
                class="text-gray-500 hover:text-error"
              >
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Progress Section -->
        <div class="p-4">
          <div class="flex justify-between items-center mb-2">
            <span class="text-sm font-medium">Today's Progress</span>
            <span class="text-sm text-gray-500">
              {{ habit.todayProgress?.value || 0 }}%
            </span>
          </div>
          <div class="w-full bg-gray-200 rounded-full h-2.5 mb-4">
            <div 
              class="bg-primary h-2.5 rounded-full" 
              :style="{ width: `${habit.todayProgress?.value || 0}%` }"
            ></div>
          </div>

          <!-- Stats -->
          <div class="grid grid-cols-3 gap-4 mb-4">
            <div class="text-center">
              <div class="text-sm text-gray-500">Current Streak</div>
              <div class="font-bold">{{ habit.currentStreak }} days</div>
            </div>
            <div class="text-center">
              <div class="text-sm text-gray-500">Best Streak</div>
              <div class="font-bold">{{ habit.bestStreak }} days</div>
            </div>
            <div class="text-center">
              <div class="text-sm text-gray-500">Completion</div>
              <div class="font-bold">{{ habit.completionRate }}%</div>
            </div>
          </div>

          <!-- Action Buttons -->
          <div class="flex justify-between">
            <button 
              @click="showProgress(habit.id)"
              class="text-primary text-sm hover:underline"
            >
              View Progress
            </button>
            <button 
              @click="checkIn(habit.id)"
              class="bg-primary text-white px-3 py-1 rounded hover:bg-opacity-90 text-sm"
            >
              Check In
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Habit Modal -->
    <AddHabitModal 
      v-if="showAddHabit"
      :habit="editingHabit"
      @close="closeAddHabit"
      @save="saveHabit"
    />

    <!-- Check-in Modal -->
    <CheckInModal
      v-if="showCheckIn"
      :habit-id="selectedHabitId"
      @close="closeCheckIn"
      @save="saveCheckIn"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/utils/api'
import AddHabitModal from '@/components/habits/AddHabitModal.vue'
import CheckInModal from '@/components/habits/CheckInModal.vue'

const router = useRouter()
const habits = ref([])
const showAddHabit = ref(false)
const showCheckIn = ref(false)
const editingHabit = ref(null)
const selectedHabitId = ref(null)

const fetchHabits = async () => {
  try {
    const response = await api.get('/habits')
    habits.value = response.data
  } catch (error) {
    console.error('Failed to fetch habits:', error)
  }
}

const editHabit = (habit) => {
  editingHabit.value = habit
  showAddHabit.value = true
}

const closeAddHabit = () => {
  showAddHabit.value = false
  editingHabit.value = null
}

const saveHabit = async (habitData: any) => {
  try {
    if (editingHabit.value) {
      await api.put(`/habits/${editingHabit.value.id}`, habitData)
    } else {
      await api.post('/habits', habitData)
    }
    await fetchHabits()
    closeAddHabit()
  } catch (error: any) {
    console.error('Failed to save habit:', error)
    alert(error.response?.data?.message || 'Failed to save habit')
  }
}

const deleteHabit = async (habitId) => {
  if (!confirm('Are you sure you want to delete this habit?')) return

  try {
    await api.delete(`/habits/${habitId}`)
    await fetchHabits()
  } catch (error) {
    console.error('Failed to delete habit:', error)
  }
}

const checkIn = (habitId: number) => {
  selectedHabitId.value = habitId
  showCheckIn.value = true
}

const closeCheckIn = () => {
  showCheckIn.value = false
  selectedHabitId.value = null
}

const saveCheckIn = async (checkInData: any) => {
  try {
    console.log('Sending check-in data:', checkInData)
    
    // Make sure we're sending the correct data structure
    const payload = {
      habitId: selectedHabitId.value,
      date: checkInData.date,
      type: checkInData.type,
      value: parseInt(checkInData.value.toString()),
      notes: checkInData.notes || '',
      mood: checkInData.mood || 'okay'
    }

    const response = await api.post(`/habits/${selectedHabitId.value}/progress`, payload)
    console.log('Check-in response:', response.data)
    await fetchHabits()
    closeCheckIn()
  } catch (error: any) {
    console.error('Check-in error:', error)
    console.error('Error details:', error.response?.data)
    alert(error.response?.data?.message || 'Failed to save check-in')
  }
}

const showProgress = (habitId) => {
  router.push(`/habits/${habitId}/progress`)
}

onMounted(fetchHabits)
</script> 