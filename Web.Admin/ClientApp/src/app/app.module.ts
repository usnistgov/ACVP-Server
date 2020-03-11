import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FormsModule } from '@angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AboutComponent } from './components/about/about.component';

import { NgxFileDropModule } from 'ngx-file-drop';

// These two additional ones are for the ngx-file-upload library used for LCAVP uploads.  However, the HTTP
// one may be necessary once implementing AJAX shortly
import { HttpClientModule } from '@angular/common/http';
import { LegacyFileUploadComponent } from './components/legacy-file-upload/legacy-file-upload.component';
import { ValidationDbValidationComponent } from './components/validation-db-validation/validation-db-validation.component';
import { ValidationDbValidationsComponent } from './components/validation-db-validations/validation-db-validations.component';
import { ValidationDbDependenciesComponent } from './components/validation-db-dependencies/validation-db-dependencies.component';
import { ValidationDbDependencyComponent } from './components/validation-db-dependency/validation-db-dependency.component';
import { ValidationDbOeComponent } from './components/validation-db-oe/validation-db-oe.component';
import { ValidationDbOEsComponent } from './components/validation-db-oes/validation-db-oes.component';
import { ValidationDbProductComponent } from './components/validation-db-product/validation-db-product.component';
import { ValidationDbProductsComponent } from './components/validation-db-products/validation-db-products.component';
import { ValidationDbPersonComponent } from './components/validation-db-person/validation-db-person.component';
import { ValidationDbPersonsComponent } from './components/validation-db-persons/validation-db-persons.component';
import { ValidationDbOrganizationComponent } from './components/validation-db-organization/validation-db-organization.component';
import { ValidationDbOrganizationsComponent } from './components/validation-db-organizations/validation-db-organizations.component';
import { WorkflowComponent } from './components/workflow/workflow/workflow.component';
import { WorkflowDependencyCreateComponent } from './components/workflow/dependency/create/workflow-dependency-create.component';
import { WorkflowOrganizationCreateComponent } from './components/workflow/organization/create/workflow-organization-create.component';
import { WorkflowOeCreateComponent } from './components/workflow/operatingEnvironment/create/workflow-oe-create.component';
import { WorkflowPersonCreateComponent } from './components/workflow/person/create/workflow-person-create.component';
import { WorkflowProductCreateComponent } from './components/workflow/product/create/workflow-product-create.component';
import { WorkflowValidationCreateComponent } from './components/workflow/validation/create/workflow-validation-create.component';
import { WorkflowDependencyUpdateComponent } from './components/workflow/dependency/update/workflow-dependency-update.component';
import { WorkflowOeUpdateComponent } from './components/workflow/operatingEnvironment/update/workflow-oe-update.component';
import { WorkflowOrganizationUpdateComponent } from './components/workflow/organization/update/workflow-organization-update.component';
import { WorkflowPersonUpdateComponent } from './components/workflow/person/update/workflow-person-update.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    LegacyFileUploadComponent,
    ValidationDbValidationComponent,
    ValidationDbValidationsComponent,
    ValidationDbDependencyComponent,
    ValidationDbDependenciesComponent,
    ValidationDbOeComponent,
    ValidationDbOEsComponent,
    ValidationDbProductComponent,
    ValidationDbProductsComponent,
    ValidationDbPersonComponent,
    ValidationDbPersonsComponent,
    ValidationDbOrganizationComponent,
    ValidationDbOrganizationsComponent,
    WorkflowDependencyCreateComponent,
    WorkflowComponent,
    WorkflowOrganizationCreateComponent,
    WorkflowOeCreateComponent,
    WorkflowPersonCreateComponent,
    WorkflowProductCreateComponent,
    WorkflowValidationCreateComponent,
    WorkflowDependencyUpdateComponent,
    WorkflowOeUpdateComponent,
    WorkflowOrganizationUpdateComponent,
    WorkflowPersonUpdateComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,

    // For NgxFileUpload
    HttpClientModule,
    NgxFileDropModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
