import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { Person } from '../../../../models/person/Person';
import { WorkflowPersonUpdatePayload } from '../../../../models/workflow/person/WorkflowPersonUpdatePayload';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';
import { PersonProviderService } from '../../../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-workflow-person-update',
  templateUrl: './workflow-person-update.component.html',
  styleUrls: ['./workflow-person-update.component.scss']
})
export class WorkflowPersonUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowPersonUpdatePayload>;
  currentState: Person;

  constructor(private PersonService: PersonProviderService, private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowPersonUpdatePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the data for the new organization to which the person will be associated
    // if they changing orgs
    if (this.workflowItem.payload.organizationURLUpdated) {

      let splitsArr = this.workflowItem.payload.vendorUrl.split('/');

      this.OrganizationService.getOrganization(parseInt(splitsArr[splitsArr.length-1])).subscribe(
        data => {
          this.workflowItem.payload.organization = data;
        },
        err => { },
        () => { }
      );
    }

    // Go get the current state for comparison
    this.PersonService.getPerson(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
        console.log(data);
      },
      err => { },
      () => { }
    );

  }
  ngOnInit() {
  }

}
