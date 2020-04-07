"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WorkflowStatus;
(function (WorkflowStatus) {
    WorkflowStatus["Pending"] = "Pending";
    WorkflowStatus["Incomplete"] = "Incomplete";
    WorkflowStatus["Approved"] = "Approved";
    WorkflowStatus["Rejected"] = "Rejected";
})(WorkflowStatus = exports.WorkflowStatus || (exports.WorkflowStatus = {}));
// This portion is sued to enable us to iterate over the values in the HTML to create the workflow
// page's APIAction dropdown select box
(function (WorkflowStatus) {
    function values() {
        return Object.keys(WorkflowStatus).filter(function (type) { return isNaN(type) && type !== 'values'; });
    }
    WorkflowStatus.values = values;
})(WorkflowStatus = exports.WorkflowStatus || (exports.WorkflowStatus = {}));
//# sourceMappingURL=WorkflowStatus.enum.js.map