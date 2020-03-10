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
//# sourceMappingURL=TestSessionStatus.js.map