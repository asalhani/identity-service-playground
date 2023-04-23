import { NgModule } from '@angular/core';
import { DashboardUiModule } from 'inspection-dashboard-ui';
import { DashboardRoutingModule } from './dashboard-routing.module';

@NgModule({
  imports: [
    DashboardUiModule,
    DashboardRoutingModule
  ],
  declarations: []
})
export class DashboardModule { }
