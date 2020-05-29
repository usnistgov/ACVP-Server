import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ValidationList } from '../../../models/validation/ValidationList';
import { ValidationListParameters } from '../../../models/validation/ValidationListParameters';
import { Validation } from '../../../models/validation/Validation';

@Injectable({
  providedIn: 'root'
})
export class ValidationProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Begin TestSession-related calls
  getValidations(params: ValidationListParameters) {
    console.log(params);
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.productName === "") { params.productName = null; }
    if (params.validationId === "") { params.validationId = null; }
    if (params.validationLabel === "") { params.validationLabel = null; }

    var slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "productName": params.productName,
      "validationId": parseInt(params.validationId),
      "validationLabel": params.validationLabel
    }

    return this.http.post<ValidationList>(this.apiRoot + '/Validations', slightlyReformatted);
  }

  getValidation(validationId: number) {
    return this.http.get<Validation>(this.apiRoot + '/Validations/' + validationId);
  }
}
