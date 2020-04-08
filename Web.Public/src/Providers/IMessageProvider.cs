using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IMessageProvider
    {
        void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content);
        long GetNextRequestID();
    }
}