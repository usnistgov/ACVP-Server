import { Component, OnInit } from '@angular/core';
import { Organization } from '../../models/organization/Organization';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { AddressCreateParameters } from '../../models/address/AddressCreateParameters';
import { OrganizationProviderService } from '../../services/ajax/organization/organization-provider.service';
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

  onSeedUpdated() {
    this.AcvpUserDataService.getAcvpUser(this.selectedUser.acvpUserId).subscribe(
      data => { this.selectedUser = data; },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.AcvpUserDataService.getAcvpUser(parseInt(params.get("id"))).subscribe(
        data => { this.selectedUser = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
