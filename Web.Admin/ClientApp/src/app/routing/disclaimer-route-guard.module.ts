import { Injectable, Component, NgModule } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@NgModule({
  declarations: [],
  imports: [],
  providers: [],
  bootstrap: []
})
export class DisclaimerRouteGuard implements CanActivate {

  constructor(private cookieService: CookieService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.cookieService.get('ACVPDisclaimerAccepted') !== 'accepted') {
      console.log('Setting routeParam to ' + state.url + ' and navigating away to dislcaimer');
      this.router.navigate(['disclaimer'], { queryParams: { returnUrl: state.url } });
      return false;
    } else {
      return true;
    }
  }
}
