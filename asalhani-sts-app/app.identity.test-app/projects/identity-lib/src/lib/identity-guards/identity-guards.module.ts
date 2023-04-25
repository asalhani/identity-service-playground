import {ModuleWithProviders, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { IdentityGuardsRoutingModule } from './identity-guards-routing.module';
import {IdentityGuardsConfig} from "./models/identity-guards-config";
import {IdentityUiConfig} from "../utils/identity-ui-config";
import {IdentityUiSettings} from "../models/identity-ui-settings";
import {IdentityGuardsSettings} from "./models/identity-guards-settings";
import { AuthCallbackComponent } from './views/auth-callback/auth-callback.component';
import { SilentRefreshComponent } from './views/silent-refresh/silent-refresh.component';


@NgModule({
  declarations: [
    AuthCallbackComponent,
    SilentRefreshComponent
  ],
  imports: [
    CommonModule,
    IdentityGuardsRoutingModule
  ],
  exports: [

  ]
})
export class IdentityGuardsModule {
  static forLibRoot(config: IdentityGuardsConfig = {getConfigValues: () => new IdentityGuardsSettings()}): ModuleWithProviders<IdentityGuardsModule> {
      return {
      ngModule: IdentityGuardsModule,
      providers: [
        { provide: 'identity_guards_config', useValue: config }
      ]
    };
  }
}

