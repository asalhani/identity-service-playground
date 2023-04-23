import { NgModule } from '@angular/core';
import { InspectionUserManagementUiModule } from 'inspection-user-management-ui';
import { UsersRoutingModule } from './users-routing.module';

@NgModule({
  imports: [
    InspectionUserManagementUiModule,
    UsersRoutingModule
  ],
  declarations: []
})
export class UsersModule { }
