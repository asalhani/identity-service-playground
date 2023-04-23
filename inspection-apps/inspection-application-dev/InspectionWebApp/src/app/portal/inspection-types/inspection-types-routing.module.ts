import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectionTypesUiRoutingModule } from 'inspection-process-ui';

const routes: Routes = [
  { path: '', children: InspectionTypesUiRoutingModule.getRoutes() }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspectionTypesRoutingModule { }
