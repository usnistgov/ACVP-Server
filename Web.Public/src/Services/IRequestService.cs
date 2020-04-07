using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IRequestService
    {
        Request GetRequest(long id);
    }
}