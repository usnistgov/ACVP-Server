using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Services
{
    public interface IMessageService
    {
        long InsertIntoQueue(APIAction apiAction, byte[] userCert, object content);
    }
}