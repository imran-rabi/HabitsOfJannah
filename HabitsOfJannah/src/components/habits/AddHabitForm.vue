<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
    <div class="bg-white p-8 rounded-lg w-full max-w-md">
      <div class="flex justify-between items-center mb-6">
        <h3 class="text-xl font-bold">Add New Habit</h3>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">
          <span class="text-2xl">&times;</span>
        </button>
      </div>

      <form @submit.prevent="handleSubmit">
        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Name</label>
          <input 
            v-model="form.name"
            type="text"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Description</label>
          <textarea 
            v-model="form.description"
            class="w-full px-3 py-2 border rounded-lg"
            rows="3"
          ></textarea>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Frequency</label>
          <select 
            v-model="form.frequency"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
            <option value="Daily">Daily</option>
            <option value="Weekly">Weekly</option>
            <option value="Monthly">Monthly</option>
          </select>
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Start Date</label>
          <input 
            v-model="form.startDate"
            type="date"
            class="w-full px-3 py-2 border rounded-lg"
            required
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">End Date (Optional)</label>
          <input 
            v-model="form.endDate"
            type="date"
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-2">Reminder Time (Optional)</label>
          <input 
            v-model="form.reminderTime"
            type="time"
            class="w-full px-3 py-2 border rounded-lg"
          >
        </div>

        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Notes (Optional)</label>
          <textarea 
            v-model="form.notes"
            class="w-full px-3 py-2 border rounded-lg"
            rows="2"
          ></textarea>
        </div>

        <div class="flex justify-end gap-4">
          <button 
            type="button"
            @click="$emit('close')"
            class="px-4 py-2 text-gray-600 hover:text-gray-800"
          >
            Cancel
          </button>
          <button 
            type="submit"
            class="px-4 py-2 bg-primary text-white rounded-lg hover:bg-opacity-90"
          >
            Create Habit
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '@/utils/api'

const emit = defineEmits(['close', 'habit-added'])

const form = ref({
  name: '',
  description: '',
  frequency: 'Daily',
  startDate: new Date().toISOString().split('T')[0],
  endDate: '',
  reminderTime: '',
  notes: ''
})

const handleSubmit = async () => {
  try {
    const response = await api.post('/habits', form.value)
    emit('habit-added', response.data)
    emit('close')
  } catch (error: any) {
    alert(error.response?.data?.message || 'Failed to create habit')
  }
}
</script>
