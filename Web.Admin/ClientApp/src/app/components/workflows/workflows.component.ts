import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowItemList } from '../../models/Workflow/WorkflowItemList';
import { APIAction } from '../../models/Workflow/APIAction.enum';

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {

  workflowItems: WorkflowItemList;
  requestId: string;
  DBID: string;
  APIAction = APIAction;
  selectedAPIAction: string;
  

  WorkflowStatusOptions = [ "All", "Pending", "Approved", "Incomplete", "Rejected" ];

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

    this.ajs.getWorkflows(this.workflowItems.pageSize, this.workflowItems.currentPage, this.requestId, this.selectedAPIAction, this.DBID).subscribe(
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

  setAPIAction(input: string) {
    this.selectedAPIAction = input;
    this.loadData();
  }

  loadData() {

    if (this.requestId !== "") {
      this.router.navigate([], {
        queryParams: { requestId: this.requestId },
        queryParamsHandling: 'merge'
      });
    }
    else {
      this.router.navigate([], {
        queryParams: { requestId: null },
        queryParamsHandling: 'merge'
      });
    }

    if (this.DBID !== "") {
      this.router.navigate([], {
        queryParams: { DBID: this.DBID },
        queryParamsHandling: 'merge'
      });
    }
    else {
      this.router.navigate([], {
        queryParams: { DBID: null },
        queryParamsHandling: 'merge'
      });
    }

    if (this.selectedAPIAction !== "All") {
      this.router.navigate([], {
        queryParams: { selectedAPIAction: this.selectedAPIAction },
        queryParamsHandling: 'merge'
      });
    }
    else {
      this.router.navigate([], {
        queryParams: { selectedAPIAction: null },
        queryParamsHandling: 'merge'
      });
    }

    if (this.selectedAPIAction === "All") {
      this.ajs.getWorkflows(this.workflowItems.pageSize, this.workflowItems.currentPage, this.requestId, null, this.DBID).subscribe(
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
    }
    else {
      this.ajs.getWorkflows(this.workflowItems.pageSize, this.workflowItems.currentPage, this.requestId, this.selectedAPIAction, this.DBID).subscribe(
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
    }

  }

  ngOnInit() {

    this.workflowItems = new WorkflowItemList();
    this.workflowItems.pageSize = 10;
    this.workflowItems.currentPage = 1;

    // Check the params in the route and set accordingly.  This enables the user to
    // go back/forward and not lose settings for filtering
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.workflowItems.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('requestId')) {
      this.requestId = this.route.snapshot.queryParamMap.get('requestId');
    }
    if (this.route.snapshot.queryParamMap.get('DBID')) {
      this.DBID = this.route.snapshot.queryParamMap.get('DBID');
    }
    if (this.route.snapshot.queryParamMap.get('selectedAPIAction')) {
      if (this.route.snapshot.queryParamMap.get('selectedAPIAction') === 'All') {
        this.selectedAPIAction = "All";
      }
      this.selectedAPIAction = APIAction[this.route.snapshot.queryParamMap.get('selectedAPIAction')];
    }

    this.loadData();

  }

}
