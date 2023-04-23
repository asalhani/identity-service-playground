import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PaymentUIRoutingModule } from 'payments-ui';

const routes: Routes = [
  { path: '', children: PaymentUIRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaymentsRoutingModule { }
