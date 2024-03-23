export const timeStringToNumber = (timeString: string): number => {
  const [time, period] = timeString.split(' ')
  const [hours, minutes] = time.split(':').map(Number)
  return period === 'AM'
    ? hours + (hours === 12 ? -12 : 0) + minutes / 60
    : hours + (hours === 12 ? 0 : 12) + minutes / 60
}
