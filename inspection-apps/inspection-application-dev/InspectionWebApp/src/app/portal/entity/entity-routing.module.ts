import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EntityAdministrationLibRoutingModule } from 'entity-administration-ui';

const routes: Routes = [
  { path: '', children: EntityAdministrationLibRoutingModule.getRoutes() },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EntityRoutingModule { }
