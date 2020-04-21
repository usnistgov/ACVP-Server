import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { ModalService } from '../../../services/modal/modal.service';
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { Result } from '../../../models/responses/Result';
import { AcvpUserCertificateUpdateParameters } from '../../../models/AcvpUser/AcvpUserCertificateUpdateParameters';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-acvp-user-certificate',
  templateUrl: './acvp-user-certificate.component.html',
  styleUrls: ['./acvp-user-certificate.component.scss']
})
export class AcvpUserCertificateComponent implements OnInit {
  form: FormGroup;
  selectedUser: AcvpUser;
  editCertificateValue: string = '';

  constructor(private UserService: AcvpUserDataProviderService, private ModalService: ModalService) { }

  @Input()
  set user(user: AcvpUser) {
    this.selectedUser = user;
  }

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  raiseEditCertificateModal() {
    this.ModalService.showModal('EditCertificateModal');
  }

  hideEditCertificateModal() {
    this.ModalService.hideModal('EditCertificateModal');
  }

  uploadCertificate = async (file) => {

    // Create a FileReader instance
    let reader = new FileReader();

    // Alias "this" so that it can be used in the callback defined next
    var self = this;

    // Set the onLoad callback.  This runs when the readDataAsURL completes (it's async)
    reader.onload = function () {

      // Parse the result and split it to get only the Base64 portion
      let param = new AcvpUserCertificateUpdateParameters(reader.result.toString().split(',')[1]);

      self.UserService.updateCertificate(self.selectedUser.acvpUserId, param)
        .subscribe(data => {

          // This hides the modal that is part of this component
          self.hideEditCertificateModal();

          // This notifies the parent, so that the parent can know to refresh itself
          self.notifyParentComponent.emit(data);
        }
      );
    };

    // Now, execute readDataAsURL, which will eventually fire the callback
    reader.readAsDataURL(file);
  }

  ngOnInit() {
  }

}
