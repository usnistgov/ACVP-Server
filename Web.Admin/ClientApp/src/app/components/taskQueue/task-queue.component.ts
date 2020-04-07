import { Component, OnInit } from '@angular/core';
import { TaskQueue } from '../../models/taskQueue/TaskQueue';
import { AdministrativeAjaxProviderService } from '../../services/ajax/administrative/administrative-ajax-provider.service';

@Component({
  selector: 'app-task-queue',
  templateUrl: './task-queue.component.html',
  styleUrls: ['./task-queue.component.scss']
})
export class TaskQueueComponent implements OnInit {

  taskQueue: TaskQueue;

  constructor(private AdministrativeAjax: AdministrativeAjaxProviderService) { }

  deleteTask(id: number) {
    this.AdministrativeAjax.deleteTask(id).subscribe(
      data => {
        // Reload the page
        this.ngOnInit();
      });
  }

  ngOnInit() {
    this.AdministrativeAjax.getTaskQueue().subscribe(
      data => {
        this.taskQueue = data;
      });
  };

}
