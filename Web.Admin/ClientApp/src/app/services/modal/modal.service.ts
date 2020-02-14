import { Injectable } from '@angular/core';

// This is required in order to use JQuery funtionality
declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  constructor() { }

  showModal(modalId: string) {
    $("#" + modalId).modal('show');
  }

}
