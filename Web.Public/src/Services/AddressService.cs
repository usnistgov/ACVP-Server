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

        public Address Get(long id) => _addressProvider.Get(id);

    }
}