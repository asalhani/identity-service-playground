import {ModuleWithProviders, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { IdentityGuardsRoutingModule } from './identity-guards-routing.module';
import {IdentityGuardsConfig} from "./models/identity-guards-config";
import {IdentityGuardsSettings} from "./models/identity-guards-settings";
import { AuthCallbackComponent } from './views/auth-callback/auth-callback.component';
import { SilentRefreshComponent } from './views/silent-refresh/silent-refresh.component';
import { IdentityGuardsComponent } from './identity-guards.component';


@NgModule({
  declarations: [
    AuthCallbackComponent,
    SilentRefreshComponent,
    IdentityGuardsComponent
  ],
  imports: [
    CommonModule,
    IdentityGuardsRoutingModule
  ],
  exports: [

  ]
})
export class IdentityGuardsModule {
  static forGuardRoot(config: IdentityGuardsConfig = {getConfigValues: () => new IdentityGuardsSettings()}): ModuleWithProviders<IdentityGuardsModule> {
      return {
      ngModule: IdentityGuardsModule,
      providers: [
        { provide: 'identity_guards_config', useValue: config }
      ]
    };
  }
}

