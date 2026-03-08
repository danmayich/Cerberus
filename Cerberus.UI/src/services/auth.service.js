const getApiBaseUrl = () => process.env.VUE_APP_API_URL || 'http://localhost:5058';

const getAuthBaseUrl = () => {
    // Auth controller route is /Authentication/* (not /api/Authentication/*).
    // If API base URL includes /api, strip it for browser redirects.
    const apiBase = getApiBaseUrl().replace(/\/+$/, '');
    return apiBase.replace(/\/api$/i, '');
};

const authService = {
    login() {
        const baseUrl = getAuthBaseUrl();
        const currentUrl = window.location.href;
        const returnUrl = currentUrl.includes('/character') ? currentUrl : `${window.location.origin}/character`;
        window.location.href = `${baseUrl}/Authentication/login?returnUrl=${encodeURIComponent(returnUrl)}`;
    },
    logout() {
        const baseUrl = getAuthBaseUrl();
        const returnUrl = window.location.origin;
        const logoutUrl = `${baseUrl}/Authentication/logout?returnUrl=${encodeURIComponent(returnUrl)}`;
        console.log('Logout URL:', logoutUrl);
        console.log('Return URL:', returnUrl);
        window.location.href = logoutUrl;
    }
};

export default authService;
