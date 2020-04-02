using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IAddressService
    {
        Address Get(long vendorId, long id);
        (long TotalCount, List<Address> Addresses) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
    }
}