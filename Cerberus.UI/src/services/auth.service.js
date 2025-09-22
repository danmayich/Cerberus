const authService = {
    login() {
        const baseUrl = process.env.VUE_APP_API_URL || 'http://localhost:5000/api';
        // Use window.location.href to get the full current URL
        const currentUrl = window.location.href;
        // If we're not already on the character page, set it as the return URL
        const returnUrl = currentUrl.includes('/character') ? currentUrl : `${window.location.origin}/character`;
        window.location.href = `${baseUrl}/Authentication/login?returnUrl=${encodeURIComponent(returnUrl)}`;
    }
};

export default authService;
