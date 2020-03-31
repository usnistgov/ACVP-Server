import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDependencyCreatePayload } from '../../../../models/Workflow/Dependency/WorkflowDependencyCreatePayload';
import { Router } from '@angular/router';
import { DependencyDataProviderService } from '../../../../services/ajax/dependency/dependency-data-provider.service';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';

@Component({
  selector: 'app-workflow-dependency-create',
  templateUrl: './workflow-dependency-create.component.html',
  styleUrls: ['./workflow-dependency-create.component.scss']
})
export class WorkflowDependencyCreateComponent implements OnInit {

  constructor(private DependencyDataService: DependencyDataProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

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

  workflowItem: WorkflowItemBase<WorkflowDependencyCreatePayload>;
  objectKeys = Object.keys;

  /*
   * This is how the component takes the workflowItem from the main workflow controller using the
   * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
   */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDependencyCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
