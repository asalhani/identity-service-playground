import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProtectedComponent } from './protected/protected.component';
import { IdentityUiRoutingModule } from 'projects/identity-ui/src/lib/identity-ui-routing.module';
import { AuthGuardService } from 'projects/identity-guards/src/lib/services/auth-guard.service';
import { IdentityGuardsRoutingModule } from 'projects/identity-guards/src/lib/identity-guards-routing.module';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent
  }, {
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
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
