<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white p-6 rounded-lg w-full max-w-md">
      <div class="flex justify-between items-center mb-6">
        <h3 class="text-xl font-semibold">Check In</h3>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">
          <span class="text-2xl">&times;</span>
        </button>
      </div>

      <form @submit.prevent="handleSubmit">
        <!-- Progress Type Selection -->
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Progress Type</label>
          <div class="flex gap-4">
            <label class="flex items-center">
              <input 
                type="radio" 
                v-model="form.type"
                value="completion"
                class="mr-2"
              >
              Completion
            </label>
            <label class="flex items-center">
              <input 
                type="radio" 
                v-model="form.type"
                value="percentage"
                class="mr-2"
              >
              Percentage
            </label>
          </div>
        </div>

        <!-- Progress Value -->
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Progress</label>
          <div v-if="form.type === 'completion'" class="flex gap-4">
            <button 
              type="button"
              @click="form.value = 0"
              :class="[
                'flex-1 py-2 rounded-lg border',
                form.value === 0 
                  ? 'bg-error text-white border-error' 
                  : 'border-gray-300 hover:border-error hover:text-error'
              ]"
            >
              Not Done
            </button>
            <button 
              type="button"
              @click="form.value = 100"
              :class="[
                'flex-1 py-2 rounded-lg border',
                form.value === 100 
                  ? 'bg-success text-white border-success' 
                  : 'border-gray-300 hover:border-success hover:text-success'
              ]"
            >
              Done
            </button>
          </div>
          <div v-else class="flex items-center gap-2">
            <input 
              v-model.number="form.value"
              type="number"
              min="0"
              max="100"
              class="w-20 px-3 py-2 border rounded-lg"
              required
            >
            <span class="text-gray-500">%</span>
          </div>
        </div>

        <!-- Date Selection -->
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Date</label>
          <input 
            v-model="form.date"
            type="date"
            class="w-full px-3 py-2 border rounded-lg"
            :max="today"
            required
          >
        </div>

        <!-- Notes -->
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Notes (Optional)</label>
          <textarea 
            v-model="form.notes"
            class="w-full px-3 py-2 border rounded-lg"
            rows="3"
            placeholder="How did it go? Any challenges or victories?"
          ></textarea>
        </div>

        <!-- Mood Selection -->
        <div class="mb-6">
          <label class="block text-gray-700 mb-2">Mood</label>
          <div class="flex justify-between">
            <button 
              v-for="mood in moods" 
              :key="mood.value"
              type="button"
              @click="form.mood = mood.value"
              :class="[
                'p-2 rounded-full hover:bg-gray-100',
                form.mood === mood.value ? 'bg-gray-100' : ''
              ]"
              :title="mood.label"
            >
              {{ mood.emoji }}
            </button>
          </div>
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
            Save Progress
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { toLocalISOString } from '@/utils/dateUtils'

const props = defineProps<{
  habitId: number
}>()

const emit = defineEmits(['close', 'save'])

const today = toLocalISOString(new Date()).split('T')[0]

const moods = [
  { value: 'great', label: 'Great', emoji: '😄' },
  { value: 'good', label: 'Good', emoji: '🙂' },
  { value: 'okay', label: 'Okay', emoji: '😐' },
  { value: 'difficult', label: 'Difficult', emoji: '😕' },
  { value: 'tough', label: 'Tough', emoji: '😫' }
]

const form = ref({
  type: 'completion',
  value: 0,
  date: today,
  notes: '',
  mood: 'okay'
})

const handleSubmit = () => {
  const checkInData = {
    habitId: props.habitId,
    date: new Date(form.value.date).toISOString(),
    type: form.value.type,
    value: form.value.type === 'completion' ? (form.value.value === 100 ? 100 : 0) : form.value.value,
    notes: form.value.notes || '',
    mood: form.value.mood || 'okay'
  }

  console.log('Submitting check-in data:', checkInData)
  emit('save', checkInData)
}
</script> 