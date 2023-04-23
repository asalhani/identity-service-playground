import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InboxUiRoutingModule } from 'inspection-process-ui';

const routes: Routes = [
  { path: '', children: InboxUiRoutingModule.getRoutes() }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InboxRoutingModule { }
