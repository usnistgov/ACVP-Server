import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { Dependency } from '../../models/dependency/dependency';
import { Attribute } from '../../models/dependency/attribute';
import { ModalService } from '../../services/modal/modal.service';

@Component({
  selector: 'app-validation-db-dependency',
  templateUrl: './validation-db-dependency.component.html',
  styleUrls: ['./validation-db-dependency.component.scss']
})
export class ValidationDbDependencyComponent implements OnInit {

  selectedDependency: Dependency;
  referenceCopyDependency: Dependency;
  newAttribute = new Attribute("", "");

  updateStatusFlag = "none";

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  addAttribute(name: string, value: string) {
    this.ajs.addAttribute(this.selectedDependency.id, new Attribute(name, value)).subscribe(
      data => { this.refreshPageData(); this.newAttribute.name = ""; this.newAttribute.value = "";},
      err => { console.log("Attribute addition failed"); },
      () => {});
  }

  deleteAttribute(attributeId: number) {
    this.ajs.deleteAttribute(this.selectedDependency.id, attributeId).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  // Used to submit changes to the values on the lefthand side of the screen
  // submit values for fields you want updated and null for those you don't.
  // Available fields: name, type, description
  updateDependency() {
    this.updateStatusFlag = "processing";
    this.ajs.updateDependency(this.referenceCopyDependency).subscribe(
      data => { this.updateStatusFlag = "successful"; this.refreshPageData(); },
      err => { },
      () => { });
  }

  // Used to refresh the page after a dependency is added, or metadata is altered, etc.
  refreshPageData() {
    this.ajs.getDependency(this.selectedDependency.id).subscribe(
      data => { this.selectedDependency = data; this.referenceCopyDependency = data; },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getDependency(parseInt(params.get("id"))).subscribe(
        data => { this.selectedDependency = data; this.referenceCopyDependency = data; },
        err  => { },
        ()   => { })
    });
  }

}