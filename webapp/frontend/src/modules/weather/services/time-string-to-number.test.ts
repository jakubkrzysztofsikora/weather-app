import { describe, it, expect } from 'vitest'
import { timeStringToNumber } from './time-string-to-number'

describe('time-string-to-number', () => {
  const testCases = [
    ['12:00 AM', 0],
    ['12:00 PM', 12],
    ['6:30 AM', 6.5],
    ['6:30 PM', 18.5]
  ]

  testCases.forEach(([timeString, expected]) => {
    it(`should convert ${timeString} to ${expected}`, () => {
      expect(timeStringToNumber(timeString as string)).toBe(expected)
    })
  })
})
