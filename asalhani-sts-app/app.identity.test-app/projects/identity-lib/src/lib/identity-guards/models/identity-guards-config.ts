import {IdentityGuardsSettings} from "./identity-guards-settings";

export const CONFIG_TOKEN_KEY = 'identity_guards_config';

export class IdentityGuardsConfig {
  getConfigValues: () => IdentityGuardsSettings;
}
