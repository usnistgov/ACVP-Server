import { Component, OnInit } from '@angular/core';
import { OperatingEnvironment } from '../../models/operatingEnvironment/operatingEnvironment';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { Dependency } from '../../models/dependency/dependency';
import { Attribute } from '../../models/dependency/attribute';

@Component({
  selector: 'app-validation-db-oe',
  templateUrl: './validation-db-oe.component.html',
  styleUrls: ['./validation-db-oe.component.scss']
})
export class ValidationDbOeComponent implements OnInit {

  selectedOE: OperatingEnvironment;
  referenceCopyOE: OperatingEnvironment;
  newDependency = new Dependency(0, "", "", "", null);
  updateStatusFlag = "none";

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getOE(parseInt(params.get("id"))).subscribe(
        data => { this.selectedOE = data; this.referenceCopyOE = data; },
        err => { },
        () => { })
    });
  }

}
