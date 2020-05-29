import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowValidationCreatePayload } from '../../../../models/workflow/validation/WorkflowValidationCreatePayload';
import { ProductProviderService } from '../../../../services/ajax/product/product-provider.service';
import { OperatingEnvironmentProviderService } from '../../../../services/ajax/operatingEnvironment/operating-environment-provider.service';
import { TestSessionProviderService } from '../../../../services/ajax/testSession/test-session-provider.service';

@Component({
  selector: 'app-workflow-validation-create',
  templateUrl: './workflow-validation-create.component.html',
  styleUrls: ['./workflow-validation-create.component.scss']
})
export class WorkflowValidationCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowValidationCreatePayload>;
  objectKeys = Object.keys;
  constructor(private ProductService: ProductProviderService, private OEService: OperatingEnvironmentProviderService, private testSessionService: TestSessionProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowValidationCreatePayload>) {
    this.workflowItem = item;

    if (this.workflowItem.payload.moduleUrl !== null) {
      let productId: number = parseInt(this.workflowItem.payload.moduleUrl.split('/')[this.workflowItem.payload.moduleUrl.split('/').length - 1]);

      this.ProductService.getProduct(productId).subscribe(
        data => {
          this.workflowItem.payload.module = data;
          console.log(this.workflowItem);
        }
      );
    }

    if (this.workflowItem.payload.oeUrl !== null) {
      let OEId: number = parseInt(this.workflowItem.payload.oeUrl.split('/')[this.workflowItem.payload.oeUrl.split('/').length - 1]);

      this.OEService.getOE(OEId).subscribe(
        data => {
          this.workflowItem.payload.oe = data;
        }
      );
    }

    this.testSessionService.getTestSession(this.workflowItem.payload.testSessionId)
      .subscribe(
        data => {
          this.workflowItem.payload.testSession = data;
        }
      );
  }

  ngOnInit() {
  }

}
