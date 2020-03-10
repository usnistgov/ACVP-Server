import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowOrganizationCreatePayload } from '../../../../models/Workflow/Organization/WorkflowOrganizationCreatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-organization-create',
  templateUrl: './workflow-organization-create.component.html',
  styleUrls: ['./workflow-organization-create.component.scss']
})
export class WorkflowOrganizationCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowOrganizationCreatePayload>;

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

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowOrganizationCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
