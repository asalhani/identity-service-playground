import { NgModule } from '@angular/core';
import { InspectionProcessUiModule } from 'inspection-process-ui';
import { ProcessRoutingModule } from './process-routing.module';

@NgModule({
  imports: [
    InspectionProcessUiModule,
    ProcessRoutingModule
  ],
  declarations: []
})
export class ProcessModule { }
