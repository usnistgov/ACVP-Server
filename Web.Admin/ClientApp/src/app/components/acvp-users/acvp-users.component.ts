import { Component, OnInit } from '@angular/core';
import { AcvpUserList } from '../../models/AcvpUser/AcvpUserList';
import { AcvpUserDataProviderService } from '../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { AcvpUserListParameters } from '../../models/AcvpUser/AcvpUserListParameters';
import { Router } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { AcvpUserCreateParameters } from '../../models/AcvpUser/AcvpUserCreateParameters';
import { PersonCreateParameters } from '../../models/person/PersonCreateParameters';

@Component({
  selector: 'app-acvp-users',
  templateUrl: './acvp-users.component.html',
  styleUrls: ['./acvp-users.component.scss']
})
export class AcvpUsersComponent implements OnInit {

  users: AcvpUserList;
  listData: AcvpUserListParameters;
  newUserParams: AcvpUserCreateParameters;

  constructor(private AcvpUserDataService: AcvpUserDataProviderService, private router: Router, private ModalSvc: ModalService) { }

  showNewUserDialog() {
    this.ModalSvc.showModal('AddUserModal');
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
      if (this.listData.page < this.users.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.users.totalPages;
    }

    this.AcvpUserDataService.getAcvpUsers(this.listData).subscribe(
      data => {
        this.users = data;
        this.router.navigate([], {
          queryParams: { page: this.listData.page },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

  submitNewUser() {
    this.AcvpUserDataService.createAcvpUser(this.newUserParams).subscribe(
      data => {
        this.ModalSvc.hideModal('NewUserModalOne');

        // Refresh the listing page
        this.ngOnInit();
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  onNewUserAdded(message: Event) {

    this.ModalSvc.hideModal('AddUserModal');
    console.log("Here we are");
    // Refresh the page
    this.ngOnInit();
  }

  deleteUser(userId: number) {
    // Stub for now
  }

  ngOnInit() {

    this.listData = new AcvpUserListParameters();
    this.listData.page = 1;
    this.listData.pageSize = 10;

    this.newUserParams = new AcvpUserCreateParameters();
    this.newUserParams.person = new PersonCreateParameters();

    this.AcvpUserDataService.getAcvpUsers(this.listData).subscribe(
      data => {
        this.users = data;
      });
  };

}
