import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {

  testString:string = 'This is a failure';
  selectedSession = { };
  selectedSessionVectorSets = {};
  session556 = {};

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    // Nothing yet
  }

  setSelectedSession(sessionId:number) {

    // Get the main session data
    this.ajs.getSession(556).subscribe(
      data => { this.selectedSession = data; },
      err => { throw new Error('Test Session 556 not available') },
      () => {}
    );

    // Then get the vector set data associated with it
    this.ajs.getVectorSets(556).subscribe(
      data => { this.selectedSessionVectorSets = data; },
      err => { throw new Error('VectorSets not available') },
      () => {}
    );
  }

}
