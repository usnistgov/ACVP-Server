import { Component, OnInit } from '@angular/core';
import { AdministrativeAjaxProviderService } from '../../services/ajax/administrative/administrative-ajax-provider.service';
import { MessageQueue } from '../../models/messageQueue/MessageQueue';

@Component({
  selector: 'app-message-queue',
  templateUrl: './message-queue.component.html',
  styleUrls: ['./message-queue.component.scss']
})
export class MessageQueueComponent implements OnInit {

  messageQueue: MessageQueue;
  autoRefreshEnabled = true;

  constructor(private AdministrativeAjax: AdministrativeAjaxProviderService) { }

  deleteMessage(id: string) {
    this.AdministrativeAjax.deleteMessage(id).subscribe(
      data => {
        // Reload the page
        this.ngOnInit();
      });
  }

  getPageData() {
    this.AdministrativeAjax.getMessageQueue().subscribe(
      data => {
        this.messageQueue = data;
      });
  }

  ngOnInit() {
    // Get the initial data
    this.getPageData();

    // This sets the callback function to be run every second.
    // The callback contains a boolean check
    setInterval(() => {
      if (this.autoRefreshEnabled === true) {
        this.getPageData();
      }
    }, 1000);
  }

}
