using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IAddressService
    {
        Address Get(long id);
    }
}