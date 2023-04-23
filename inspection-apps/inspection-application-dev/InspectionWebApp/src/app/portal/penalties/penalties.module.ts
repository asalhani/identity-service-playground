import { NgModule } from '@angular/core';
import { PenaltiesRoutingModule } from './penalties.routing.module';
import { PenaltiesUiModule } from 'penalties-ui';

@NgModule({
  imports: [
    PenaltiesUiModule,
    PenaltiesRoutingModule,
  ],
  exports: [PenaltiesRoutingModule]
})
export class PenaltiesModule { }
