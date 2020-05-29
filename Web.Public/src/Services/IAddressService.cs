using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IAddressService
    {
        Address Get(long vendorId, long id);
        (long TotalRecords, List<Address> Addresses) GetAddressList(long vendorId, PagingOptions pagingOptions);
    }
}