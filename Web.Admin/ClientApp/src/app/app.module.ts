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
import { ValidationDbValidationsComponent } from './components/validation-db-validations/validation-db-validations.component';
import { ValidationDbDependenciesComponent } from './components/validation-db-dependencies/validation-db-dependencies.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    LegacyFileUploadComponent,
    ValidationDbValidationsComponent,
    ValidationDbDependenciesComponent
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
