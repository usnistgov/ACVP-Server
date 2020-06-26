import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowItemList } from '../../models/workflow/WorkflowItemList';
import { APIAction } from '../../models/workflow/APIAction.enum';
import { WorkflowProviderService } from '../../services/ajax/workflow/workflow-provider.service';
import { WorkflowStatus } from '../../models/workflow/WorkflowStatus.enum';
import { WorkflowListParameters } from '../../models/workflow/WorkflowListParameters';
import { WorkflowItemLite } from '../../models/workflow/WorkflowItemLite';
import { ModalService } from '../../services/modal/modal.service';

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {

  workflowItems: WorkflowItemList;
  listData: WorkflowListParameters;
  APIAction = APIAction;
  WorkflowStatus = WorkflowStatus;
  selectedAPIAction: string;
  selectedStatus: string;

  checkAllBoxesFlag = false;
  requestsToApprove: WorkflowItemLite[];

  constructor(private workflowService: WorkflowProviderService, private router: Router, private route: ActivatedRoute, private modalService: ModalService) { }

  /**
   * These two functions, routeToIndividualWorkflowPage and checkBoxClick, are required because we can't use
   * routerLinkn ont he HTML if we want to have the checkboxes AND the clickable rows.  This way, the checkBoxClick
   * can cancel the propagation of the routeToIndividualWorkflow event.  Keeps the routerLink from overriding the checkbox
   * https://stackblitz.com/edit/angular-pqna6e?file=app%2Fapp.component.html
   */
  routeToIndividualWorkflowPage(pageNum: number) {
    this.router.navigate(['workflow/', pageNum]);
  }

  checkBoxClick(event, num) {
    this.workflowItems.data[num].multiSelected = true;
    event.stopPropagation();
  }

  raiseMultiApprovalConfirmation() {
    this.requestsToApprove = this.workflowItems.data.filter(this.isMultiSelected);
    this.modalService.showModal('MultiApproveModal');
  }

  closeMultiApproveModal() {
    this.loadData();
    this.modalService.hideModal('MultiApproveModal');
  }

  // Small utility function used in a filter
  isMultiSelected(element: WorkflowItemLite, index, array) {
    if (element.multiSelected) { return true; }
    return false;
  }

  // The handler used to handle the "selectAll" checkbox
  checkAllBoxes() {
    if (this.checkAllBoxesFlag == false) {
      this.workflowItems.data.forEach(function (item, index, array) {
        if (item.status === "Pending") {
          item.multiSelected = true;
        }
      });
    }
    else {
      this.workflowItems.data.forEach(function (item, index, array) {
        if (item.status === "Pending") {
          item.multiSelected = false;
        }
      });
    }
    this.checkAllBoxesFlag = !this.checkAllBoxesFlag;
  }

  initializeMultiSelectFields() {
    this.workflowItems.data.forEach(function (item, index, array) { item.multiSelected = false; item.multiSelectSubmissionStatus = "unsubmitted"; });
  }

  loadData() {

    // Anytime the user's search changes, we default to page one
    this.listData.page = 1;

    // This sets the queryParams, but if they're empty, they end up having "&name=" by itself in the URL
    // So the following if statements check each of the available routeParams and clear them from the URL if they're set
    this.router.navigate([], {
      queryParams: this.listData
    });

    // Clear empty ones as necessary
    if (this.listData.RequestId === "") {
      this.router.navigate([], {
        queryParams: { name: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.APIActionID === "") {
      this.router.navigate([], {
        queryParams: { id: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.WorkflowItemId === "") {
      this.router.navigate([], {
        queryParams: { description: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.Status === "All") {
      this.router.navigate([], {
        queryParams: { description: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.workflowService.getWorkflows(this.listData).subscribe(
      data => {
        this.workflowItems = data;
        this.router.navigate([], {
          queryParams: { page: this.listData.page },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  ngOnInit() {
    this.listData = new WorkflowListParameters("", "", "", "");
    this.workflowItems = new WorkflowItemList();

    this.listData.pageSize = 10;
    this.listData.page = 1;
    this.listData.Status = "Pending";
    this.listData.APIActionID = "All";

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('Status')) {
      this.listData.Status = this.route.snapshot.queryParamMap.get('Status');
    }
    if (this.route.snapshot.queryParamMap.get('APIActionID')) {
      this.listData.APIActionID = this.route.snapshot.queryParamMap.get('APIActionID');
    }
    if (this.route.snapshot.queryParamMap.get('RequestId')) {
      this.listData.RequestId = this.route.snapshot.queryParamMap.get('RequestId');
    }

    this.workflowService.getWorkflows(this.listData).subscribe(
      data => { this.workflowItems = data; this.initializeMultiSelectFields(); },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.listData.page = 1;
    }
    else if (whichPage == "previous") {
      if (this.listData.page > 1) {
        this.listData.page = --this.listData.page;
      }
    }
    else if (whichPage == "next") {
      if (this.listData.page < this.workflowItems.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.workflowItems.totalPages;
    }

    this.workflowService.getWorkflows(this.listData).subscribe(
      data => {
        this.workflowItems = data;
        this.router.navigate([], {
          queryParams: { page: this.listData.page },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };


}
