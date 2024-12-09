<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white p-6 rounded-lg w-full max-w-md">
      <div class="flex justify-between items-center mb-6">
        <h3 class="text-xl font-semibold">
          {{ habit ? 'Edit Habit' : 'Add New Habit' }}
        </h3>
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
          <label class="block text-gray-700 mb-2">Target Goal</label>
          <div class="flex items-center gap-2">
            <input 
              v-model.number="form.targetValue"
              type="number"
              min="1"
              max="100"
              class="w-20 px-3 py-2 border rounded-lg"
              required
            >
            <span class="text-gray-500">times per {{ form.frequency.toLowerCase() }}</span>
          </div>
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
            {{ habit ? 'Save Changes' : 'Create Habit' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { toLocalISOString } from '@/utils/dateUtils'
import api from '@/utils/api'

const props = defineProps<{
  habit?: any
}>()

const emit = defineEmits(['close', 'save'])

const form = ref({
  name: '',
  description: '',
  frequency: 'Daily',
  startDate: toLocalISOString(new Date()).split('T')[0],
  endDate: '',
  targetValue: 1,
  reminderTime: '',
  notes: ''
})

const handleSubmit = async () => {
  try {
    const habitData = {
      name: form.value.name,
      description: form.value.description || '',
      frequency: form.value.frequency,
      startDate: new Date(form.value.startDate).toISOString(),
      endDate: form.value.endDate ? new Date(form.value.endDate).toISOString() : null,
      targetValue: parseInt(form.value.targetValue.toString()),
      reminderTime: form.value.reminderTime || '',
      notes: form.value.notes || ''
    }

    console.log('Submitting habit data:', JSON.stringify(habitData, null, 2))
    emit('save', habitData)
    emit('close')
  } catch (error: any) {
    console.error('Error:', error)
    alert('Failed to save habit')
  }
}

onMounted(() => {
  if (props.habit) {
    form.value = {
      name: props.habit.name,
      description: props.habit.description,
      frequency: props.habit.frequency,
      startDate: props.habit.startDate.split('T')[0],
      endDate: props.habit.endDate ? props.habit.endDate.split('T')[0] : '',
      targetValue: props.habit.targetValue,
      reminderTime: props.habit.reminderTime || '',
      notes: props.habit.notes || ''
    }
  }
})
</script> 