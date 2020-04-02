import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageQueue } from '../../../models/MessageQueue/MessageQueue';
import { TaskQueue } from '../../../models/TaskQueue/TaskQueue';

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

}
