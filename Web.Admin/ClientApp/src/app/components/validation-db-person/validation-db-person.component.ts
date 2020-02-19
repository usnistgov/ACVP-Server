import { Component, OnInit } from '@angular/core';
import { Person } from '../../models/Person/Person';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { PersonPhone } from '../../models/Person/PersonPhone';

@Component({
  selector: 'app-validation-db-person',
  templateUrl: './validation-db-person.component.html',
  styleUrls: ['./validation-db-person.component.scss']
})
export class ValidationDbPersonComponent implements OnInit {

  selectedPerson: Person;
  referenceCopyPerson: Person;

  // Plceholder object to bind the fields for the add-new-phone input boxes
  newPhoneData = new PersonPhone();
  newPhoneFlag = false;

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  addNewPhone() {
    this.newPhoneFlag = true;
  }

  submitNewPhone() {
   
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getPerson(parseInt(params.get("id"))).subscribe(
        data => { this.selectedPerson = JSON.parse(JSON.stringify(data)); this.referenceCopyPerson = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
