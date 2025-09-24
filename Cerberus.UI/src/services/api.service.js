import axios from 'axios';

class ApiService {
  constructor() {
    this.axios = axios.create({
      baseURL: process.env.VUE_APP_API_URL,
      timeout: 30000,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      },
      withCredentials: true,  // This enables sending cookies with requests
      maxRedirects: 0  // Prevent axios from following redirects automatically
    });

    // Add request interceptor
    this.axios.interceptors.request.use(
      (config) => {
        console.group('API Request Details');
        console.log('Full URL:', config.baseURL + '/' + config.url);
        console.log('Method:', config.method?.toUpperCase());
        console.log('Headers:', config.headers);
        if (config.data) console.log('Request Body:', config.data);
        if (config.params) console.log('Query Params:', config.params);
        console.log('Cookies Enabled:', config.withCredentials);
        console.groupEnd();
        return config;
      },
      (error) => {
        console.group('API Request Error');
        console.error('Error:', error);
        console.error('Error Message:', error.message);
        console.groupEnd();
        return Promise.reject(error);
      }
    );

    // Add response interceptor
    this.axios.interceptors.response.use(
      (response) => {
        console.group('API Response Success');
        console.log('Status:', response.status);
        console.log('Headers:', response.headers);
        console.log('Response Data:', response.data);
        console.groupEnd();
        return response.data;
      },
      (error) => {
        console.group('API Response Error');
        console.error('Error:', error);
        
        if (error.response) {
          console.log('Status:', error.response.status);
          console.log('Headers:', error.response.headers);
          console.log('Response Data:', error.response.data);
          
          if (error.response.status === 302 || error.response.status === 401) {
            const redirectUrl = error.response.headers?.location;
            console.log('Redirect URL:', redirectUrl);
            
            if (redirectUrl && redirectUrl.includes('Authentication/login')) {
              console.log('Authentication redirect detected');
              // For OIDC redirects, navigate the whole page
              if (redirectUrl.includes('/Authentication/login')) {
                window.location.href = redirectUrl;
                return new Promise(() => {}); // Prevent further error handling
              }
            }
          }
          console.error('API Error:', error.response.data);
        } else if (error.request) {
          console.error('Network Error:', error.request);
        } else {
          console.error('Error:', error.message);
        }
        return Promise.reject(error);
      }
    );
  }

  // Generic CRUD methods
  async get(endpoint, params = {}) {
    return this.axios.get(endpoint, { params });
  }

  async post(endpoint, data = {}) {
    return this.axios.post(endpoint, data);
  }

  async put(endpoint, data = {}) {
    return this.axios.put(endpoint, data);
  }

  async delete(endpoint) {
    return this.axios.delete(endpoint);
  }

  // Helper method to create endpoint-specific services
  createService(baseEndpoint) {
    return {
      getAll: (params) => this.get(`${baseEndpoint}`, params),
      getById: (id) => this.get(`${baseEndpoint}/${id}`),
      create: (data) => this.post(baseEndpoint, data),
      update: (id, data) => this.put(`${baseEndpoint}/${id}`, data),
      delete: (id) => this.delete(`${baseEndpoint}/${id}`)
    };
  }
}

// Create and export a singleton instance
export const apiService = new ApiService();

// Export the class for cases where a new instance is needed
export default ApiService;