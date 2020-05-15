import { Component, OnInit, Input } from '@angular/core';
import { TestSession } from '../../../models/testSession/TestSession';

@Component({
  selector: 'app-testsession',
  templateUrl: './testsession.component.html',
  styleUrls: ['./testsession.component.scss']
})
export class TestsessionComponent implements OnInit {

  selectedSession: TestSession;

  constructor() { }

  @Input()
  set session(selectedSession: TestSession) {
    this.selectedSession = selectedSession;
  }

  ngOnInit() {
  }

}
