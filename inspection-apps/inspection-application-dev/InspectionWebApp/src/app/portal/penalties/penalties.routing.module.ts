import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PenaltiesUiRoutingModule } from 'penalties-ui';

const routes: Routes = [
  { path: '', children: PenaltiesUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PenaltiesRoutingModule { }
