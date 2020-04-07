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
var ProductListParameters = /** @class */ (function (_super) {
    __extends(ProductListParameters, _super);
    function ProductListParameters(name, id, description) {
        var _this = _super.call(this) || this;
        _this.name = name;
        _this.id = id;
        _this.description = description;
        return _this;
    }
    return ProductListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.ProductListParameters = ProductListParameters;
//# sourceMappingURL=ProductListParameters.js.map