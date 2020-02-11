import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component'
import { LegacyFileUploadComponent } from './components/legacy-file-upload/legacy-file-upload.component'
import { ValidationDbValidationsComponent } from './components/validation-db-validations/validation-db-validations.component';
import { ValidationDbDependenciesComponent } from './components/validation-db-dependencies/validation-db-dependencies.component';
import { ValidationDbDependencyComponent } from './components/validation-db-dependency/validation-db-dependency.component';
import { ValidationDbOeComponent } from './components/validation-db-oe/validation-db-oe.component';
import { ValidationDbProductComponent } from './components/validation-db-product/validation-db-product.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
