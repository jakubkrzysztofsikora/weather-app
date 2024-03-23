import { describe, it, expect } from 'vitest'
import { numberToTimeSpan } from './number-to-time-span'

describe('number-to-time-span', () => {
  const testCases = [
    [0, '0 hours'],
    [1, '1 hour'],
    [1.5, '1 hour and 30 minutes'],
    [1.75, '1 hour and 45 minutes'],
    [2, '2 hours'],
    [2.5, '2 hours and 30 minutes'],
    [1.017, '1 hour and 1 minute']
  ]

  testCases.forEach(([number, expected]) => {
    it(`should convert ${number} to ${expected}`, () => {
      expect(numberToTimeSpan(number as number)).toBe(expected)
    })
  })
})
