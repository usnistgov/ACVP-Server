"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Product = /** @class */ (function () {
    function Product() {
    }
    Product.prototype.Product = function (id, name, vendor, url, version, description, itar, address) {
        this.id = id;
        this.name = name;
        this.url = url;
        this.version = version;
        this.description = description;
        this.itar = itar;
        this.address = address;
    };
    ;
    return Product;
}());
exports.Product = Product;
//# sourceMappingURL=Product.js.map