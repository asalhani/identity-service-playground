import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InspectionCenterUiModule } from 'inspection-center-ui';
import { CentersRoutingModule } from './centers-routing.module';

@NgModule({
  imports: [
    CommonModule,
    InspectionCenterUiModule,
    CentersRoutingModule
  ],
  declarations: []
})
export class CentersModule { }
