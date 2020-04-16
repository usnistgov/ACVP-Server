import { Component, OnInit, Input } from '@angular/core';
import { WorkflowDeletePayload } from '../../../../models/workflow/WorkflowDeletePayload';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { Person } from '../../../../models/person/Person';
import { PersonProviderService } from '../../../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-workflow-person-delete',
  templateUrl: './workflow-person-delete.component.html',
  styleUrls: ['./workflow-person-delete.component.scss']
})
export class WorkflowPersonDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: Person;

  constructor(private PersonService: PersonProviderService) { }

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
