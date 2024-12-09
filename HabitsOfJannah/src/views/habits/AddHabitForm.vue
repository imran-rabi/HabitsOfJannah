import { toLocalISOString } from '@/utils/dateUtils'

const form = ref({
  // ... other fields ...
  startDate: toLocalISOString(new Date()).split('T')[0], // Just the date part
  endDate: '',
  reminderTime: ''
}) 