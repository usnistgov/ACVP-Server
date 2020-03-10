import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { WorkflowItemBase } from '../../../models/Workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../models/Workflow/IWorkflowItemPayload';

@Component({
  selector: 'app-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['./workflow.component.scss']
})
export class WorkflowComponent implements OnInit {

  workflowItem: WorkflowItemBase<IWorkflowItemPayload>;

  constructor(private ajs: AjaxService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getWorkflow(parseInt(params.get("id"))).subscribe(
        data => { this.workflowItem = data; console.log(this.workflowItem); },
        err => { },
        () => { })
    });
  }

}
