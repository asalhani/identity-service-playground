import { IdentityGuardsSettings } from "./identity-guards-settings";

export interface IdentityGuardsConfig {
    getConfigValues?: () => IdentityGuardsSettings;
}