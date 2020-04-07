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
var OrganizationListParameters = /** @class */ (function (_super) {
    __extends(OrganizationListParameters, _super);
    function OrganizationListParameters(id, name) {
        var _this = _super.call(this) || this;
        _this.id = id;
        _this.name = name;
        return _this;
    }
    return OrganizationListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.OrganizationListParameters = OrganizationListParameters;
//# sourceMappingURL=OrganizationListParameters.js.map