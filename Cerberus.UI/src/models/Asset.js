/**
 * Asset model - represents a single asset with concrete fields.
 * Use this class when you want a predictable shape for assets and a single place
 * to add new fields or helpers later.
 */
export default class Asset {
    /**
     * @param {Object} data - asset data (normalized or raw)
     */
    constructor(data = {}) {
        // Accept either snake_case or camelCase keys to be robust
        this.itemId = data.itemId ?? data.item_id ?? data.itemID ?? null;
        this.itemName = data.itemName ?? data.item_name ?? null;
        this.typeId = data.typeId ?? data.type_id ?? data.typeID ?? null;
        this.locationId = data.locationId ?? data.location_id ?? null;
        this.locationType = data.locationType ?? data.location_type ?? null;
        this.locationFlag = data.locationFlag ?? data.location_flag ?? null;

        // Fields the view expects
        this.id = data.id ?? this.itemId;
        this.name = data.name ?? this.itemName;
        this.type = data.type ?? this.typeId;
        this.location = data.location ?? (this.locationType || this.locationId ? `${this.locationType || ''} ${this.locationId || ''}`.trim() : null);
        this.quantity = data.quantity ?? 0;

        // other flags
        this.isSingleton = data.isSingleton ?? data.is_singleton ?? false;
        this.isBlueprintCopy = data.isBlueprintCopy ?? data.is_blueprint_copy ?? null;

        // keep original data handy
        this._raw = data;
    }

    toPlain() {
        return {
            id: this.id,
            name: this.name,
            type: this.type,
            location: this.location,
            quantity: this.quantity,
            isSingleton: this.isSingleton,
            isBlueprintCopy: this.isBlueprintCopy,
            _raw: this._raw
        };
    }
}
