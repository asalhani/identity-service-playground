import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { IdentityUiModule } from 'projects/identity-ui/src/lib/identity-ui.module';
import { IdentityUiSettings } from 'projects/identity-ui/src/lib/models/identity-ui-settings';
import { IdentityGuardsModule } from 'projects/identity-guards/src/lib/identity-guards.module';
import { IdentityGuardsSettings } from 'projects/identity-guards/src/lib/models/identity-guards-settings';
import { AppRoutingModule } from './app-routing.module';
import { ProtectedComponent } from './protected/protected.component';
import { HomeComponent } from './home/home.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@NgModule({
  declarations: [
    AppComponent,
    ProtectedComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      },
      isolate: true
    }),
    AppRoutingModule,
    IdentityUiModule.forRoot({ getConfigValues: getConfigValuesForIdentityUi }),
    IdentityGuardsModule.forRoot({ getConfigValues: getConfigValuesForIdentityGurads }),
  ],
  exports: [
    TranslateModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }

export function getConfigValuesForIdentityUi(): IdentityUiSettings {
  return {
    recaptchaSiteKey: '6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI',
    identityServerEndpoint: 'http://localhost:5000',
    captchaType: 'Custom',
    AppTitle: 'تفتيش امانة الرياض',
    AppLogo:'assets/images/ruh.svg'
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

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
