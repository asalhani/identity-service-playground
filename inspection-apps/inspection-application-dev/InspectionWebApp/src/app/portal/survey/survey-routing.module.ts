import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SurveyorUIRoutingModule } from 'surveyor-ui';

const routes: Routes = [
  { path: '', children: SurveyorUIRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SurveyRoutingModule { }
