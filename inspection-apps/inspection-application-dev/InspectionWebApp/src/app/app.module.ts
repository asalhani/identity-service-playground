import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { IdentityGuardsModule, IdentityGuardsSettings } from 'identity-guards';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PortalModule } from './portal/portal.module';
import { AppConfigService } from './config/app-config.service';
import { NgHttpLoaderModule } from 'ng-http-loader';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { CONFIG_TOKEN_KEY, INBOX_WIDGET_TOKEN_KEY } from './config/constants';
import { AppTranslateHttpLoader } from './shared/app-translate-http-loader';
import { PublicModule } from './public/public.module';
import { PlatformLocation, APP_BASE_HREF, CommonModule } from '@angular/common';
import { LightboxModule } from 'ngx-lightbox';
import { InboxWidgetService } from './portal/services/inbox-widget.service';
import { NgIdleModule } from '@ng-idle/core'
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppHttpInterceptor } from 'elm-common-ui-helpers-library';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    RouterModule,
    BrowserAnimationsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient, PlatformLocation]
      },
      isolate: true
    }),
    AppRoutingModule,
    PublicModule,
    PortalModule,
    LightboxModule,
    NgHttpLoaderModule.forRoot(),
    IdentityGuardsModule.forRoot({ getConfigValues: getConfigValuesForIdentityGurads }),
    NgIdleModule.forRoot()
  ],
  exports: [
    TranslateModule
  ],
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
    {
      provide: 'lib_config', useValue: { getConfigValue: getConfigValue }
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AppHttpInterceptor,
      multi: true,
      deps: [Injector, AppConfigService]
    },
    {
      provide: CONFIG_TOKEN_KEY, useValue: { getConfigValue: getConfigValue }
    },
    {
      provide: INBOX_WIDGET_TOKEN_KEY, useExisting: InboxWidgetService
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

// Entry point of app initializations ..
export function initializeApp(appConfig: AppConfigService) {
  return () => appConfig.load();
}

// Configuration contracts ..
export function getConfigValue(key: string): string {
  return AppConfigService.settings[key];
}

export function getConfigValuesForIdentityGurads(): IdentityGuardsSettings {
  return AppConfigService.settings.IdentityGuardsConfig;
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
  return new AppTranslateHttpLoader(http, getBaseRootURL(platformLocation) + '/assets/i18n/', '.json');
}
