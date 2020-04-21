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
    this.AcvpUserDataService.getAcvpUsers(this.listData).subscribe(
      data => {
        this.users = data;
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
