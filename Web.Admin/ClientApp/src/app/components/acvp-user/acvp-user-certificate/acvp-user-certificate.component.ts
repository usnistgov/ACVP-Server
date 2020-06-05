import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { ModalService } from '../../../services/modal/modal.service';
import { AcvpUserDataProviderService } from '../../../services/ajax/acvp-user/acvp-user-data-provider.service';
import { Result } from '../../../models/responses/Result';
import { AcvpUserCertificateUpdateParameters } from '../../../models/AcvpUser/AcvpUserCertificateUpdateParameters';
import { FormGroup } from '@angular/forms';
import { PersonProviderService } from '../../../services/ajax/person/person-provider.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-acvp-user-certificate',
  templateUrl: './acvp-user-certificate.component.html',
  styleUrls: ['./acvp-user-certificate.component.scss']
})
export class AcvpUserCertificateComponent implements OnInit {
  form: FormGroup;
  selectedUser: AcvpUser;
  editCertificateValue: string = '';

  // Some basic objects to use for showing and calculating elapsed certs
  todayDate = Date.now();
  dateMachine = new Date(Date.now());
  twoMonthsInFutureDate = this.dateMachine.setDate(this.dateMachine.getDate() + 60);

  emailBody: string;

  constructor(private UserService: AcvpUserDataProviderService,
    private ModalService: ModalService,
    private PersonService: PersonProviderService,
    private location: Location) { }

  @Input()
  set user(user: AcvpUser) {
    if (typeof user !== "undefined") {
      this.selectedUser = user;
      this.PersonService.getPerson(user.personId).subscribe(
        data => {
          this.selectedUser.person = data;
          this.generateEmailBody();
        }
      );
    }
  }

  generateEmailBody() {

    // Assemble the emails to send to
    var emailAddresses = "";
    console.log(this.selectedUser.person.emailAddresses);
    for (let i = 0; i < this.selectedUser.person.emailAddresses.length; i++) {
      emailAddresses = emailAddresses + this.selectedUser.person.emailAddresses[i] + ";";
    }

    var environment = "";
    var envEmailContact = "";

    // Assemble the environment name
    if (window.location.href.includes('admin.acvts.nist.gov')) {
      environment = "Production";
      envEmailContact = "acvts-prod@nist.gov";
    } else if (window.location.href.includes('admin.demo.acvts.nist.gov')) {
      environment = "Demo";
      envEmailContact = "acvts-demo@nist.gov";
    } else if (window.location.href.includes('admin.test.acvts.nist.gov')) {
      environment = "Test";
      envEmailContact = "acvts-test@nist.gov";
    } else if (window.location.href.includes('admin.dev.acvts.nist.gov')) {
      environment = "Development";
      envEmailContact = "acvts-dev@nist.gov";
    } else if (window.location.href.includes('localhost') || window.location.href.includes('127.0.0.1')) {
      environment = "Local";
      envEmailContact = "acvts-local@nist.gov";
    }

    this.emailBody = "" +
      "mailto:" + emailAddresses +
      "&subject=ACVTS " + environment + " Environment Credential Expiration Reminder" +
      "&body=" + this.selectedUser.fullName + ",\r\n\r\n" +

      "This is a reminder that your credential to the ACVTS " + environment +
      " Environment is at or near expiration.\r\n\r\n" +

      "The expiration date is: " + this.selectedUser.expiresOn + "\r\n\r\n" +

      "Please contact " + envEmailContact + " for instructions on submitting a new CSR" +
      " if you are interested in renewing your " + environment + " certificate. \r\n\r\n" +

      "If we don’t hear back from you within 2 weeks of this email notification, we will " +
      "remove your existing cert from the " + environment + " environment either by the date " +
      "the certificate expires or two weeks from the date of this email notification in the " +
      "event your certificate has already expired. If you no longer need access to the Demo environment," +
      "please reply back and let us know that as well.\r\n\r\n" +
      "Thank you,\r\nACVTS Administration Team";

    this.emailBody = encodeURI(this.emailBody);
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
