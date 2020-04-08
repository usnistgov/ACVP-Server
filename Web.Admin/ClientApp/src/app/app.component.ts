import { Component } from '@angular/core';
import { CurrentUserDataProviderService } from './services/ajax/currentUser/currentUser-data-provider.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ng5';

  email: string

  constructor(private CurrentUserProviderService: CurrentUserDataProviderService) {
    this.CurrentUserProviderService.getCurrentUserEmail().subscribe(
      data => {
        console.log('data: ' + data);
        this.email = data;
      },
      err => { 
        console.error('Error: ' + err);
        this.email = 'Unknown User';
      });
  }
}
