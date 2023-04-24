import { NgModule } from '@angular/core';
import { IdentityLibComponent } from './identity-lib.component';
import {RouterModule} from "@angular/router";
import { LoginPartComponent } from './views/login-part/login-part.component';
import { LoginComponent } from './views/login/login.component';



@NgModule({
  declarations: [
    IdentityLibComponent,
    LoginPartComponent,
    LoginComponent
  ],
  imports: [
    RouterModule
  ],
  exports: [
    IdentityLibComponent,
    LoginPartComponent,
    LoginComponent
  ]
})
export class IdentityLibModule { }
