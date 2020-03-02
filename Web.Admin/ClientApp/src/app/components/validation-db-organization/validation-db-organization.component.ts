import { Component, OnInit } from '@angular/core';
import { Organization } from '../../models/organization/organization';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';
import { Address } from '../../models/Address/Address';
import { AddressCreateParameters } from '../../models/Address/AddressCreateParameters';

@Component({
  selector: 'app-validation-db-organization',
  templateUrl: './validation-db-organization.component.html',
  styleUrls: ['./validation-db-organization.component.scss']
})
export class ValidationDbOrganizationComponent implements OnInit {

  selectedOrganization: Organization;
  referenceCopyOrganization: Organization;
  updateStatusFlag = "none";

  newAddressData = new AddressCreateParameters(-1, -1, "", "", "", "", "", "", "");

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  raiseNewAddressModal() {
    this.newAddressData = new AddressCreateParameters(-1, -1, "", "", "", "", "", "", "");
    this.modalService.showModal("newAddressModal");
  }

  updateOrganization() {
    this.ajs.updateOrganization(this.referenceCopyOrganization).subscribe(
      data => { this.updateStatusFlag = "successful"; this.refreshPageData(); },
      err => { console.log("Update failed"); },
      () => { });
  }

  submitNewAddress() {
    // Always insert at the end for now
    this.newAddressData.orderIndex = this.selectedOrganization.addresses.length+1;
    this.newAddressData.organizationID = this.selectedOrganization.id;
    this.ajs.addNewAddress(this.newAddressData).subscribe(
      data => { this.refreshPageData(); this.modalService.hideModal("newAddressModal"); },
      err => { },
      () => { });
  }

  deleteAddressAtIndex(index: number) {
    this.ajs.deleteAddressFromOrganization(index, this.selectedOrganization.id).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { });
  }

  refreshPageData() {
    this.ajs.getOrganization(this.selectedOrganization.id).subscribe(
      data => { this.selectedOrganization = JSON.parse(JSON.stringify(data)); this.referenceCopyOrganization = JSON.parse(JSON.stringify(data)); },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getOrganization(parseInt(params.get("id"))).subscribe(
        data => { this.selectedOrganization = JSON.parse(JSON.stringify(data)); this.referenceCopyOrganization = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
