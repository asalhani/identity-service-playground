import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FacilityUIRoutingModule } from 'facility-ui';

const routes: Routes = [
  { path: '', children: FacilityUIRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FacilityRoutingModule { }
