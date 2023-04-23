import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IncidentsUiRoutingModule } from 'incidents-ui';

const routes: Routes = [
  { path: '', children: IncidentsUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IncidentsRoutingModule { }
