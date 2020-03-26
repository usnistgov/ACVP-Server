import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkflowItemBase } from '../../../models/Workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../models/Workflow/IWorkflowItemPayload';
import { WorkflowProviderService } from '../../../services/ajax/workflow/workflow-provider.service';

@Component({
  selector: 'app-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['./workflow.component.scss']
})
export class WorkflowComponent implements OnInit {

  workflowItem: WorkflowItemBase<IWorkflowItemPayload>;

  constructor(private workflowService: WorkflowProviderService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.workflowService.getWorkflow(parseInt(params.get("id"))).subscribe(
        data => { this.workflowItem = data; console.log(this.workflowItem); },
        err => { },
        () => { })
    });
  }

}
