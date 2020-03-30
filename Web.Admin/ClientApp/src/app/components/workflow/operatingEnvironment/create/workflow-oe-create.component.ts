import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { OperatingEnvironmentCreatePayload } from '../../../../models/Workflow/OperatingEnvironment/OperatingEnvironmentCreatePayload';
import { Router } from '@angular/router';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';

@Component({
  selector: 'app-workflow-oe-create',
  templateUrl: './workflow-oe-create.component.html',
  styleUrls: ['./workflow-oe-create.component.scss']
})
export class WorkflowOeCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<OperatingEnvironmentCreatePayload>;
  objectKeys = Object.keys;

  constructor(private workflowService: WorkflowProviderService, private router: Router) { }

  approveWorkflow() {
    this.workflowService.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.workflowService.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
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

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<OperatingEnvironmentCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
