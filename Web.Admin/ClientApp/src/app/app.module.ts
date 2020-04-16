import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ClickableClickModule } from 'angular-clickable-click';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FormsModule } from '@angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AboutComponent } from './components/about/about.component';
import { CurrentUserComponent } from './components/currentUser/currentUser.component';

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
import { WorkflowProductUpdateComponent } from './components/workflow/product/update/workflow-product-update.component';
import { WorkflowsComponent } from './components/workflows/workflows.component';
import { WorkflowDependencyDeleteComponent } from './components/workflow/dependency/delete/workflow-dependency-delete.component';
import { WorkflowOeDeleteComponent } from './components/workflow/operatingEnvironment/delete/workflow-oe-delete.component';
import { WorkflowOrganizationDeleteComponent } from './components/workflow/organization/delete/workflow-organization-delete.component';
import { WorkflowPersonDeleteComponent } from './components/workflow/person/delete/workflow-person-delete.component';
import { WorkflowProductDeleteComponent } from './components/workflow/product/delete/workflow-product-delete.component';
import { MessageQueueComponent } from './components/messageQueue/message-queue.component';
import { TaskQueueComponent } from './components/taskQueue/task-queue.component';
import { AcvpUserComponent } from './components/acvp-user/acvp-user.component';
import { AcvpUsersComponent } from './components/acvp-users/acvp-users.component';
import { AcvpUsersNewUserComponent } from './components/acvp-users/acvp-users-new-user/acvp-users-new-user.component';
import { AcvpUserTotpSeedComponent } from './components/acvp-user/acvp-user-totp-seed/acvp-user-totp-seed.component';
import { AcvpUserCertificateComponent } from './components/acvp-user/acvp-user-certificate/acvp-user-certificate.component';
import { WorkflowActionsComponent } from './components/workflow/workflow/workflow-actions/workflow-actions.component';
import { FileUploadComponentComponent } from './components/file-upload-component/file-upload-component.component';
import { FileUploadComponent } from './components/file-upload-module/file-upload-module.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    CurrentUserComponent,
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
    WorkflowPersonUpdateComponent,
    WorkflowProductUpdateComponent,
    WorkflowsComponent,
    WorkflowDependencyDeleteComponent,
    WorkflowOeDeleteComponent,
    WorkflowOrganizationDeleteComponent,
    WorkflowPersonDeleteComponent,
    WorkflowProductDeleteComponent,
    MessageQueueComponent,
    TaskQueueComponent,
    AcvpUserComponent,
    AcvpUsersComponent,
    AcvpUsersNewUserComponent,
    AcvpUserTotpSeedComponent,
    AcvpUserCertificateComponent,
    WorkflowActionsComponent,
    FileUploadComponentComponent,
    FileUploadComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    ClickableClickModule,
    // For NgxFileUpload
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
