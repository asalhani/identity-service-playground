import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule, PopoverModule, CollapseModule, ModalModule } from 'ngx-bootstrap';
import { PortalRoutingModule } from './portal-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { AppConfigService } from '../config/app-config.service';
import { NewLayoutComponent } from './_layout/new-layout/new-layout.component';
import { NewHeaderComponent } from './_layout/new-header/new-header.component';
import { NewNavComponent } from './_layout/new-nav/new-nav.component';
import { CONFIG_TOKEN_KEY, INBOX_WIDGET_TOKEN_KEY } from '../config/constants';
import { InboxWidgetService } from './services/inbox-widget.service';
import localeAr from '@angular/common/locales/ar';

// This module is a container of sub shell modules that lazy load each library to boost and isolate each one context.
//
// This is due an inherent issue in Angular 6/7,
// where external node_modules libraries cannot be lazy loaded directly.
// https://github.com/angular/angular-cli/issues/6373#issuecomment-319116889
//
@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    PortalRoutingModule,
    TranslateModule.forRoot(),
    BsDropdownModule.forRoot(),
    PopoverModule.forRoot(),
    ModalModule.forRoot(),
    CollapseModule.forRoot(),
  ],
  declarations: [
    NewLayoutComponent,
    NewHeaderComponent,
    NewNavComponent
  ],
  exports: [
    BsDropdownModule,
    PopoverModule,
    ModalModule,
    CollapseModule,
    PortalRoutingModule
  ],
  providers: [
    {
      provide: CONFIG_TOKEN_KEY, useValue: { getConfigValue: getConfigValue }
    },
    {
      provide: INBOX_WIDGET_TOKEN_KEY, useExisting: InboxWidgetService
    },
    { provide: LOCALE_ID, useValue: 'ar-SA' }
  ]
})

export class PortalModule {
  constructor() {
    registerLocaleData(localeAr, 'ar-SA');
  }
}

export function getConfigValue(key: string): string {
  return AppConfigService.settings[key];
}
