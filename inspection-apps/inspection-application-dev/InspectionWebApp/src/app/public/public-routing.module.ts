import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PortalRoutingModule } from '../portal/portal-routing.module';

const routes: Routes = [
  // { path: '', component: HomeComponent },
  { path: '', redirectTo: 'portal', pathMatch: 'full' },
  { path: 'portal', children: PortalRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {
  static getRoutes(): Routes {
    return routes;
  }
}
