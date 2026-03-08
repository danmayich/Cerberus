import { apiService } from './api.service';
import Character from '../models/Character'

// Convert snake_case to camelCase
const toCamel = (s) => s.replace(/_([a-z])/g, (_, c) => c.toUpperCase());

function convertKeysToCamel(obj) {
    if (Array.isArray(obj)) {
        return obj.map(convertKeysToCamel);
    } else if (obj && typeof obj === 'object') {
        return Object.keys(obj).reduce((acc, key) => {
            const camelKey = toCamel(key);
            acc[camelKey] = convertKeysToCamel(obj[key]);
            return acc;
        }, {});
    }
    return obj;
}

function mapAssetForView(asset) {
    // asset fields from API: item_id, type_id, location_id, location_type, quantity, is_singleton, is_blueprint_copy, location_flag, item_name
    // view expects: id, name, type, location, quantity
    return {
        id: asset.itemId ?? asset.item_id ?? asset.id,
        name: asset.itemName ?? asset.item_name ?? null,
        type: asset.typeId ?? asset.type_id ?? asset.type,
        location: asset.locationId ?? asset.location_id ?? asset.location ?? `${asset.location_type || ''} ${asset.location_id || ''}`.trim(),
        quantity: asset.quantity ?? 0,
        // keep original data for advanced views if needed
        _raw: asset
    };
}

const characterService = {
    async getCharacter() {
        try {
            console.log('CharacterService: Fetching character data');
            const response = await apiService.get('Character');
            console.log('CharacterService: Character data received:', response);

            // Unwrap axios-like responses
            const payload = response && response.data !== undefined ? response.data : response;

            // Convert all keys to camelCase for consistency
            const normalized = convertKeysToCamel(payload);

            // Map assets to the shape the view expects (mapAssetForView kept for backwards compatibility)
            if (normalized && Array.isArray(normalized.assets)) {
                normalized.assets = normalized.assets.map(mapAssetForView);
            }

            // Return a concrete Character instance
            const characterModel = new Character(normalized);
            return characterModel;
        } catch (error) {
            console.error('CharacterService: Error fetching character:', error);
            throw error;
        }
    }
};

export default characterService;