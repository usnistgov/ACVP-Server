import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowPersonCreatePayload } from '../../../../models/workflow/person/WorkflowPersonCreatePayload';
import { Organization } from '../../../../models/organization/Organization';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';

@Component({
  selector: 'app-workflow-person-create',
  templateUrl: './workflow-person-create.component.html',
  styleUrls: ['./workflow-person-create.component.scss']
})
export class WorkflowPersonCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowPersonCreatePayload>;
  organization: Organization;

  constructor(private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowPersonCreatePayload>) {
    this.workflowItem = item;

    // Get the organization data in order to populate the organization field
    const splitString = this.workflowItem.payload.vendorUrl.split('/');
    this.OrganizationService.getOrganization(parseInt(splitString[splitString.length - 1])).subscribe(
      data => { this.organization = data; },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
