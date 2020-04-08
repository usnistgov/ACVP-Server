import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { Router } from '@angular/router';
import { OperatingEnvironmentUpdatePayload } from '../../../../models/workflow/operatingEnvironment/OperatingEnvironmentUpdatePayload';
import { OperatingEnvironment } from '../../../../models/operatingEnvironment/OperatingEnvironment';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { OperatingEnvironmentProviderService } from '../../../../services/ajax/operatingEnvironment/operating-environment-provider.service';

@Component({
  selector: 'app-workflow-oe-update',
  templateUrl: './workflow-oe-update.component.html',
  styleUrls: ['./workflow-oe-update.component.scss']
})
export class WorkflowOeUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<OperatingEnvironmentUpdatePayload>;
  currentState: OperatingEnvironment;
  objectKeys = Object.keys;

  constructor(private OEService: OperatingEnvironmentProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

  shouldAttributeBeDisplayed(key: string) {
    if (key == "id" || key == "isInlineCreate") {
      return false;
    }
    return true;
  }

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
  set wfItem(item: WorkflowItemBase<OperatingEnvironmentUpdatePayload>) {
    this.workflowItem = item;

    // Loop through the dependencies listed
    for (let i = 0; i < this.workflowItem.payload.dependencies.length; i++) {

      // Check each one for whether it's existing (i.e. there's an additional API request reuqired to the API)
      // Frustratingly, the API still provide the name, type, and description fields on existing ones, but they're always
      // null, as a result of an issue with the API-side modeling.  Something to fix later on the server-side and then we can remove this block (TODO)
      if (this.workflowItem.payload.dependencies[i].isInlineCreate == false) {
        this.OEService.getDependency(this.workflowItem.payload.dependencies[i].id).subscribe(
          data => {
            this.workflowItem.payload.dependencies[i] = data;
            this.workflowItem.payload.dependencies[i].isInlineCreate = false;
            console.log(data);
          },
          err => { },
          () => { }
        );
      }
    }
    
    this.OEService.getOE(this.workflowItem.payload.id).subscribe(
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

