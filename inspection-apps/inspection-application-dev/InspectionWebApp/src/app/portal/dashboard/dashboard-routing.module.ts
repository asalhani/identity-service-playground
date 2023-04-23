import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardUIRoutingModule } from 'inspection-dashboard-ui';

const routes: Routes = [
  { path: '', children: DashboardUIRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
