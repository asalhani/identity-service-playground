import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectionProcessUiRoutingModule } from 'inspection-process-ui';

const routes: Routes = [
  { path: '', children: InspectionProcessUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcessRoutingModule { }
