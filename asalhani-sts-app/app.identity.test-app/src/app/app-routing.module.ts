import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./home/home.component";
import {ProtectedComponent} from "./protected/protected.component";
import {IdentityGuardsRoutingModule, AuthGuardService, IdentityUiRoutingModule} from "identty-lib";

const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  {
    path: 'home', component: HomeComponent
  },
  {
    path: 'identity', children: IdentityUiRoutingModule.getRoutes()
  }, {
    path: 'identity-guards', children: IdentityGuardsRoutingModule.getRoutes()
  }, {
    path: 'protected',
    component: ProtectedComponent,
    canActivate: [AuthGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
