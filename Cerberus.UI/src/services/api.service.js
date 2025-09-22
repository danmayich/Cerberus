import axios from 'axios';

class ApiService {
  constructor() {
    this.axios = axios.create({
      baseURL: process.env.VUE_APP_API_URL || 'http://localhost:5000/api',
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      }
    });

    // Add request interceptor
    this.axios.interceptors.request.use(
      (config) => {
        // You can add auth tokens here if needed
        // const token = getToken();
        // if (token) {
        //   config.headers.Authorization = `Bearer ${token}`;
        // }
        console.log('API Request:', {
          method: config.method,
          url: config.url,
          baseURL: config.baseURL,
          data: config.data,
          params: config.params
        });
        return config;
      },
      (error) => {
        console.error('API Request Error:', error);
        return Promise.reject(error);
      }
    );

    // Add response interceptor
    this.axios.interceptors.response.use(
      (response) => response.data,
      (error) => {
        // Handle specific error cases here
        if (error.response) {
          // Server responded with error
          console.error('API Error:', error.response.data);
        } else if (error.request) {
          // Request made but no response
          console.error('Network Error:', error.request);
        } else {
          // Other errors
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