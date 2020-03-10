import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component'
import { LegacyFileUploadComponent } from './components/legacy-file-upload/legacy-file-upload.component'
import { ValidationDbValidationsComponent } from './components/validation-db-validations/validation-db-validations.component';
import { ValidationDbDependenciesComponent } from './components/validation-db-dependencies/validation-db-dependencies.component';
import { ValidationDbDependencyComponent } from './components/validation-db-dependency/validation-db-dependency.component';
import { ValidationDbOeComponent } from './components/validation-db-oe/validation-db-oe.component';
import { ValidationDbProductComponent } from './components/validation-db-product/validation-db-product.component';
import { ValidationDbProductsComponent } from './components/validation-db-products/validation-db-products.component';
import { ValidationDbOEsComponent } from './components/validation-db-oes/validation-db-oes.component';
import { ValidationDbPersonComponent } from './components/validation-db-person/validation-db-person.component';
import { ValidationDbPersonsComponent } from './components/validation-db-persons/validation-db-persons.component';
import { ValidationDbOrganizationComponent } from './components/validation-db-organization/validation-db-organization.component';
import { ValidationDbOrganizationsComponent } from './components/validation-db-organizations/validation-db-organizations.component';

// This is a parent module that loads all the other workflow modules itself
import { WorkflowComponent } from './components/workflow/workflow/workflow.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'legacyFileUpload',
    component: LegacyFileUploadComponent
  },
  {
    path: 'validation-db/validations',
    component: ValidationDbValidationsComponent
  },
  {
    path: 'validation-db/dependencies',
    component: ValidationDbDependenciesComponent
  },
  {
    path: 'validation-db/dependencies/:id',
    component: ValidationDbDependencyComponent
  },
  {
    path: 'validation-db/operatingEnvironments/:id',
    component: ValidationDbOeComponent
  },
  {
    path: 'validation-db/products/:id',
    component: ValidationDbProductComponent
  },
  {
    path: 'validation-db/products',
    component: ValidationDbProductsComponent
  },
  {
    path: 'validation-db/operatingEnvironments',
    component: ValidationDbOEsComponent
  },
  {
    path: 'validation-db/persons/:id',
    component: ValidationDbPersonComponent
  },
  {
    path: 'validation-db/persons',
    component: ValidationDbPersonsComponent
  },
  {
    path: 'validation-db/organizations',
    component: ValidationDbOrganizationsComponent
  },
  {
    path: 'validation-db/organizations/:id',
    component: ValidationDbOrganizationComponent
  },
  {
    // This is a parent module that loads all the other workflow modules itself
    path: 'workflow/:id',
    component: WorkflowComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
