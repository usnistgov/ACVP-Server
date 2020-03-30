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
var PersonListParameters = /** @class */ (function (_super) {
    __extends(PersonListParameters, _super);
    function PersonListParameters(name, id, organizationName) {
        var _this = _super.call(this) || this;
        _this.name = "";
        _this.id = "";
        _this.organizationName = "";
        return _this;
    }
    return PersonListParameters;
}(PagedEnumerable_1.PagedEnumerable));
exports.PersonListParameters = PersonListParameters;
//# sourceMappingURL=PersonListParameters.js.map