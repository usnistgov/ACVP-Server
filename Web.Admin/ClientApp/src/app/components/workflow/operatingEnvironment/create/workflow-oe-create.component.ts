import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { OperatingEnvironmentCreatePayload } from '../../../../models/workflow/operatingEnvironment/OperatingEnvironmentCreatePayload';
import { Dependency } from '../../../../models/dependency/Dependency';
import { DependencyDataProviderService } from '../../../../services/ajax/dependency/dependency-data-provider.service';

@Component({
  selector: 'app-workflow-oe-create',
  templateUrl: './workflow-oe-create.component.html',
  styleUrls: ['./workflow-oe-create.component.scss']
})
export class WorkflowOeCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<OperatingEnvironmentCreatePayload>;
  objectKeys = Object.keys;

  constructor(private dependencyService: DependencyDataProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<OperatingEnvironmentCreatePayload>) {
    this.workflowItem = item;

    this.workflowItem.payload.existingDependencies = [];
    for (let x = 0; x < this.workflowItem.payload.dependencyUrls.length; x++) {

      // Parse out the id from the provided url
      let dependencyId = parseInt(this.workflowItem.payload.dependencyUrls[x].split("/")[this.workflowItem.payload.dependencyUrls[x].split("/").length - 1]);

      // Now, GET the actual data
      this.dependencyService.getDependency(dependencyId).subscribe(
        data => { this.workflowItem.payload.existingDependencies.push(data); });
    }
  }

  ngOnInit() {
  }

}
