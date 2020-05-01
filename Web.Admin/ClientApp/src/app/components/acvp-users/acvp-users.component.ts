import { Component, OnInit } from '@angular/core';
import { AcvpUserList } from '../../models/AcvpUser/AcvpUserList';
import { AcvpUserDataProviderService } from '../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { AcvpUserListParameters } from '../../models/AcvpUser/AcvpUserListParameters';
import { Router } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { AcvpUserCreateParameters } from '../../models/AcvpUser/AcvpUserCreateParameters';
import { PersonCreateParameters } from '../../models/person/PersonCreateParameters';
import { Result } from '../../models/responses/Result';
import { AcvpUser } from '../../models/AcvpUser/AcvpUser';

@Component({
  selector: 'app-acvp-users',
  templateUrl: './acvp-users.component.html',
  styleUrls: ['./acvp-users.component.scss']
})
export class AcvpUsersComponent implements OnInit {

  users: AcvpUserList;
  listData: AcvpUserListParameters;
  newUserParams: AcvpUserCreateParameters;

  // Some basic objects to use for showing and calculating elapsed certs
  todayDate = Date.now();
  dateMachine = new Date(Date.now());
  twoMonthsInFutureDate = this.dateMachine.setDate(this.dateMachine.getDate() + 60);

  selectedDeleteUser: AcvpUser;

  constructor(private AcvpUserDataService: AcvpUserDataProviderService, private router: Router, private ModalSvc: ModalService) { }

  showNewUserDialog() {
    this.ModalSvc.showModal('AddUserModal');
  }

  showDeleteConfirmation(selectedDeleteUser: AcvpUser) {
    this.selectedDeleteUser = selectedDeleteUser;
    this.ModalSvc.showModal('DeleteConfirmationModal');
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

        this.users.data.forEach(function (item, index, array) {
          item.expiresOn = new Date(item.expiresOn);
        });

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
        this.loadData();
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  // This function is used as the handler for when the AddNewUser child component
  // emits the event to let this parent component know it's done
  onNewUserAdded(result: Result) {

    this.ModalSvc.hideModal('AddUserModal');
    // Refresh the page
    //this.ngOnInit();

    // Navigate to the new user's page
    this.router.navigate(['/acvpUsers/' + result.id]);
  }

  deleteUser(id: number) {
    this.AcvpUserDataService.deleteAcvpUser(id).subscribe(
      data => {
        this.AcvpUserDataService.getAcvpUsers(this.listData).subscribe(
          data => {
            this.loadData();
            this.ModalSvc.hideModal('DeleteConfirmationModal');
          });
      });
  }

  loadData() {
    var self = this;
    this.AcvpUserDataService.getAcvpUsers(this.listData).subscribe(
      data => {
        this.users = data;
        this.users.data.forEach(function (item, index, array) {
          // Not sure why, but apparently we have to parse these dates from Strings into Date objects
          // if we want to use them as such.  I would've thought they'd be parsed as Dates since that's their type...
          item.expiresOn = new Date(item.expiresOn);
        });
      });
  }

  ngOnInit() {

    this.listData = new AcvpUserListParameters();
    this.listData.page = 1;
    this.listData.pageSize = 10;

    this.newUserParams = new AcvpUserCreateParameters();
    this.newUserParams.person = new PersonCreateParameters();

    this.loadData();
  };

}
