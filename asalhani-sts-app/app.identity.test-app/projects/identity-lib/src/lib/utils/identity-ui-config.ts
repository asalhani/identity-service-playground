import {IdentityUiSettings} from "../models/identity-ui-settings";


export const CONFIG_TOKEN_KEY = 'identity_ui_config';
export class IdentityUiConfig {
  getConfigValues: () => IdentityUiSettings;

  constructor() {
  }
}
