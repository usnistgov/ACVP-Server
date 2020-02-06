import { Component, OnInit } from '@angular/core';
import { OperatingEnvironment } from '../../models/operatingEnvironment/operatingEnvironment';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { Dependency } from '../../models/dependency/dependency';
import { Attribute } from '../../models/dependency/attribute';
import { DependencyList } from '../../models/dependency/dependency-list';

@Component({
  selector: 'app-validation-db-oe',
  templateUrl: './validation-db-oe.component.html',
  styleUrls: ['./validation-db-oe.component.scss']
})
export class ValidationDbOeComponent implements OnInit {

  selectedOE: OperatingEnvironment;
  referenceCopyOE: OperatingEnvironment;
  availableDependencies: DependencyList;
  newDependency = new Dependency(0, "", "", "", null);
  updateStatusFlag = "none";

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  addDependency(dependencyId: number) {
    this.ajs.addDependencyToOE(dependencyId, this.selectedOE.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { })
  }

  deleteDependency(dependencyId:number) {
    this.ajs.removeDependencyFromOE(dependencyId, this.selectedOE.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { })
  }

  refreshPageData() {
    this.ajs.getOE(this.selectedOE.id).subscribe(
      data => { this.selectedOE = JSON.parse(JSON.stringify(data)); this.referenceCopyOE = JSON.parse(JSON.stringify(data)); },
      err => { },
      () => { });
  }

  updateOE() {
    this.ajs.updateOE(this.referenceCopyOE).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  editDependencies() {

    this.ajs.getDependencies(10, 1).subscribe(
      data => { this.availableDependencies = data; },
      err => { },
      () => { })
    this.modalService.showModal('editDependenciesModal');
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getOE(parseInt(params.get("id"))).subscribe(
        data => { this.selectedOE = JSON.parse(JSON.stringify(data)); this.referenceCopyOE = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
