import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { AppComponent } from './app.component';
import { IdentityUiModule, IdentityUiSettings } from 'identity-ui';
import { AppRoutingModule } from './app-routing.module';
import { AppConfigService } from './config/app-config.service';
import { APP_BASE_HREF, PlatformLocation } from '@angular/common';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient, PlatformLocation]
      },
      isolate: true
    }),
    IdentityUiModule.forRoot({ getConfigValues: getConfigValuesForIdentityUi }),
  ],
  exports: [TranslateModule],
  providers: [
    {
      provide: APP_BASE_HREF,
      useFactory: getBaseHref,
      deps: [PlatformLocation]
    },
    AppConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfigService], multi: true
    },
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }

export function initializeApp(appConfig: AppConfigService) {
  return () => appConfig.load();
}

export function getConfigValuesForIdentityUi(): IdentityUiSettings {
  return {
    recaptchaSiteKey: AppConfigService.settings.recaptchaSiteKey,
    identityServerEndpoint: AppConfigService.settings.identityServerEndpoint,
    captchaType: AppConfigService.settings.captchaType,
    AppTitle: AppConfigService.settings.AppTitle,
    AppLogo: AppConfigService.settings.AppLogo
  };
}

export function getBaseHref(platformLocation: PlatformLocation): string {
  const baseHrefURL: string = getBaseRootURL(platformLocation);
  return baseHrefURL;
}

export function getBaseRootURL(platformLocation: PlatformLocation): string {
  let baseHrefURL: string;
  baseHrefURL = platformLocation.getBaseHrefFromDOM();
  if (baseHrefURL === '/') {
    baseHrefURL = '';
  }
  return baseHrefURL;
}

export function createTranslateLoader(http: HttpClient, platformLocation: PlatformLocation) {
  return new TranslateHttpLoader(http, getBaseRootURL(platformLocation) + '/assets/i18n/', '.json');
}
