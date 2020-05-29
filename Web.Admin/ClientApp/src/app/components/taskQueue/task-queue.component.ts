import { Component, OnInit } from '@angular/core';
import { TaskQueue } from '../../models/taskQueue/TaskQueue';
import { AdministrativeAjaxProviderService } from '../../services/ajax/administrative/administrative-ajax-provider.service';
import { TestSessionProviderService } from '../../services/ajax/testSession/test-session-provider.service';
import { TestSessionListParameters } from '../../models/testSession/TestSessionListParameters';

@Component({
  selector: 'app-task-queue',
  templateUrl: './task-queue.component.html',
  styleUrls: ['./task-queue.component.scss']
})
export class TaskQueueComponent implements OnInit {

  taskQueue: TaskQueue;

  constructor(private AdministrativeAjax: AdministrativeAjaxProviderService,
    private TestSessionProvider: TestSessionProviderService) { }

  deleteTask(id: number) {
    this.AdministrativeAjax.deleteTask(id).subscribe(
      data => {
        // Reload the page
        this.ngOnInit();
      });
  }

  ngOnInit() {

    let self = this;

    this.AdministrativeAjax.getTaskQueue().subscribe(
      data => {
        this.taskQueue = data;

        this.taskQueue.data.forEach(function (task, index, array) {
          let sessionSearchParams = new TestSessionListParameters("", "", "");
          sessionSearchParams.VectorSetId = task.vectorSetID.toString();

          self.TestSessionProvider.getTestSessions(sessionSearchParams).subscribe(
            data => {
              if (data.data.length > 0) {
                // This works because if a TS exists, there should only ever be one
                task.testSessionId = data.data[0].testSessionId;
              }
            }
          );

        });

      });
  }

}
