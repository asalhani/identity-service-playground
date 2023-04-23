import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppealsUiRoutingModule } from 'appeals-ui';

const routes: Routes = [
  { path: '', children: AppealsUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppealsRoutingModule { }
