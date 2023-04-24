import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {IdentityLibModule} from "identty-lib";


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    IdentityLibModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
