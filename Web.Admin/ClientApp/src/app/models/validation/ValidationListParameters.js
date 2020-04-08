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
var ValidationListParameters = /** @class */ (function (_super) {
    __extends(ValidationListParameters, _super);
    function ValidationListParameters(validationId, validationLabel, productName) {
        var _this = _super.call(this) || this;
        _this.validationId = validationId;
        _this.validationLabel = validationLabel;
        _this.productName = productName;
        return _this;
    }
    return ValidationListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.ValidationListParameters = ValidationListParameters;
//# sourceMappingURL=ValidationListParameters.js.map