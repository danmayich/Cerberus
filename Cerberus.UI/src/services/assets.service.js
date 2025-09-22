import { apiService } from './api.service';

// Create a service for a specific endpoint
const assetsService = {
  async getAll(params) {
    console.log('AssetsService: Calling getAll');
    try {
      const response = await apiService.get('assets', params);
      console.log('AssetsService: getAll response:', response);
      return response;
    } catch (error) {
      console.error('AssetsService: getAll error:', error);
      throw error;
    }
  },
  async getById(id) {
    console.log('AssetsService: Getting asset by id:', id);
    return apiService.get(`assets/${id}`);
  },
  async create(data) {
    console.log('AssetsService: Creating new asset:', data);
    return apiService.post('assets', data);
  },
  async update(id, data) {
    console.log('AssetsService: Updating asset:', id, data);
    return apiService.put(`assets/${id}`, data);
  },
  async delete(id) {
    console.log('AssetsService: Deleting asset:', id);
    return apiService.delete(`assets/${id}`);
  },
  // Custom method
  async getAvailableAssets() {
    console.log('AssetsService: Getting available assets');
    return apiService.get('assets/available');
  }
};

export default assetsService;