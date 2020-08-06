import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AcvpUser } from '../../models/AcvpUser/AcvpUser';
import { AcvpUserDataProviderService } from '../../services/ajax/acvp-user/acvp-user-data-provider.service';

@Component({
  selector: 'app-acvp-user',
  templateUrl: './acvp-user.component.html',
  styleUrls: ['./acvp-user.component.scss']
})
export class AcvpUserComponent implements OnInit {

  selectedUser: AcvpUser;

  constructor(private AcvpUserDataService: AcvpUserDataProviderService, private route: ActivatedRoute) { }

  refreshPageData() {
    this.AcvpUserDataService.getAcvpUser(this.selectedUser.acvpUserID).subscribe(
      data => {
        this.selectedUser = data;

        // The ORM doesn't convert these, so we have to do it.  Not sure why.
        this.selectedUser.expiresOn = new Date(this.selectedUser.expiresOn);
      },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.AcvpUserDataService.getAcvpUser(parseInt(params.get("id"))).subscribe(
        data => {
          this.selectedUser = JSON.parse(JSON.stringify(data));

          // The ORM doesn't convert these, so we have to do it.  Not sure why.
          this.selectedUser.expiresOn = new Date(this.selectedUser.expiresOn);
        },
        err => { },
        () => { })
    });
  }

}
