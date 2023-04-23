import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectionCenterUiRoutingModule } from 'inspection-center-ui';

const routes: Routes = [
  { path: '', children: InspectionCenterUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CentersRoutingModule { }
