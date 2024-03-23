import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./test-setup.js'],
    reporters: ['json'],
    outputFile: './test-output.json'
  }
})
