import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LegacyFileUploadComponent } from '../components/legacy-file-upload/legacy-file-upload.component';
import { ValidationDbValidationsComponent } from '../components/validation-db-validations/validation-db-validations.component';
import { ValidationDbDependenciesComponent } from '../components/validation-db-dependencies/validation-db-dependencies.component';
import { ValidationDbDependencyComponent } from '../components/validation-db-dependency/validation-db-dependency.component';
import { ValidationDbOeComponent } from '../components/validation-db-oe/validation-db-oe.component';
import { ValidationDbProductComponent } from '../components/validation-db-product/validation-db-product.component';
import { ValidationDbProductsComponent } from '../components/validation-db-products/validation-db-products.component';
import { ValidationDbOEsComponent } from '../components/validation-db-oes/validation-db-oes.component';
import { ValidationDbPersonComponent } from '../components/validation-db-person/validation-db-person.component';
import { ValidationDbPersonsComponent } from '../components/validation-db-persons/validation-db-persons.component';
import { ValidationDbOrganizationComponent } from '../components/validation-db-organization/validation-db-organization.component';
import { ValidationDbOrganizationsComponent } from '../components/validation-db-organizations/validation-db-organizations.component';

// This is a parent module that loads all the other workflow modules itself
import { WorkflowComponent } from '../components/workflow/workflow/workflow.component';
import { WorkflowsComponent } from '../components/workflows/workflows.component';
import { ValidationDbValidationComponent } from '../components/validation-db-validation/validation-db-validation.component';
import { MessageQueueComponent } from '../components/messageQueue/message-queue.component';
import { TaskQueueComponent } from '../components/taskQueue/task-queue.component';
import { CurrentUserComponent } from '../components/currentUser/currentUser.component';
import { AcvpUsersComponent } from '../components/acvp-users/acvp-users.component';
import { AcvpUserComponent } from '../components/acvp-user/acvp-user.component';
import { DisclaimerRouteGuard } from './disclaimer-route-guard.module';
import { DisclaimerComponent } from '../disclaimer/disclaimer.component';
import { TestsessionsComponent } from '../components/testsessions/testsessions.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'workflow',
    pathMatch: 'full'
  },
  {
    path: 'testSessions',
    component: TestsessionsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'testSessions/:id',
    component: TestsessionsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'disclaimer',
    component: DisclaimerComponent
  },
  {
    path: 'acvpUsers/:id',
    component: AcvpUserComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'acvpUsers',
    component: AcvpUsersComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'currentUser',
    component: CurrentUserComponent
  },
  {
    path: 'legacyFileUpload',
    component: LegacyFileUploadComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/validations',
    component: ValidationDbValidationsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/validations/:id',
    component: ValidationDbValidationComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/dependencies',
    component: ValidationDbDependenciesComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/dependencies/:id',
    component: ValidationDbDependencyComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/operatingEnvironments/:id',
    component: ValidationDbOeComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/products/:id',
    component: ValidationDbProductComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/products',
    component: ValidationDbProductsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/operatingEnvironments',
    component: ValidationDbOEsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/persons/:id',
    component: ValidationDbPersonComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/persons',
    component: ValidationDbPersonsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/organizations',
    component: ValidationDbOrganizationsComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'validation-db/organizations/:id',
    component: ValidationDbOrganizationComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'messageQueue',
    component: MessageQueueComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    path: 'taskQueue',
    component: TaskQueueComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    // This is a parent module that loads all the other workflow modules itself
    path: 'workflow/:id',
    component: WorkflowComponent,
    canActivate: [DisclaimerRouteGuard]
  },
  {
    // This is a parent module that loads all the other workflow modules itself
    path: 'workflow',
    component: WorkflowsComponent,
    canActivate: [DisclaimerRouteGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
