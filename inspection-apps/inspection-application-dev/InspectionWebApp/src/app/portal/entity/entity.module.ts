import { NgModule } from '@angular/core';
import { EntityAdministrationLibModule } from 'entity-administration-ui';
import { EntityRoutingModule } from './entity-routing.module';

@NgModule({
  imports: [
    EntityAdministrationLibModule,
    EntityRoutingModule
  ],
  declarations: []
})
export class EntityModule { }
