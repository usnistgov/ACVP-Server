import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { ModalService } from '../../../services/modal/modal.service';
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { Result } from '../../../models/responses/Result';
import { AcvpUserCertificateUpdateParameters } from '../../../models/AcvpUser/AcvpUserCertificateUpdateParameters';

@Component({
  selector: 'app-acvp-user-certificate',
  templateUrl: './acvp-user-certificate.component.html',
  styleUrls: ['./acvp-user-certificate.component.scss']
})
export class AcvpUserCertificateComponent implements OnInit {

  selectedUser: AcvpUser;
  editCertificateValue: string = '';

  constructor(private UserService: AcvpUserDataProviderService, private ModalService: ModalService) { }

  raiseEditCertificateModal() {
    this.ModalService.showModal('EditCertificateModal');
  }

  hideEditCertificateModal() {
    this.ModalService.hideModal('EditCertificateModal');
  }

  submitEditSeedModal() {
    this.UserService.updateCertificate(this.selectedUser.acvpUserId, new AcvpUserCertificateUpdateParameters(this.editCertificateValue)).subscribe(
      data => { this.hideEditCertificateModal(); this.notifyParentComponent.emit(data); });
  }

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  @Input()
  set user(selectedUser: AcvpUser) {
    this.selectedUser = selectedUser;
    if (selectedUser) {
      this.editCertificateValue = selectedUser.certificateBase64;
    }
  }

  ngOnInit() {
  }

}
