import { apiService } from './api.service';

const authService = {
    async login() {
        try {
            console.log('AuthService: Initiating login flow');
            const response = await apiService.get('Authentication/login');
            console.log('AuthService: Login response received');
            
            // Since this is an OIDC flow, it will handle redirects automatically
            // If we get a response, it might contain a redirect URL
            if (response?.url) {
                window.location.href = response.url;
            }
            
            return response;
        } catch (error) {
            console.error('AuthService: Login error:', error);
            throw error;
        }
    }
};

export default authService;