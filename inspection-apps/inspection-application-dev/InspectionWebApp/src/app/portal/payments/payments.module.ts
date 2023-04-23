import { NgModule } from '@angular/core';
import { PaymentsUiModule } from 'payments-ui';
import { PaymentsRoutingModule } from './payments-routing.module';

@NgModule({
  imports: [
    PaymentsUiModule,
    PaymentsRoutingModule
  ],
  declarations: []
})
export class PaymentsModule { }
