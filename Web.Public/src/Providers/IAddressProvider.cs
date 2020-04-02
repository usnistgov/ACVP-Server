using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IAddressProvider
    {
        Address Get(long vendorId, long id);
        List<Address> GetAddressList(long vendorId);
    }
}