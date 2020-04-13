import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { OrganizationProviderService } from '../../../services/ajax/organization/organization-provider.service';
import { OrganizationListParameters } from '../../../models/organization/OrganizationListParameters';
import { OrganizationList } from '../../../models/organization/OrganizationList';
import { Organization } from '../../../models/organization/Organization';
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { AcvpUserCreateParameters } from '../../../models/AcvpUser/AcvpUserCreateParameters';
import { PersonCreateParameters } from '../../../models/person/PersonCreateParameters';
import { Result } from '../../../models/responses/Result';

@Component({
  selector: 'app-acvp-users-new-user',
  templateUrl: './acvp-users-new-user.component.html',
  styleUrls: ['./acvp-users-new-user.component.scss']
})
export class AcvpUsersNewUserComponent implements OnInit {

  NewUserInputPage: number;
  newUserParams: AcvpUserCreateParameters;
  listData: OrganizationListParameters;
  organizations: OrganizationList;

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  selectedOrganization: Organization;

  constructor(private OrganizationService: OrganizationProviderService, private ACVPUserService: AcvpUserDataProviderService) { }

  loadData() {
    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => {
        this.organizations = data;
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.listData.page = 1;
    }
    else if (whichPage == "previous") {
      if (this.listData.page > 1) {
        this.listData.page = --this.listData.page;
      }
    }
    else if (whichPage == "next") {
      if (this.listData.page < this.organizations.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.organizations.totalPages;
    }

    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => {
        this.organizations = data;
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

  submitNewUser() {

    // Should do some basic input-validity checks here eventually

    // Set the organication ID in the params going out
    this.newUserParams.person.OrganizationId = this.selectedOrganization.id;

    // Then submit them
    this.ACVPUserService.createAcvpUser(this.newUserParams).subscribe(
      data => {
        this.notifyParentComponent.emit(data);
      }
    );
  }

  ngOnInit() {

    this.listData = new OrganizationListParameters("","");
    this.listData.page = 1;
    this.listData.pageSize = 10;

    this.NewUserInputPage = 0;

    this.newUserParams = new AcvpUserCreateParameters();
    this.newUserParams.person = new PersonCreateParameters();

    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => {
        this.organizations = data;
      }
    );
  }

}
