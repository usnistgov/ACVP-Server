using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressProvider _addressProvider;

        public AddressService(IAddressProvider addressProvider)
        {
            _addressProvider = addressProvider;
        }

        public Address Get(long vendorId, long id) => _addressProvider.Get(vendorId, id);

        public (long TotalCount, List<Address> Addresses) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter) => _addressProvider.GetFilteredList(filter, pagingOptions.Offset, pagingOptions.Limit, orDelimiter, andDelimiter);
    }
}