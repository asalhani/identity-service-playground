import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InspectionUserManagementUiRoutingModule } from 'inspection-user-management-ui';

const routes: Routes = [
  { path: '', children: InspectionUserManagementUiRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
