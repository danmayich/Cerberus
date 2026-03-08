import Asset from './Asset'

/**
 * Character model - concrete representation of the character payload.
 * Add new fields here when API/UX requires them.
 */
export default class Character {
    constructor(data = {}) {
        // Accept both camelCase and snake_case
        this.id = data.id ?? data.character_id ?? null;
        this.lastUpdated = data.lastUpdated ?? data.last_updated ?? null;

        // Character metadata from ESI
        this.characterInfo = data.characterInfo ?? data.character_info ?? null;

        // walletTransactions and transactionGroups may be objects keyed by id
        this.walletTransactions = data.walletTransactions ?? data.wallet_transactions ?? {};
        this.transactionGroups = data.transactionGroups ?? data.transaction_groups ?? {};

        // assets: convert to Asset instances
        const rawAssets = data.assets ?? [];
        this.assets = Array.isArray(rawAssets) ? rawAssets.map(a => new Asset(a)) : [];

        // keep raw payload
        this._raw = data;
    }

    // convenience to get plain JSON for views or serialization
    toPlain() {
        return {
            id: this.id,
            lastUpdated: this.lastUpdated,
            characterInfo: this.characterInfo,
            assets: this.assets.map(a => a.toPlain()),
            walletTransactions: this.walletTransactions,
            transactionGroups: this.transactionGroups,
            _raw: this._raw
        };
    }
}
