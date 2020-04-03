import { Component, OnInit } from '@angular/core';
import { CurrentUserDataProviderService } from 'src/app/services/ajax/currentUser/currentUser-data-provider.service';

@Component({
  selector: 'app-currentUser',
  templateUrl: './currentUser.component.html'
})
export class CurrentUserComponent implements OnInit {

    claims: Map<string, string>;

    constructor(private currentUserService: CurrentUserDataProviderService) { }

    ngOnInit() {
        this.currentUserService.getCurrentUserClaims().subscribe(
            data => {
                this.claims = data;
            },
            err => {
                console.log(err);
            }
        );
    }

}
