import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {IdentityLibModule, IdentityUiSettings, IdentityGuardsModule, IdentityGuardsSettings} from "identty-lib";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    IdentityLibModule.forLibRoot({ getConfigValues: getConfigValuesForIdentityUi }),
    IdentityGuardsModule.forGuardRoot({ getConfigValues: getConfigValuesForIdentityGurads }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function getConfigValuesForIdentityUi(): IdentityUiSettings {
  return {
    identityServerEndpoint: 'http://localhost:5006',
  };
}

export function getConfigValuesForIdentityGurads(): IdentityGuardsSettings {
  return {
    enableConsoleLogging: true,
    postLoginRedirectUrl: '/protected',
    oidcSettings: {
      authority: 'http://localhost:5000',
      client_id: 'local_spa',
      redirect_uri: 'http://localhost:4200/#/identity-guards/auth-callback#',
      post_logout_redirect_uri: 'http://localhost:4200',
      response_type: 'id_token token',
      // tslint:disable-next-line:max-line-length
      scope: 'openid profile inspection_profile',
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      // silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
      silent_redirect_uri: 'http://localhost:4200/#/identity-guards/silent-refresh#'
    }
  };
}


