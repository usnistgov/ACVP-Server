import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { Person } from '../../../../models/Person/Person';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';
import { WorkflowPersonUpdatePayload } from '../../../../models/Workflow/Person/WorkflowPersonUpdatePayload';

@Component({
  selector: 'app-workflow-person-update',
  templateUrl: './workflow-person-update.component.html',
  styleUrls: ['./workflow-person-update.component.scss']
})
export class WorkflowPersonUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowPersonUpdatePayload>;
  currentState: Person;

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
  set wfItem(item: WorkflowItemBase<WorkflowPersonUpdatePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the data for the new organization to which the person will be associated
    // if they changing orgs
    if (this.workflowItem.payload.organizationURLUpdated) {

      let splitsArr = this.workflowItem.payload.organizationUrl.split('/');

      this.ajs.getOrganization(parseInt(splitsArr[splitsArr.length-1])).subscribe(
        data => {
          this.workflowItem.payload.organization = data;
        },
        err => { },
        () => { }
      );
    }

    // Go get the current state for comparison
    this.ajs.getPerson(this.workflowItem.payload.id).subscribe(
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
