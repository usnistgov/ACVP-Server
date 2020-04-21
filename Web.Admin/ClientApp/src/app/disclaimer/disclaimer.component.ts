import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-disclaimer',
  templateUrl: './disclaimer.component.html',
  styleUrls: ['./disclaimer.component.scss']
})
export class DisclaimerComponent implements OnInit {

  constructor(private cookieService: CookieService, private route: ActivatedRoute, private router: Router) { }

  acceptDisclaimer() {
    this.cookieService.set('ACVPDisclaimerAccepted', 'accepted', 1);
    this.route.queryParams.subscribe(params => {
      console.log('Returning from: ' + params['returnUrl']);
      this.router.navigate([params['returnUrl']], {});
    });
    
  }

  ngOnInit() {
  }

}
