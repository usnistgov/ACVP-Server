using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.Providers
{
    public interface IMessageProvider
    {
        void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content);
        long GetNextRequestID();
    }
}