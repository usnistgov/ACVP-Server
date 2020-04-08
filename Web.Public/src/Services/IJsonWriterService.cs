using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IJsonWriterService
    {
        object BuildVersionedObject(object content);
        string BuildRequestObject(long requestId, APIAction apiActionId, long userId, object content);
    }
}