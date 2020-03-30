import { Component, OnInit, Input } from '@angular/core';
import { WorkflowDeletePayload } from '../../../../models/Workflow/WorkflowDeletePayload';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { Person } from '../../../../models/Person/Person';
import { Router } from '@angular/router';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { PersonProviderService } from '../../../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-workflow-person-delete',
  templateUrl: './workflow-person-delete.component.html',
  styleUrls: ['./workflow-person-delete.component.scss']
})
export class WorkflowPersonDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: Person;

  constructor(private PersonService: PersonProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

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
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the current state for comparison
    this.PersonService.getPerson(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
      },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
