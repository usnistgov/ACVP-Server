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
var TestSessionListParameters = /** @class */ (function (_super) {
    __extends(TestSessionListParameters, _super);
    function TestSessionListParameters(TestSessionId, VectorSetId, TestSessionStatus) {
        var _this = _super.call(this) || this;
        _this.TestSessionId = TestSessionId;
        _this.VectorSetId = VectorSetId;
        _this.TestSessionStatus = TestSessionStatus;
        return _this;
    }
    return TestSessionListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.TestSessionListParameters = TestSessionListParameters;
//# sourceMappingURL=TestSessionListParameters.js.map