import { defineConfig } from 'vite';

export default defineConfig({
  optimizeDeps: {
    include: ['react', 'react-dom', 'react-router-dom', 'axios']
  }
});
