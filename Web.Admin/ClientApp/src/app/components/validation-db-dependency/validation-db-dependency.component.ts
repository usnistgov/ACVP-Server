import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Dependency } from '../../models/dependency/Dependency';
import { DependencyDataProviderService } from '../../services/ajax/dependency/dependency-data-provider.service';
import { Attribute } from '../../models/dependency/Attribute';

@Component({
  selector: 'app-validation-db-dependency',
  templateUrl: './validation-db-dependency.component.html',
  styleUrls: ['./validation-db-dependency.component.scss']
})
export class ValidationDbDependencyComponent implements OnInit {

  selectedDependency: Dependency;
  referenceCopyDependency: Dependency;
  newAttribute = new Attribute("", "");

  noNameProvidedFlag = false;
  noValueProvidedFlag = false;

  updateStatusFlag = "none";

  constructor(private dds: DependencyDataProviderService, private route: ActivatedRoute) { }

  addAttribute(name: string, value: string) {

    this.noNameProvidedFlag = false;
    this.noValueProvidedFlag = false;

    if (name == "" || name == null) {
      this.noNameProvidedFlag = true;
    }
    if (value == "" || name == null) {
      this.noValueProvidedFlag = true;
    }
    if (!this.noNameProvidedFlag && !this.noValueProvidedFlag) {
      this.dds.addAttribute(this.selectedDependency.id, new Attribute(name, value)).subscribe(
        data => { this.refreshPageData(); this.newAttribute.name = ""; this.newAttribute.value = ""; },
        err => { console.log("Attribute addition failed"); },
        () => { });
    }
  }

  deleteAttribute(attributeId: number) {
    this.dds.deleteAttribute(this.selectedDependency.id, attributeId).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  // Used to submit changes to the values on the lefthand side of the screen
  // submit values for fields you want updated and null for those you don't.
  // Available fields: name, type, description
  updateDependency() {
    this.updateStatusFlag = "processing";
    this.dds.updateDependency(this.referenceCopyDependency).subscribe(
      data => { this.updateStatusFlag = "successful"; this.refreshPageData(); },
      err => { },
      () => { });
  }

  // Used to refresh the page after a dependency is added, or metadata is altered, etc.
  refreshPageData() {
    this.dds.getDependency(this.selectedDependency.id).subscribe(
      data => { this.selectedDependency = data; this.referenceCopyDependency = data; },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.dds.getDependency(parseInt(params.get("id"))).subscribe(
        data => { this.selectedDependency = data; this.referenceCopyDependency = data; },
        err  => { },
        ()   => { })
    });
  }

}
