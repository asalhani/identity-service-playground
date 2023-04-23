import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewLayoutComponent } from './_layout/new-layout/new-layout.component';
import { ElmBpmLibUiRoutingModule } from 'elm-bpm-lib';
import { AuthGuardService } from 'identity-guards';

const routes: Routes = [
  {
    path: '',
    component: NewLayoutComponent,
    canActivate: [AuthGuardService],
    children: [
      // {
      //   path: ''
      //   , redirectTo: 'dashboard'
      //   , pathMatch: 'full'
      // },
      // { path: 'dashboard', loadChildren: 'src/app/portal/dashboard/dashboard.module#DashboardModule' },
      { path: 'entity', loadChildren: 'src/app/portal/entity/entity.module#EntityModule' },
      { path: 'facility', loadChildren: 'src/app/portal/facility/facility.module#FacilityModule' },
      { path: 'centers', loadChildren: 'src/app/portal/centers/centers.module#CentersModule' },
      { path: 'users', loadChildren: 'src/app/portal/users/users.module#UsersModule' },
      { path: 'process', loadChildren: 'src/app/portal/process/process.module#ProcessModule' },
      { path: 'process-reviews', loadChildren: 'src/app/portal/process-reviews/process-reviews.module#ProcessReviewsModule' },
      { path: 'appeals', loadChildren: 'src/app/portal/appeals/appeals.module#AppealsModule' },
      { path: 'incidents', loadChildren: 'src/app/portal/incidents/incidents.module#IncidentsModule' },
      { path: 'fieldsupport', loadChildren: 'src/app/portal/fieldsupport/fieldsupport.module#FieldSupportModule' },
      { path: 'penalties', loadChildren: 'src/app/portal/penalties/penalties.module#PenaltiesModule' },
      { path: 'survey', loadChildren: 'src/app/portal/survey/survey.module#SurveyModule' },
      { path: 'payment', loadChildren: 'src/app/portal/payments/payments.module#PaymentsModule' },
      { path: 'inbox', loadChildren: 'src/app/portal/inbox/inbox.module#InboxModule' },
      { path: 'inspection-types', loadChildren: 'src/app/portal/inspection-types/inspection-types.module#InspectionTypesModule' },
      { path: 'workflow', loadChildren: 'src/app/portal/workflows/workflows.module#WorkflowsModule' }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PortalRoutingModule {
  static getRoutes(): Routes {
    return routes;
  }
}
