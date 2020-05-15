import { Component, OnInit } from '@angular/core';
import { TestSessionProviderService } from '../../services/ajax/testSession/test-session-provider.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { TestSession } from '../../models/testSession/TestSession';
import { TestSessionListParameters } from '../../models/testSession/TestSessionListParameters';
import { TestSessionList } from '../../models/testSession/TestSessionList';
import { TestSessionStatus } from '../../models/testSession/TestSessionStatus';

@Component({
  selector: 'app-testsessions',
  templateUrl: './testsessions.component.html',
  styleUrls: ['./testsessions.component.scss']
})
export class TestsessionsComponent implements OnInit {

  sessions: TestSessionList;
  listData: TestSessionListParameters;
  selectedSession: TestSession;

  showFilters = false;

  // Data-binding and filter-related values
  testSessionId: string;
  vectorSetId: string;
  selectedTestSessionStatus: string;
  TestSessionStatus = TestSessionStatus;

  constructor(private TestSessionService: TestSessionProviderService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location) { }

  ngOnInit() {

    this.listData = new TestSessionListParameters("", "", null);
    this.sessions = new TestSessionList();

    this.listData.page = 1;
    this.listData.pageSize = 10;
    this.listData.TestSessionStatus = "All";

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.sessions.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    if (this.route.snapshot.queryParamMap.get('TestSessionId')) {
      // To make sure the filters panel is open on loading.  Particularly important when navigating back and forth
      this.showFilters = true;
      this.listData.TestSessionId = this.route.snapshot.queryParamMap.get('TestSessionId');
    }

    if (this.route.snapshot.queryParamMap.get('VectorSetId')) {
      // To make sure the filters panel is open on loading.  Particularly mportant when navigating back and forth
      this.showFilters = true;
      this.listData.VectorSetId = this.route.snapshot.queryParamMap.get('VectorSetId');
    }

    if (this.route.snapshot.queryParamMap.get('TestSessionStatus')) {
      // To make sure the filters panel is open on loading.  Particularly mportant when navigating back and forth
      this.showFilters = true;
      this.listData.TestSessionStatus = this.route.snapshot.queryParamMap.get('TestSessionStatus');
    }

    this.route.paramMap.subscribe(params => {
      if (params.get("id") !== null) {
        this.setSelectedSession(parseInt(params.get("id")));
      }
    });

    // ... Then make the initial data request accordingly
    this.TestSessionService.getTestSessions(this.listData).subscribe(
      data => { this.sessions = data; },
      err => { throw new Error('TestSession list not available') },
      () => { }
    );
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
    if (this.listData.TestSessionId === "") {
      this.router.navigate([], {
        queryParams: { name: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.VectorSetId === "") {
      this.router.navigate([], {
        queryParams: { id: null },
        queryParamsHandling: 'merge'
      });
    }

    //if (this.listData.TestSessionStatus === "All") {
    //  this.listData.TestSessionStatus = null;
      this.TestSessionService.getTestSessions(this.listData).subscribe(
        data => {
          this.sessions = data;
          this.router.navigate([], {
            queryParams: { page: this.listData.page },
            queryParamsHandling: 'merge'
          });
        },
        err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
        () => { }
      );
    //}
    //else {
    //  this.TestSessionService.getTestSessions(this.listData).subscribe(
    //    data => {
    //      this.sessions = data;
    //      this.router.navigate([], {
    //        queryParams: { page: this.listData.page },
    //        queryParamsHandling: 'merge'
    //      });
    //    },
    //    err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
    //    () => { }
    //  );
    //}
  }

  setSelectedSession(sessionId: number) {
    this.TestSessionService.getTestSession(sessionId).subscribe(
      data => {
        this.selectedSession = data;

        // The reason for using this is that it allows changing the url without reloading the route
        this.location.go('/testSessions/' + this.selectedSession.testSessionId);
      },
      err => { throw new Error('Test Session ' + sessionId + ' not available') },
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
      if (this.listData.page < this.sessions.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.sessions.totalPages;
    }

    this.TestSessionService.getTestSessions(this.listData).subscribe(
      data => {
        this.sessions = data;
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
