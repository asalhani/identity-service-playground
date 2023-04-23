import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcessReviewsUiRoutingModule } from 'inspection-process-ui';

const routes: Routes = [
  { path: '', children: ProcessReviewsUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcessReviewsModuleRoutingModule { }
