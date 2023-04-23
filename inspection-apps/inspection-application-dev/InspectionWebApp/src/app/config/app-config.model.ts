import { IdentityGuardsSettings } from 'identity-guards';

export interface AppConfig {
  env?: string;
  ServiceURL?: string;
  EntityServiceURL?: string;
  FacilityServiceURL?: string;
  InspectionCenterServiceURL?: string;
  LookupServiceURL?: string;
  InspectionProcessServiceURL?: string;
  IdentityGuardsConfig?: IdentityGuardsSettings;
  AppTitle?: string;
  AppLogo?: string;
}
