using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;
using Web.Public.Results;

namespace Web.Public.Providers
{
    public interface IVendorProvider
    {
        VendorResult CreateVendor(Organization vendor);
        List<Organization> GetVendorList();
        Organization GetVendor(int id);
        VendorResult UpdateVendor(int id, Organization vendor);
        Result DeleteVendor(int id);
    }
}