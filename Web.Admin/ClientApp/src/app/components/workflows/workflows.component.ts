import { Component, OnInit } from '@angular/core';
import { WorkflowItemLite } from '../../models/Workflow/WorkflowItemLite';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowStatus } from '../../models/Workflow/WorkflowStatus.enum';
import { WorkflowItemList } from '../../models/Workflow/WorkflowItemList';

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {

  workflowItems: WorkflowItemList;

  WorkflowStatusOptions = [ "All", "Pending", "Approved", "Incomplete", "Rejected" ];

  pageData = { "pageSize": 10, "pageNumber": 1, "totalPages" : 0};

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.workflowItems.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.workflowItems.currentPage > 1) {
        this.workflowItems.currentPage = --this.workflowItems.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.workflowItems.currentPage < this.workflowItems.totalPages) {
        this.workflowItems.currentPage = ++this.workflowItems.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.workflowItems.currentPage = this.workflowItems.totalPages;
    }

    this.ajs.getWorkflows(this.workflowItems.pageSize, this.workflowItems.currentPage).subscribe(
      data => {
        this.workflowItems = data;
        this.router.navigate([], {
          queryParams: { page: this.workflowItems.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

  ngOnInit() {

    this.workflowItems = new WorkflowItemList();
    this.workflowItems.pageSize = 10;
    this.workflowItems.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.workflowItems.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    this.ajs.getWorkflows(this.workflowItems.pageSize, this.workflowItems.currentPage).subscribe(
      data => { this.workflowItems = data; },
      err => { },
      () => { });
  }

}
