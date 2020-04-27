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

    for (let x = 0; x < this.workflowItem.payload.dependencies.length; x++) {
      // If it's an existing dependency
      if (this.workflowItem.payload.dependencies[x].id !== -1) {
        this.dependencyService.getDependency(this.workflowItem.payload.dependencies[x].id).subscribe(
          data => { this.workflowItem.payload.dependencies[x] = data; });
      }
    }
  }

  ngOnInit() {
  }

}
