"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TestSessionStatus;
(function (TestSessionStatus) {
    TestSessionStatus[TestSessionStatus["Unknown"] = 0] = "Unknown";
    TestSessionStatus[TestSessionStatus["Cancelled"] = 1] = "Cancelled";
    TestSessionStatus[TestSessionStatus["PendingEvaluation"] = 2] = "PendingEvaluation";
    TestSessionStatus[TestSessionStatus["Failed"] = 3] = "Failed";
    TestSessionStatus[TestSessionStatus["Passed"] = 4] = "Passed";
    TestSessionStatus[TestSessionStatus["SubmittedForApproval"] = 5] = "SubmittedForApproval";
    TestSessionStatus[TestSessionStatus["Published"] = 6] = "Published";
})(TestSessionStatus = exports.TestSessionStatus || (exports.TestSessionStatus = {}));
// This portion is sued to enable us to iterate over the values in the HTML to create the TestSession
// page's status dropdown select box
(function (TestSessionStatus) {
    function values() {
        return Object.keys(TestSessionStatus).filter(function (type) { return isNaN(type) && type !== 'values'; });
    }
    TestSessionStatus.values = values;
})(TestSessionStatus = exports.TestSessionStatus || (exports.TestSessionStatus = {}));
//# sourceMappingURL=TestSessionStatus.js.map