import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IdentityUiRoutingModule } from 'identity-ui';

const routes: Routes = [
  {
    path: 'identity', children: IdentityUiRoutingModule.getRoutes()
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})

export class AppRoutingModule { }
