import { Component, OnInit } from '@angular/core';
import { Person } from '../../models/person/Person';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { PersonPhone } from '../../models/person/PersonPhone';
import { PersonProviderService } from '../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-validation-db-person',
  templateUrl: './validation-db-person.component.html',
  styleUrls: ['./validation-db-person.component.scss']
})
export class ValidationDbPersonComponent implements OnInit {

  selectedPerson: Person;
  referenceCopyPerson: Person;

  // Placeholder string for the new email box
  newEmailData: string;

  // Placeholder object to bind the fields for the add-new-phone input boxes
  newPhoneData = new PersonPhone();
  newPhoneFlag = false;

  // Placeholder object to bind the fields for the add-new-note input boxes
  //newNoteData = new PersonNote();
  //newNoteFlag = false;

  constructor(private PersonService: PersonProviderService, private route: ActivatedRoute, private modalService: ModalService) { }

  submitNewPhone() {

    // Create the (empty) person object for sending across as the post body
    var patchBody = new Person();
    patchBody.id = this.selectedPerson.id;

    // Store the existing phone numbers (since we're just usingt he "update person" function of the API)
    patchBody.phoneNumbers = this.referenceCopyPerson.phoneNumbers;

    // But don't forget to include the new one on the patch-ed body
    patchBody.phoneNumbers.push(this.newPhoneData);

    // And final, submit it
    this.PersonService.updatePerson(patchBody).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });;
  }

  deletePhone(index: number) {
    // Create the (empty) person object for sending across as the post body
    var patchBody = new Person();
    patchBody.id = this.selectedPerson.id;

    // Store the existing phone numbers (since we're just using the "update person" function of the API)
    patchBody.phoneNumbers = this.referenceCopyPerson.phoneNumbers;

    // And remove the relevant one (this is perhaps a bit wordy, but hopefully is readable)
    patchBody.phoneNumbers.splice(index, 1);

    this.PersonService.updatePerson(patchBody).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  submitNewEmail() {

    // See submitPhone or Delete phone for documentation
    var patchBody = new Person();
    patchBody.id = this.selectedPerson.id;

    patchBody.emailAddresses = this.referenceCopyPerson.emailAddresses;

    patchBody.emailAddresses.push(this.newEmailData);
    console.log('submitting data now.');
    this.PersonService.updatePerson(patchBody).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  deleteEmail(index: number) {
    // Create the (empty) person object for sending across as the post body
    var patchBody = new Person();
    patchBody.id = this.selectedPerson.id;

    // Store the existing phone numbers (since we're just using the "update person" function of the API)
    patchBody.emailAddresses = this.referenceCopyPerson.emailAddresses;

    // And remove the relevant one (this is perhaps a bit wordy, but hopefully is readable)
    patchBody.emailAddresses.splice(index, 1);

    this.PersonService.updatePerson(patchBody).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  //submitNewNote() {

  //  // Create the (empty) person object for sending across as the post body
  //  var patchBody = new Person();
  //  patchBody.id = this.selectedPerson.id;

  //  // Store the existing phone numbers (since we're just usingt he "update person" function of the API)
  //  patchBody.notes = this.referenceCopyPerson.notes;

  //  // But don't forget to include the new one on the patch-ed body
  //  patchBody.notes.push(this.newNoteData);

  //  // And final, submit it
  //  this.ajs.updatePerson(patchBody).subscribe(
  //    data => { this.refreshPageData(); },
  //    err => { },
  //    () => { });;
  //}

  updateName() {
    // Create the (empty) person object for sending across as the post body
    var patchBody = new Person();
    patchBody.id = this.selectedPerson.id;

    patchBody.name = this.referenceCopyPerson.name;

    this.PersonService.updatePerson(patchBody).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  refreshPageData() {
    this.PersonService.getPerson(this.selectedPerson.id).subscribe(
      data => {
        this.selectedPerson = JSON.parse(JSON.stringify(data));
        this.referenceCopyPerson = JSON.parse(JSON.stringify(data));

        //this.newNoteFlag = false;
        //this.newNoteData = new PersonNote();

        this.newPhoneFlag = false;
        this.newPhoneData = new PersonPhone();

        this.newEmailData = "";
      },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.PersonService.getPerson(parseInt(params.get("id"))).subscribe(
        data => { this.selectedPerson = JSON.parse(JSON.stringify(data)); this.referenceCopyPerson = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
