using System.Collections.Generic;
using ACVPCore.Results;
using Web.Public.Models;
using Web.Public.Results;

namespace Web.Public.Providers
{
    public interface IVendorProvider
    {
        VendorResult CreateVendor(Vendor vendor);
        List<VendorResult> GetVendorList();
        VendorResult GetVendor(int id);
        VendorResult UpdateVendor(int id, Vendor vendor);
        Result DeleteVendor(int id);
    }
}