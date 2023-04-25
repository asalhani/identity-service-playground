import {ModuleWithProviders, NgModule} from '@angular/core';
import {IdentityLibComponent} from './identity-lib.component';
import {RouterModule} from "@angular/router";
import {LoginPartComponent} from './views/login-part/login-part.component';
import {LoginComponent} from './views/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {CONFIG_TOKEN_KEY, IdentityUiConfig} from "./utils/identity-ui-config";
import {IdentityUiSettings} from "./models/identity-ui-settings";
import {HttpClientModule} from "@angular/common/http";
import {IdentityUiRoutingModule} from "./identity-ui-routing.module";


@NgModule({
  declarations: [
    IdentityLibComponent,
    LoginPartComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    IdentityUiRoutingModule
  ],
  exports: [
    IdentityLibComponent,
    LoginPartComponent,
    LoginComponent
  ]
})
export class IdentityLibModule {

  constructor() {
  }

  static forLibRoot(config: IdentityUiConfig = {getConfigValues: () => new IdentityUiSettings}): ModuleWithProviders<IdentityLibModule> {
    return {
      ngModule: IdentityLibModule,
      providers: [
        {provide: CONFIG_TOKEN_KEY, useValue: config}
      ]
    };
  }
}
