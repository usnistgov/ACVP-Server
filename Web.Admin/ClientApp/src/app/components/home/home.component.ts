import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'
import { TestSessionList } from '../../models/TestSession/TestSessionList';
import { TestSession } from '../../models/TestSession/TestSession';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {

  selectedSession: TestSession;
  sessions: TestSessionList;

  // Necessary alias to make Math available to the front-end for paging calculations
  math = Math;

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  getPage(whichPage: string) {

    console.log(whichPage);

    if (whichPage == "first") {
      this.sessions.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.sessions.currentPage > 1) {
        this.sessions.currentPage = --this.sessions.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.sessions.currentPage < (Math.floor(this.sessions.totalRecords / this.sessions.pageSize))) {
        this.sessions.currentPage = ++this.sessions.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.sessions.currentPage = (Math.floor(this.sessions.totalRecords / this.sessions.pageSize));
    }

    this.ajs.getTestSessions(this.sessions.pageSize, this.sessions.currentPage).subscribe(
      data => {
        this.sessions = data;
        this.router.navigate([], {
          queryParams: { page: this.sessions.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

  ngOnInit() {

    this.sessions = new TestSessionList();
    this.sessions.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.sessions.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    // ... Then make the initial data request accordingly
    this.ajs.getTestSessions(10, this.sessions.currentPage).subscribe(
      data => { this.sessions = data; },
      err => { throw new Error('TestSession list not available') },
      () => { }
    );

  }

  setSelectedSession(sessionId:number) {
    this.ajs.getTestSession(sessionId).subscribe(
      data => {
        this.selectedSession = data;
      },
      err => { throw new Error('Test Session ' + sessionId + ' not available') },
      () => { }
    );
  }

}
