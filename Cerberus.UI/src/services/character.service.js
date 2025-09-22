import { apiService } from './api.service';

const characterService = {
    async getCharacter() {
        try {
            console.log('CharacterService: Fetching character data');
            const response = await apiService.get('Character');
            console.log('CharacterService: Character data received:', response);
            return response;
        } catch (error) {
            console.error('CharacterService: Error fetching character:', error);
            throw error;
        }
    }
};

export default characterService;