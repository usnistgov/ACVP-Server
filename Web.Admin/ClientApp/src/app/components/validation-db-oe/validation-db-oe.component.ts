import { Component, OnInit } from '@angular/core';
import { OperatingEnvironment } from '../../models/operatingEnvironment/OperatingEnvironment';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { Dependency } from '../../models/dependency/Dependency';
import { Attribute } from '../../models/dependency/Attribute';

import { OperatingEnvironmentProviderService } from '../../services/ajax/operatingEnvironment/operating-environment-provider.service';
import { DependencyList } from 'src/app/models/dependency/Dependency-list';

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

  noDependencyProvidedFlag = false;
  noTypeProvidedFlag = false;
  noDescriptionProvidedFlag = false;

  dependencyListPageData = { resultsPerPage : 10, pageNumber : 1 };

  constructor(private oeps: OperatingEnvironmentProviderService, private route: ActivatedRoute, private modalService: ModalService) { }

  addDependency(dependencyId: number) {
    this.oeps.addDependencyToOE(dependencyId, this.selectedOE.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { })
  }

  createAndAddDependency() {

    this.noDependencyProvidedFlag = false;
    this.noTypeProvidedFlag = false;
    this.noDescriptionProvidedFlag = false;

    if (this.newDependency.name === "" || this.newDependency.name === null) {
      this.noDependencyProvidedFlag = true;
    }
    if (this.newDependency.type === "" || this.newDependency.type === null) {
      this.noTypeProvidedFlag = true;
    }
    if (this.newDependency.description === "" || this.newDependency.description === null) {
      this.noDescriptionProvidedFlag = true;
    }
    if (!this.noDependencyProvidedFlag && !this.noTypeProvidedFlag && !this.noDescriptionProvidedFlag) {
      this.oeps.createDependency(this.newDependency).subscribe(
        data => { this.addDependency(data.id); this.newDependency = new Dependency(0, "", "", "", new Array<Attribute>()); },
        err => { },
        () => { });
    }
  }

  deleteDependency(dependencyId:number) {
    this.oeps.removeDependencyFromOE(dependencyId, this.selectedOE.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { })
  }

  refreshPageData() {
    this.oeps.getOE(this.selectedOE.id).subscribe(
      data => { this.selectedOE = JSON.parse(JSON.stringify(data)); this.referenceCopyOE = JSON.parse(JSON.stringify(data)); },
      err => { },
      () => { });
  }

  updateOE() {
    this.oeps.updateOE(this.referenceCopyOE).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  editDependencies() {

    this.oeps.getDependencies(this.dependencyListPageData.resultsPerPage, this.dependencyListPageData.pageNumber).subscribe(
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

    this.oeps.getDependencies(this.dependencyListPageData.resultsPerPage, this.dependencyListPageData.pageNumber).subscribe(
      data => { this.availableDependencies = data; },
      err => { },
      () => { })
}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.oeps.getOE(parseInt(params.get("id"))).subscribe(
        data => { this.selectedOE = JSON.parse(JSON.stringify(data)); this.referenceCopyOE = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
