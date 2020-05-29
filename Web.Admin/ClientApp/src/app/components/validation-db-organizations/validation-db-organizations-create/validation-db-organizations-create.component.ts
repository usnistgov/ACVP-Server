import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';
import { OrganizationProviderService } from '../../../services/ajax/organization/organization-provider.service';
import { OrganizationCreateParameters } from '../../../models/organization/OrganizationCreateParameters';
import { Result } from '../../../models/responses/Result';

@Component({
  selector: 'app-validation-db-organizations-create',
  templateUrl: './validation-db-organizations-create.component.html',
  styleUrls: ['./validation-db-organizations-create.component.scss']
})
export class ValidationDbOrganizationsCreateComponent implements OnInit {

  dataModelName = "";
  dataModelUrl = "";
  dataModelVoiceNumber = "";
  dataModelFaxNumber = "";

  noNameErrorFlag = false;;

  constructor(private organizationService: OrganizationProviderService) { }

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  submitNewOrgModal() {

    this.noNameErrorFlag = false;

    if (this.dataModelName !== "" && this.dataModelName !== null) {
      let param = new OrganizationCreateParameters();
      param.name = this.dataModelName;
      param.website = this.dataModelUrl;
      param.voiceNumber = this.dataModelVoiceNumber;
      param.faxNumber = this.dataModelFaxNumber;

      this.organizationService.createNewOrganization(param).subscribe(
        data => { this.notifyParentComponent.emit(data); });
    }
    else {
      this.noNameErrorFlag = true;
    }
  }

  ngOnInit() {
  }

}
