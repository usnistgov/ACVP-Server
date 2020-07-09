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

  // Used to store the interval object for eventual cleanup on component destruction
  //interval;

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
    // 06/04/2020 - Commented out because it was generating tons of logs when users left it open on their machines.
  //  this.interval = setInterval(() => {
  //    if (this.autoRefreshEnabled === true) {
  //      this.getPageData();
  //    }
  //  }, 1000);
  }

  //ngOnDestroy() { clearInterval(this.interval); }

}
