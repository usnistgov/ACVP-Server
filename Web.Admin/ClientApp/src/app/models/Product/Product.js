"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Product = /** @class */ (function () {
    function Product() {
    }
    Product.prototype.OperatingEnvironment = function (id, name, vendor, url, version, description, itar) {
        this.id = id;
        this.name = name;
        this.url = url;
        this.version = version;
        this.description = description;
        this.itar = itar;
    };
    ;
    return Product;
}());
exports.Product = Product;
//# sourceMappingURL=Product.js.map