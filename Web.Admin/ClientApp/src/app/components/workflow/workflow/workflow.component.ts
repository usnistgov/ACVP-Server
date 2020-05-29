import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkflowItemBase } from '../../../models/workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../models/workflow/IWorkflowItemPayload';
import { WorkflowProviderService } from '../../../services/ajax/workflow/workflow-provider.service';
import { ModalService } from '../../../services/modal/modal.service';

@Component({
  selector: 'app-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['./workflow.component.scss']
})
export class WorkflowComponent implements OnInit {

  RawStringFormatted: string;

  workflowItem: WorkflowItemBase<IWorkflowItemPayload>;

  constructor(private workflowService: WorkflowProviderService, private route: ActivatedRoute, private modalService: ModalService) { }

  showRawModal() {
    this.modalService.showModal('RawModal');
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.workflowService.getWorkflow(parseInt(params.get("id"))).subscribe(
        data => { this.workflowItem = data; this.RawStringFormatted = JSON.stringify(this.workflowItem, null, 4); },
        err => { },
        () => { })
    });
  }

}
