<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/utils/api'

const router = useRouter()
const form = ref({
  name: '',
  description: '',
  frequency: 'daily',
  startDate: new Date().toISOString().split('T')[0],
  endDate: '',
  targetValue: 1,
  reminderTime: null,
  notes: ''
})

const handleSubmit = async () => {
  try {
    const habitData = {
      name: form.value.name,
      description: form.value.description || '',
      frequency: form.value.frequency.toLowerCase(),
      startDate: new Date(form.value.startDate).toISOString(),
      endDate: form.value.endDate ? new Date(form.value.endDate).toISOString() : null,
      targetValue: parseInt(form.value.targetValue),
      reminderTime: form.value.reminderTime || null,
      notes: form.value.notes || ''
    }

    console.log('Submitting habit:', habitData)
    const response = await api.post('/habits', habitData)
    console.log('Response:', response.data)
    router.push('/habits')
  } catch (error: any) {
    console.error('Error creating habit:', error.response?.data)
    alert(error.response?.data?.message || 'Failed to create habit')
  }
}
</script>

<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-2xl mx-auto">
      <h1 class="text-2xl font-bold mb-6">Create New Habit</h1>
      
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <!-- Form fields -->
        <div>
          <label class="block text-gray-700 mb-2">Name</label>
          <input 
            v-model="form.name"
            type="text"
            required
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <div>
          <label class="block text-gray-700 mb-2">Description</label>
          <textarea 
            v-model="form.description"
            class="w-full px-3 py-2 border rounded-lg"
            rows="3"
          ></textarea>
        </div>

        <div>
          <label class="block text-gray-700 mb-2">Frequency</label>
          <select 
            v-model="form.frequency"
            class="w-full px-3 py-2 border rounded-lg"
          >
            <option value="daily">Daily</option>
            <option value="weekly">Weekly</option>
            <option value="monthly">Monthly</option>
          </select>
        </div>

        <div>
          <label class="block text-gray-700 mb-2">Start Date</label>
          <input 
            v-model="form.startDate"
            type="date"
            required
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <div>
          <label class="block text-gray-700 mb-2">End Date (Optional)</label>
          <input 
            v-model="form.endDate"
            type="date"
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <div>
          <label class="block text-gray-700 mb-2">Target Value</label>
          <input 
            v-model="form.targetValue"
            type="number"
            min="1"
            required
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <button 
          type="submit"
          class="w-full bg-primary text-white py-2 rounded-lg hover:bg-opacity-90"
        >
          Create Habit
        </button>
      </form>
    </div>
  </div>
</template> 