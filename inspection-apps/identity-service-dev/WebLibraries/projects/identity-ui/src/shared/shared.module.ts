import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BsDropdownModule, PopoverModule, CollapseModule, ModalModule } from 'ngx-bootstrap';

@NgModule({
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    PopoverModule.forRoot(),
    ModalModule.forRoot(),
    CollapseModule.forRoot()
  ],
  exports: [
    BsDropdownModule,
    PopoverModule,
    ModalModule,
    CollapseModule
  ],
})
export class SharedModule { }
