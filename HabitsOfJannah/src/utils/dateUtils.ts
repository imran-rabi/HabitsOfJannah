export const formatDate = (date: string | Date): string => {
  const d = typeof date === 'string' ? new Date(date) : date

  return d.toLocaleDateString('en-US', {
    month: '2-digit',
    day: '2-digit',
    year: 'numeric'
  })
}

export const formatTime = (time: string | Date): string => {
  const d = new Date(time)
  return d.toLocaleTimeString(undefined, {
    hour: '2-digit',
    minute: '2-digit'
  })
}

export const getCurrentDateTime = (): string => {
  return new Date().toISOString()
}

export const toLocalISOString = (date: Date): string => {
  const tzOffset = date.getTimezoneOffset() * 60000 // offset in milliseconds
  const localISOTime = (new Date(date.getTime() - tzOffset)).toISOString()
  return localISOTime
}

export const fromUTCToLocal = (utcDate: string): Date => {
  return new Date(utcDate)
} 