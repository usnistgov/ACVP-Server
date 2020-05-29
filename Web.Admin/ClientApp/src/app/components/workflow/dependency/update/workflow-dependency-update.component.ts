import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowDependencyUpdatePayload } from '../../../../models/workflow/dependency/WorkflowDependencyUpdatePayload';
import { Dependency } from '../../../../models/dependency/Dependency';
import { DependencyDataProviderService } from '../../../../services/ajax/dependency/dependency-data-provider.service';

@Component({
  selector: 'app-workflow-dependency-update',
  templateUrl: './workflow-dependency-update.component.html',
  styleUrls: ['./workflow-dependency-update.component.scss']
})
export class WorkflowDependencyUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDependencyUpdatePayload>;
  currentState: Dependency;
  objectKeys = Object.keys;

  constructor(private DependencyDataService: DependencyDataProviderService) { }

  isUserDefinedAttribute(key: string) {
    return (key != 'id' && key != 'name' && key != 'type' && key != 'description' && key != 'url' &&
      key != 'typeUpdated' && key != 'nameUpdated' && key != 'descriptionUpdated' && key != 'attributesUpdated');
  }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDependencyUpdatePayload>) {
    this.workflowItem = item;

    this.DependencyDataService.getDependency(this.workflowItem.payload.id).subscribe(
      data => { this.currentState = data; console.log(this.currentState); },
      err => { },
      () => { }
    );
  }

  ngOnInit() {
  }

}
