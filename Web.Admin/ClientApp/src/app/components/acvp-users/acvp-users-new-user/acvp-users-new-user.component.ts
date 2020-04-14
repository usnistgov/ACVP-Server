import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { OrganizationProviderService } from '../../../services/ajax/organization/organization-provider.service';
import { OrganizationListParameters } from '../../../models/organization/OrganizationListParameters';
import { OrganizationList } from '../../../models/organization/OrganizationList';
import { Organization } from '../../../models/organization/Organization';
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { AcvpUserCreateParameters } from '../../../models/AcvpUser/AcvpUserCreateParameters';
import { PersonCreateParameters } from '../../../models/person/PersonCreateParameters';
import { Result } from '../../../models/responses/Result';
import { ModalService } from '../../../services/modal/modal.service';

@Component({
  selector: 'app-acvp-users-new-user',
  templateUrl: './acvp-users-new-user.component.html',
  styleUrls: ['./acvp-users-new-user.component.scss']
})
export class AcvpUsersNewUserComponent implements OnInit {

  NewUserInputPage: number;
  newUserParams: AcvpUserCreateParameters;
  listParams: OrganizationListParameters;
  organizations: OrganizationList;
  selectedOrganization: Organization;

  // Error message to be used when an error is encountered
  modalFooterError: string;
  personNotProvidedFlag: Boolean;
  certificateNotProvidedFlag: Boolean;
  seedNotProvidedFlag: Boolean;

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  constructor(private OrganizationService: OrganizationProviderService, private ACVPUserService: AcvpUserDataProviderService, private ModalService: ModalService) { }

  loadData() {
    this.listParams.page = 1; // Reset to page one when search term changes
    this.OrganizationService.getOrganizations(this.listParams).subscribe(
      data => {
        this.organizations = data;
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  closeNewUserModal() {
    this.ModalService.hideModal('AddUserModal');

    // Reset the data used on that page, so it's clean next time they re-open it
    this.selectedOrganization = null;
    this.organizations.currentPage = 1;
    this.NewUserInputPage = 0;
    this.modalFooterError = "";
  }

  moveToNextPage() {

    if (this.selectedOrganization !== null) {
      this.NewUserInputPage = this.NewUserInputPage + 1;
      this.modalFooterError = "";
    }
    else {
      this.modalFooterError = "No organization selected.  Please choose an organization";
    }
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.listParams.page = 1;
    }
    else if (whichPage == "previous") {
      if (this.organizations.currentPage > 1) {
        this.listParams.page = --this.listParams.page;
      }
    }
    else if (whichPage == "next") {
      if (this.organizations.currentPage < this.organizations.totalPages) {
        this.listParams.page = ++this.listParams.page;
      }
    }
    else if (whichPage == "last") {
      this.listParams.page = this.organizations.totalPages;
    }

    this.OrganizationService.getOrganizations(this.listParams).subscribe(
      data => {
        this.organizations = data;
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

  submitNewUser() {

    var submitFlag: Boolean = true;

    this.personNotProvidedFlag = false;
    this.certificateNotProvidedFlag = false;
    this.seedNotProvidedFlag = false;

    console.log(this.newUserParams.person.Name);
    console.log(this.newUserParams.certificate);
    console.log(this.newUserParams.seed);

    // Super-basic input-validity checks
    if (this.newUserParams.person.Name === "" || typeof(this.newUserParams.person.Name) === 'undefined') {
      this.personNotProvidedFlag = true;
      submitFlag = false;
    }
    if (this.newUserParams.certificate === "" || typeof(this.newUserParams.certificate) === 'undefined') {
      this.certificateNotProvidedFlag = true;
      submitFlag = false;
    }
    if (this.newUserParams.seed === "" || typeof(this.newUserParams.seed) === 'undefined') {
      this.seedNotProvidedFlag = true;
      submitFlag = false;
    }

    if (submitFlag == true) {
      // Set the organization ID in the params going out
      this.newUserParams.person.OrganizationId = this.selectedOrganization.id;

      // Then submit them
      this.ACVPUserService.createAcvpUser(this.newUserParams).subscribe(
        data => {
          this.notifyParentComponent.emit(data);
        }
      );
    }
    else {

    }
  }

  ngOnInit() {

    this.listParams = new OrganizationListParameters("","");
    this.listParams.page = 1;
    this.listParams.pageSize = 10;

    this.selectedOrganization = null;

    this.NewUserInputPage = 0;

    this.newUserParams = new AcvpUserCreateParameters();
    this.newUserParams.person = new PersonCreateParameters();

    this.OrganizationService.getOrganizations(this.listParams).subscribe(
      data => {
        this.organizations = data;
      }
    );
  }

}
