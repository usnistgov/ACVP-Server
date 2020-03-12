import { Component, OnInit } from '@angular/core';
import { WorkflowItemLite } from '../../models/Workflow/WorkflowItemLite';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {

  workflowItems: WorkflowItemLite[];

  constructor(private ajs: AjaxService, private route: ActivatedRoute) { }

  ngOnInit() {
    
  }

}
