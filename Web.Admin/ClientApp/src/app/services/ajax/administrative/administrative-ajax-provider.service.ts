import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageQueue } from '../../../models/messageQueue/MessageQueue';
import { TaskQueue } from '../../../models/taskQueue/TaskQueue';

@Injectable({
  providedIn: 'root'
})
export class AdministrativeAjaxProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  getMessageQueue() {
    return this.http.get<MessageQueue>(this.apiRoot + '/MessageQueue');
  }

  getTaskQueue() {
    return this.http.get<TaskQueue>(this.apiRoot + '/TaskQueue');
  }

  deleteMessage(id: string) {
    return this.http.delete(this.apiRoot + '/MessageQueue/' + id);
  }

  deleteTask(id: number) {
    return this.http.delete(this.apiRoot + '/TaskQueue/' + id);
  }

}
