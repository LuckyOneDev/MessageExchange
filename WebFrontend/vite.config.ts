import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            '/api': {
                target: 'http://webapi:8080',
                changeOrigin: true,
                ws: true,
                rewrite: (path) => path.replace(/^\/api/, ''),
            },
        },
        watch: {
            usePolling: true,
        },
        host: true,
        port: 3000,
    }
})
