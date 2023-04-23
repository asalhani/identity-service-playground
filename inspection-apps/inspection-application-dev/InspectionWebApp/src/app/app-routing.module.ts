import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { IdentityGuardsRoutingModule } from 'identity-guards';
import { PortalRoutingModule } from './portal/portal-routing.module';
import { PublicRoutingModule } from './public/public-routing.module';

const routes: Routes = [
  { path: '', redirectTo: 'public', pathMatch: 'full' },
  { path: 'portal', children: PortalRoutingModule.getRoutes() },
  { path: 'public', children: PublicRoutingModule.getRoutes() },
  { path: 'identity-guards', children: IdentityGuardsRoutingModule.getRoutes() },
  { path: '**', component: PageNotFoundComponent, data: { title: 'Not Found' } },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
