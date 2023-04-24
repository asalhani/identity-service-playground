import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {IdentityLibModule, IdentityUiSettings} from "identty-lib";


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    IdentityLibModule.forLibRoot({ getConfigValues: getConfigValuesForIdentityUi })
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
