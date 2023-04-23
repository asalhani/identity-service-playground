import { NgModule } from '@angular/core';
import { ElmBpmLibModule } from 'elm-bpm-lib';
import { WorkflowsRoutingModule } from './workflows-routing.module';

@NgModule({
  imports: [
    ElmBpmLibModule,
    WorkflowsRoutingModule
  ],
  declarations: []
})
export class WorkflowsModule { }
