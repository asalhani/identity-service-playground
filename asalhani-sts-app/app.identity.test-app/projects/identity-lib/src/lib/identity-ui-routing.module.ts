import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./views/login/login.component";
import {IdentityLibComponent} from "./identity-lib.component";
import {LogoutComponent} from "./views/logout/logout.component";

const routes: Routes = [
  {
    path: '', component: IdentityLibComponent,
    children: [
      { path: 'login', component: LoginComponent, data: { title: 'login' } },
      { path: 'logout', component: LogoutComponent, data: { title: 'logout' } },
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
