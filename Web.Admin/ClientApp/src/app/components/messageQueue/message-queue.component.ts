import { Component, OnInit } from '@angular/core';
import { AdministrativeAjaxProviderService } from '../../services/ajax/administrative/administrative-ajax-provider.service';
import { MessageQueue } from '../../models/MessageQueue/MessageQueue';

@Component({
  selector: 'app-message-queue',
  templateUrl: './message-queue.component.html',
  styleUrls: ['./message-queue.component.scss']
})
export class MessageQueueComponent implements OnInit {

  messageQueue: MessageQueue;

  constructor(private AdministrativeAjax: AdministrativeAjaxProviderService) { }

  ngOnInit() {
    this.AdministrativeAjax.getMessageQueue().subscribe(
      data => {
        this.messageQueue = data;
      });
  };

}
