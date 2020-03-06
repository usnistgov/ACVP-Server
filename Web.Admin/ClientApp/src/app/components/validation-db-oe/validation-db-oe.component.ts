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
  newDependency = new Dependency(0, "", "", "", new Array<Attribute>());
  updateStatusFlag = "none";

  dependencyListPageData = { resultsPerPage : 10, pageNumber : 1 };

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  addDependency(dependencyId: number) {
    this.ajs.addDependencyToOE(dependencyId, this.selectedOE.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { })
  }

  createAndAddDependency() {
    this.ajs.createDependency(this.newDependency).subscribe(
      data => { this.addDependency(data.id); this.newDependency = new Dependency(0, "", "", "", new Array<Attribute>()); },
      err => { },
      () => { });
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

    this.ajs.getDependencies(this.dependencyListPageData.resultsPerPage, this.dependencyListPageData.pageNumber).subscribe(
      data => { this.availableDependencies = data; },
      err => { },
      () => { })
    this.modalService.showModal('editDependenciesModal');
  }

  pageChange(whichButton: string) {

    if (whichButton === "first") {
      this.dependencyListPageData.pageNumber = 1;
    } else if (whichButton === "previous" && this.dependencyListPageData.pageNumber >= 1) {
      this.dependencyListPageData.pageNumber = this.dependencyListPageData.pageNumber - 1;
    } else if (whichButton = "next") {
      this.dependencyListPageData.pageNumber = this.dependencyListPageData.pageNumber + 1;
    }

    this.ajs.getDependencies(this.dependencyListPageData.resultsPerPage, this.dependencyListPageData.pageNumber).subscribe(
      data => { this.availableDependencies = data; },
      err => { },
      () => { })
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
