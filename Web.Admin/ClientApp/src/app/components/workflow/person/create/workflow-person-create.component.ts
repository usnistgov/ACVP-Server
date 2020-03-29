import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowPersonCreatePayload } from '../../../../models/Workflow/Person/WorkflowPersonCreatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';
import { Organization } from '../../../../models/Organization/Organization';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';

@Component({
  selector: 'app-workflow-person-create',
  templateUrl: './workflow-person-create.component.html',
  styleUrls: ['./workflow-person-create.component.scss']
})
export class WorkflowPersonCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowPersonCreatePayload>;
  organization: Organization;

  constructor(private ajs: AjaxService, private OrganizationService: OrganizationProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

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
  set wfItem(item: WorkflowItemBase<WorkflowPersonCreatePayload>) {
    this.workflowItem = item;

    // Get the organization data in order to populate the organization field
    const splitString = this.workflowItem.payload.organizationUrl.split('/');
    this.OrganizationService.getOrganization(parseInt(splitString[splitString.length - 1])).subscribe(
      data => { this.organization = data; },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
