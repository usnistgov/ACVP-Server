import { Component, OnInit } from '@angular/core';
import { TestSessionList } from '../../models/TestSession/TestSessionList';
import { TestSession } from '../../models/TestSession/TestSession';
import { Router, ActivatedRoute } from '@angular/router';
import { TestSessionProviderService } from '../../services/ajax/testSession/test-session-provider.service';
import { TestSessionStatus } from '../../models/TestSession/TestSessionStatus';
import { TestSessionListParameters } from '../../models/TestSession/TestSessionListParameters';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {

  selectedSession: TestSession;
  sessions: TestSessionList;
  listData: TestSessionListParameters;
  testSessionId: string;
  vectorSetId: string;
  selectedTestSessionStatus: string;
  TestSessionStatus = TestSessionStatus;

  constructor(private TestSessionService: TestSessionProviderService, private router: Router, private route: ActivatedRoute) { }

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

  setTestSessionStatus(input: string) {
    this.listData.TestSessionStatus = input;
    this.loadData();
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

    if (this.selectedTestSessionStatus === "All" || this.selectedTestSessionStatus === null) {
      this.listData.TestSessionStatus = null;
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
    }
    else {
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
    }
  }

  ngOnInit() {

    this.listData = new TestSessionListParameters("", "", null);
    this.sessions = new TestSessionList();

    this.listData.page = 1;
    this.listData.pageSize = 10;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.sessions.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    // ... Then make the initial data request accordingly
    this.TestSessionService.getTestSessions(this.listData).subscribe(
      data => { this.sessions = data; },
      err => { throw new Error('TestSession list not available') },
      () => { }
    );

  }

  setSelectedSession(sessionId:number) {
    this.TestSessionService.getTestSession(sessionId).subscribe(
      data => {
        this.selectedSession = data;
      },
      err => { throw new Error('Test Session ' + sessionId + ' not available') },
      () => { }
    );
  }

}
