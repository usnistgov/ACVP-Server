using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Services
{
    public interface IJsonWriterService
    {
        object BuildVersionedObject(object content);
        string BuildRequestObject(long requestId, APIAction apiActionId, long userId, object content);
    }
}