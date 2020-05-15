import { Component, OnInit, Input } from '@angular/core';
import { TestSession } from '../../../models/testSession/TestSession';
import { TestSessionProviderService } from '../../../services/ajax/testSession/test-session-provider.service';

@Component({
  selector: 'app-testsession',
  templateUrl: './testsession.component.html',
  styleUrls: ['./testsession.component.scss']
})
export class TestsessionComponent implements OnInit {

  selectedSession: TestSession;

  constructor(private TestSessionService: TestSessionProviderService) { }

  @Input()
  set session(selectedSession: TestSession) {
    this.selectedSession = selectedSession;
  }

  refreshPageData() {
    this.TestSessionService.getTestSession(this.selectedSession.testSessionId).subscribe(
      data => {
        this.selectedSession = data;
      }
    );
  };

  queueGeneration(vectorSetId: number, testSessionId: number) {
    console.log(`Resubmitting vector set ${vectorSetId} for generation.`);
    this.TestSessionService.queueGeneration(vectorSetId).subscribe(
      null,
      err => {
        console.log(`Failed queueing generation task: ${JSON.stringify(err)}`)
      },
      () => {
        this.refreshPageData();
      });
  }

  queueValidation(vectorSetId: number, testSessionId: number) {
    console.log(`Resubmitting vector set ${vectorSetId} for validation.`);
    this.TestSessionService.queueValidation(vectorSetId).subscribe(
      null,
      err => {
        console.log(`Failed queueing validation task: ${JSON.stringify(err)}`)
      },
      () => {
        this.refreshPageData();
      });
  }


  ngOnInit() {
  }

}
