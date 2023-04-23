import { UserManagerSettings } from "oidc-client";

export interface IdentityGuardsSettings {
    enableConsoleLogging: boolean,
    postLoginRedirectUrl: string,
    oidcSettings: UserManagerSettings
}