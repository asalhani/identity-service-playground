import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FieldSupportUiRoutingModule } from 'field-support-ui';

const routes: Routes = [
  { path: '', children: FieldSupportUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FieldSupportRoutingModule { }
