using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IAddressProvider
    {
        Address Get(long vendorId, long id);
        (long TotalCount, List<Address> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
    }
}