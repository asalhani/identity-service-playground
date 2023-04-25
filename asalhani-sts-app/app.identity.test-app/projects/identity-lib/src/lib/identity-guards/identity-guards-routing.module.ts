import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {IdentityGuardsComponent} from "./identity-guards.component";
import {AuthCallbackComponent} from "./views/auth-callback/auth-callback.component";
import {SilentRefreshComponent} from "./views/silent-refresh/silent-refresh.component";
import {TestGComponent} from "./views/test-g.component";

const routes: Routes = [
  { path: '', component: IdentityGuardsComponent},
  { path: 'silent-refresh', component: SilentRefreshComponent },
  { path: 'auth-callback', component: AuthCallbackComponent },
  {
  path: 'test', component: TestGComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityGuardsRoutingModule {
  static getRoutes(): Routes {
    return routes;
  }
}
