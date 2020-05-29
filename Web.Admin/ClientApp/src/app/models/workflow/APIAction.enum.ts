export enum APIAction {
  All = '',
  Unknown = 'Unknown',
  CreateDependency = 'CreateDependency',
  UpdateDependency = 'UpdateDependency',
  DeleteDependency = 'DeleteDependency',
  CreateImplementation = 'CreateImplementation',
  UpdateImplementation = 'UpdateImplementation',
  DeleteImplementation = 'DeleteImplementation',
  CreateOE = 'CreateOE',
  UpdateOE = 'UpdateOE',
  DeleteOE = 'DeleteOE',
  CreatePerson = 'CreatePerson',
  UpdatePerson = 'UpdatePerson',
  DeletePerson = 'DeletePerson',
  CreateVendor = 'CreateVendor',
  UpdateVendor = 'UpdateVendor',
  DeleteVendor = 'DeleteVendor',
  RegisterTestSession = 'RegisterTestSession',
  CancelTestSession = 'CancelTestSession',
  CertifyTestSession = 'CertifyTestSession',
  SubmitVectorSetResults = 'SubmitVectorSetResults',
  CancelVectorSet = 'CancelVectorSet'
}

// This portion is sued to enable us to iterate over the values in the HTML to create the workflow
// page's APIAction dropdown select box
export namespace APIAction {

  export function values() {
    return Object.keys(APIAction).filter(
      (type) => isNaN(<any>type) && type !== 'values'
    );
  }

}
