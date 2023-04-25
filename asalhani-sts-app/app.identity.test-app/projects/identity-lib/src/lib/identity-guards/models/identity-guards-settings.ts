import {UserManagerSettings} from "oidc-client";

export class IdentityGuardsSettings {
  enableConsoleLogging: boolean;
  postLoginRedirectUrl: string;
  oidcSettings: UserManagerSettings
}
