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

  onCertificateUploaded(file: string) {
    console.log(file.split(",")[1]);
  }

  raiseEditCertificateModal() {
    this.ModalService.showModal('EditCertificateModal');
  }

  hideEditCertificateModal() {
    this.ModalService.hideModal('EditCertificateModal');
  }

  onChangeHandler($event) {
    console.log($event.target.files[0]); // outputs the first file
  }

  submitEditCertificateModal(event) {

    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.get('avatar').setValue({
          filename: file.name,
          filetype: file.type,
          //value: reader.result.split(',')[1]
        })
      };
    }

    //console.log("1");
    //const toBase64 = file => new Promise((resolve, reject) => {
    //  const reader = new FileReader();
    //  reader.readAsDataURL(file);
    //  reader.onload = () => resolve(reader.result);
    //  reader.onerror = error => reject(error);
    //});
    //console.log("2");
    //const file = document.querySelector('#myfile').attributes;
    //const result = await toBase64(file).catch(e => Error(e));
    //if (result instanceof Error) {
    //  console.log('Error: ', result.message);
    //  return;
    //}
    //console.log("3");
    //this.UserService.updateCertificate(this.selectedUser.acvpUserId, new AcvpUserCertificateUpdateParameters(this.editCertificateValue)).subscribe(
    //  data => { console.log("4");this.hideEditCertificateModal(); this.notifyParentComponent.emit(data); });
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
