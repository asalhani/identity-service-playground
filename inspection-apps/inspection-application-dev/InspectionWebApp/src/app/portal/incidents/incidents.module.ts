import { NgModule } from '@angular/core';
import { IncidentsUiModule } from 'incidents-ui';
import { IncidentsRoutingModule } from './incidents-routing.module';

@NgModule({
  imports: [
    IncidentsUiModule,
    IncidentsRoutingModule
  ],
  declarations: []
})
export class IncidentsModule { }
