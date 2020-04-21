import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/workflow/WorkflowDeletePayload';
import { Dependency } from '../../../../models/dependency/Dependency';
import { DependencyDataProviderService } from '../../../../services/ajax/dependency/dependency-data-provider.service';

@Component({
  selector: 'app-workflow-dependency-delete',
  templateUrl: './workflow-dependency-delete.component.html',
  styleUrls: ['./workflow-dependency-delete.component.scss']
})
export class WorkflowDependencyDeleteComponent implements OnInit {

  currentState: Dependency;
  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  objectKeys = Object.keys;

  constructor(private DependencyDataService: DependencyDataProviderService) { }

  /*
   * This is how the component takes the workflowItem from the main workflow controller using the
   * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
   */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {
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
