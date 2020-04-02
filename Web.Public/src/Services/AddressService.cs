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

        public (long TotalRecords, List<Address> Addresses) GetAddressList(long vendorId, PagingOptions pagingOptions)
        {
            var addressList = _addressProvider.GetAddressList(vendorId);

            // If there isn't enough for a full page, return the list
            if (addressList.Count <= pagingOptions.Limit)
            {
                pagingOptions.Limit = addressList.Count;
                return (addressList.Count, addressList);
            }

            // Fix limit so it doesn't go past the end of the list
            if (pagingOptions.Offset + pagingOptions.Limit > addressList.Count)
            {
                pagingOptions.Limit = addressList.Count - pagingOptions.Offset;
            }
            
            return (addressList.Count, addressList.GetRange(pagingOptions.Offset, pagingOptions.Limit));
        }
    }
}