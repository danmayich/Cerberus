import { apiService } from './api.service';

const authService = {
    login(returnUrl = window.location.href) {
        const baseUrl = process.env.VUE_APP_API_URL || 'http://localhost:5000/api';
        window.location.href = `${baseUrl}/Authentication/login?returnUrl=${encodeURIComponent(returnUrl)}`;
    },

    async isAuthenticated() {
        try {
            // Make a call to check authentication status
            await apiService.get('Authentication/status');
            return true;
        } catch (error) {
            if (error.response && error.response.status === 401) {
                return false;
            }
            // If there's a different error, assume not authenticated
            console.error('Auth check error:', error);
            return false;
        }
    }
};

export default authService;
