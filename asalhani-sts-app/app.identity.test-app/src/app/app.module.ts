import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {IdentityLibModule, IdentityUiSettings, IdentityGuardsModule, IdentityGuardsSettings} from "identty-lib";
import { ProtectedComponent } from './protected/protected.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    ProtectedComponent,
    HomeComponent
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
    identityServerEndpoint: 'https://localhost:5005',
  };
}

export function getConfigValuesForIdentityGurads(): IdentityGuardsSettings {
  return {
    enableConsoleLogging: true,
    postLoginRedirectUrl: '/protected',
    oidcSettings: {
      authority: 'https://localhost:5005',
      client_id: 'angular-spa-client',
      redirect_uri: 'http://localhost:4200/#/identity-guards/auth-callback#',
      scope: 'openid profile companyApi',
      response_type: 'code',
      post_logout_redirect_uri: 'http://localhost:4200/identity-guards/signout-callback#',
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/#/identity-guards/silent-refresh#',

      filterProtocolClaims: true,
      loadUserInfo: true,
    }
  };
}


