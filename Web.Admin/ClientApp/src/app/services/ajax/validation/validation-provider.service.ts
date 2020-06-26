import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ValidationList } from '../../../models/validation/ValidationList';
import { ValidationListParameters } from '../../../models/validation/ValidationListParameters';
import { Validation } from '../../../models/validation/Validation';
import { ValidationOEAlgorithmList } from '../../../models/validation/ValidationOEAlgorithmList';
import { CapabilityDisplay } from '../../../models/validation/CapabilityDisplay';

@Injectable({
  providedIn: 'root'
})
export class ValidationProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Begin TestSession-related calls
  getValidations(params: ValidationListParameters) {
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.implementationName === "") { params.implementationName = null; }
    if (params.validationId === "") { params.validationId = null; }
    if (params.validationLabel === "") { params.validationLabel = null; }

    let slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "implementationName": params.implementationName,
      "validationId": parseInt(params.validationId),
      "validationLabel": params.validationLabel
    }

    return this.http.post<ValidationList>(this.apiRoot + '/Validations', slightlyReformatted);
  }

  getValidation(validationId: number) {
    return this.http.get<Validation>(this.apiRoot + '/Validations/' + validationId);
  }

  getValidationOEAlgorithms(validationId: number) {
    return this.http.get<ValidationOEAlgorithmList>(this.apiRoot + '/Validations/' + validationId + '/validationOEAlgorithms');
  }

  getCapabilityHTML(validationOEAlgorithmId: number) {
    return this.http.get<CapabilityDisplay>(this.apiRoot + '/Validations/' + 5 + '/validationOEAlgorithm/' + validationOEAlgorithmId);
  }
}
