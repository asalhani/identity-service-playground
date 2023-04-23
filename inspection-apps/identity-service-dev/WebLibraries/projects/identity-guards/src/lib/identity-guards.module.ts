import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityGuardsComponent } from './identity-guards.component';
//import { AuthGuardService } from './services/auth-guard.service';
import { AuthCallbackComponent } from './views/auth-callback/auth-callback.component';
import { IdentityGuardsRoutingModule } from './identity-guards-routing.module';
import { IdentityGuardsConfig } from './models/identity-guards-config';
import { SilentRefreshComponent } from './views/silent-refresh/silent-refresh.component';

@NgModule({
  imports: [
    CommonModule,
    IdentityGuardsRoutingModule
  ],
  declarations: [
    IdentityGuardsComponent,
    AuthCallbackComponent,
    SilentRefreshComponent
  ],
  exports: [
    //AuthGuardService
    //IdentityGuardsComponent
  ]
})
export class IdentityGuardsModule {
  static forRoot(config: IdentityGuardsConfig = {}): ModuleWithProviders {
    return {
      ngModule: IdentityGuardsModule,
      providers: [
        { provide: 'identity_guards_config', useValue: config }
      ]
    };
  }
 }
