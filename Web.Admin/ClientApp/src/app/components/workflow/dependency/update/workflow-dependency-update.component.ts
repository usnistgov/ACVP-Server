import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDependencyUpdatePayload } from '../../../../models/Workflow/Dependency/WorkflowDependencyUpdatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Dependency } from '../../../../models/dependency/dependency';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-dependency-update',
  templateUrl: './workflow-dependency-update.component.html',
  styleUrls: ['./workflow-dependency-update.component.scss']
})
export class WorkflowDependencyUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDependencyUpdatePayload>;
  currentState: Dependency;
  objectKeys = Object.keys;

  constructor(private ajs: AjaxService, private router: Router) { }

  approveWorkflow() {
    this.ajs.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.ajs.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }

  refreshPageData() {
    this.router.navigateByUrl('/', { skipLocationChange: true })
      .then(() =>
        this.router.navigate(['workflow/' + this.workflowItem.workflowItemID])
      );
  }

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

    this.ajs.getDependency(this.workflowItem.payload.id).subscribe(
      data => { this.currentState = data; console.log(this.currentState); },
      err => { },
      () => { }
    );
  }

  ngOnInit() {
  }

}
