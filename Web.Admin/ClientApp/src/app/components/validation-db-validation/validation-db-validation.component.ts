import { Component, OnInit } from '@angular/core';
import { Validation } from '../../models/validation/Validation';
import { ValidationProviderService } from '../../services/ajax/validation/validation-provider.service';
import { ActivatedRoute } from '@angular/router';
import { OrganizationProviderService } from '../../services/ajax/organization/organization-provider.service';
import { Organization } from '../../models/organization/Organization';
import { ValidationOEAlgorithm } from '../../models/validation/ValidationOEAlgorithm';

@Component({
  selector: 'app-validation-db-validation',
  templateUrl: './validation-db-validation.component.html',
  styleUrls: ['./validation-db-validation.component.scss']
})
export class ValidationDbValidationComponent implements OnInit {

  selectedValidation: Validation;
  organization: Organization;
  validationOEAlgorithms: ValidationOEAlgorithm[]
  someValue: string;

  constructor(private ValidationService: ValidationProviderService, private OrganizationService: OrganizationProviderService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ValidationService.getValidation(parseInt(params.get("id"))).subscribe(
        data => {
          this.selectedValidation = data;

          this.OrganizationService.getOrganization(this.selectedValidation.vendorId).subscribe(
            data => {
              this.organization = data;
            });

          //this.ValidationService.getValidationOEAlgorithms(parseInt(params.get("id"))).subscribe(
          //  data => {
          //    this.validationOEAlgorithms = data;
          //  });
        },
        err => { },
        () => { });

      this.ValidationService.getValidationOEAlgorithms(parseInt(params.get("id"))).subscribe(
        data => {
          this.validationOEAlgorithms = data.data;
        });
    });
  };

  LoadCapabilities(validationOEAlgorithm: ValidationOEAlgorithm) {
    if (!(validationOEAlgorithm.capabilityHTML)) {
      this.ValidationService.getCapabilityHTML(validationOEAlgorithm.validationOEAlgorithmID).subscribe(
        data => { validationOEAlgorithm.capabilityHTML = data.html; })
    }
    validationOEAlgorithm.capabilitiesExpanded = !validationOEAlgorithm.capabilitiesExpanded;
  };
};
