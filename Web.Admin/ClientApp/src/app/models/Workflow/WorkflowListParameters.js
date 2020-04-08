"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var PagedEnumerable_1 = require("../responses/PagedEnumerable");
var WorkflowListParameters = /** @class */ (function (_super) {
    __extends(WorkflowListParameters, _super);
    function WorkflowListParameters(WorkflowItemId, APIActionID, RequestId, Status) {
        var _this = _super.call(this) || this;
        _this.WorkflowItemId = WorkflowItemId;
        _this.APIActionID = APIActionID;
        _this.RequestId = RequestId;
        _this.Status = Status;
        return _this;
    }
    return WorkflowListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.WorkflowListParameters = WorkflowListParameters;
//# sourceMappingURL=WorkflowListParameters.js.map