import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ElmBpmLibUiRoutingModule } from 'elm-bpm-lib';

const routes: Routes = [
  { path: '', children: ElmBpmLibUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkflowsRoutingModule { }
