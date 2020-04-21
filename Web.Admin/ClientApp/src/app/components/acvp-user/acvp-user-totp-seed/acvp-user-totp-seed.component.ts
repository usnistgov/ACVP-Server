import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { ModalService } from '../../../services/modal/modal.service';
import { FormsModule } from "@angular/forms";
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { Result } from '../../../models/responses/Result';
import { AcvpUserSeedUpdateParameters } from '../../../models/AcvpUser/AcvpUserSeedUpdateParameters';

@Component({
  selector: 'app-acvp-user-totp-seed',
  templateUrl: './acvp-user-totp-seed.component.html',
  styleUrls: ['./acvp-user-totp-seed.component.scss']
})
export class AcvpUserTotpSeedComponent implements OnInit {

  selectedUser: AcvpUser;
  editSeedValue: string = '';

  constructor(private UserService: AcvpUserDataProviderService, private ModalService: ModalService) { }

  raiseEditSeedModal() {
    this.ModalService.showModal('EditSeedModal');
  }

  hideEditSeedModal() {
    this.ModalService.hideModal('EditSeedModal');
  }

  raiseRefreshSeedConfirmation() {
    this.ModalService.showModal('RefreshSeedConfirmation');
  }

  hideRefreshSeedConfirmation() {
    this.ModalService.hideModal('RefreshSeedConfirmation');
  }

  submitRefreshSeedModal() {
    this.UserService.refreshSeed(this.selectedUser.acvpUserId).subscribe(
      data => { this.hideRefreshSeedConfirmation(); this.notifyParentComponent.emit(data); });
  }

  submitEditSeedModal() {
    this.UserService.updateSeed(this.selectedUser.acvpUserId, new AcvpUserSeedUpdateParameters(this.editSeedValue)).subscribe(
      data => { this.hideEditSeedModal(); this.notifyParentComponent.emit(data); });
  }

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  @Input()
  set user(selectedUser: AcvpUser) {
    this.selectedUser = selectedUser;
    if (selectedUser) {
      this.editSeedValue = selectedUser.seed;
    }
  }

  ngOnInit() {
  }

}
