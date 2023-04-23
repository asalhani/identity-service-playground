import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProcessReviewsUiModule } from 'inspection-process-ui';
import { ProcessReviewsModuleRoutingModule } from './process-reviews-routing.module';

@NgModule({
  imports: [
    CommonModule,
    ProcessReviewsUiModule,
    ProcessReviewsModuleRoutingModule
  ],
  declarations: []
})
export class ProcessReviewsModule { }
