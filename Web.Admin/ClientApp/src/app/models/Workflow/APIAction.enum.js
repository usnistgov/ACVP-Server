"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var APIAction;
(function (APIAction) {
    APIAction["Unknown"] = "Unknown";
    APIAction["CreateDependency"] = "CreateDependency";
    APIAction["UpdateDependency"] = "UpdateDependency";
    APIAction["DeleteDependency"] = "DeleteDependency";
    APIAction["CreateImplementation"] = "CreateImplementation";
    APIAction["UpdateImplementation"] = "UpdateImplementation";
    APIAction["DeleteImplementation"] = "DeleteImplementation";
    APIAction["CreateOE"] = "CreateOE";
    APIAction["UpdateOE"] = "UpdateOE";
    APIAction["DeleteOE"] = "DeleteOE";
    APIAction["CreatePerson"] = "CreatePerson";
    APIAction["UpdatePerson"] = "UpdatePerson";
    APIAction["DeletePerson"] = "DeletePerson";
    APIAction["CreateVendor"] = "CreateVendor";
    APIAction["UpdateVendor"] = "UpdateVendor";
    APIAction["DeleteVendor"] = "DeleteVendor";
    APIAction["RegisterTestSession"] = "RegisterTestSession";
    APIAction["CancelTestSession"] = "CancelTestSession";
    APIAction["CertifyTestSession"] = "CertifyTestSession";
    APIAction["SubmitVectorSetResults"] = "SubmitVectorSetResults";
    APIAction["CancelVectorSet"] = "CancelVectorSet";
})(APIAction = exports.APIAction || (exports.APIAction = {}));
// This portion is sued to enable us to iterate over the values in the HTML to create the workflow
// page's APIAction dropdown select box
(function (APIAction) {
    function values() {
        return Object.keys(APIAction).filter(function (type) { return isNaN(type) && type !== 'values'; });
    }
    APIAction.values = values;
})(APIAction = exports.APIAction || (exports.APIAction = {}));
//# sourceMappingURL=APIAction.enum.js.map