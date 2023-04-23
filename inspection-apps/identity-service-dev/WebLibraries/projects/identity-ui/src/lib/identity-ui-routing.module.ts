import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginViewComponent } from './views/login-view/login-view.component';
import { ForgotPasswordViewComponent } from './views/forgot-password-view/forgot-password-view.component';
import { ResetPasswordViewComponent } from './views/reset-password-view/reset-password-view.component';
import { IdentityUiComponent } from './identity-ui.component';
import { LogoutViewComponent } from './views/logout-view/logout-view.component';

const routes: Routes = [
  {
    path: '', component: IdentityUiComponent,
    children: [
      { path: 'login', component: LoginViewComponent, data: { title: 'login' } },
      { path: 'logout', component: LogoutViewComponent, data: { title: 'logout' } },
      { path: 'forgot-password', component: ForgotPasswordViewComponent, data: { title: 'forgot-password' } },
      { path: 'reset-password/:token', component: ResetPasswordViewComponent, data: { title: 'reset-password' } }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityUiRoutingModule {
  static getRoutes(): Routes {
    return routes;
  }
}
