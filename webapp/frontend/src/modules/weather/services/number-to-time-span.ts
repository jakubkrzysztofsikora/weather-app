export const numberToTimeSpan = (number: number): string => {
  const hours = Math.floor(number)
  const minutes = Math.round((number - hours) * 60)

  const hourDescription = hours > 0 ? `hour${hours > 1 ? 's' : ''}` : 'hours'
  const minuteDescription = minutes > 0 ? `minute${minutes > 1 ? 's' : ''}` : ''
  return minutes > 0
    ? `${hours} ${hourDescription} and ${String(minutes)} ${minuteDescription}`
    : `${hours} ${hourDescription}`
}
