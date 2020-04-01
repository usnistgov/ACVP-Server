using ACVPWorkflow;

namespace Web.Public.Services
{
    public interface IMessageService
    {
        long InsertIntoQueue(APIAction apiAction, byte[] userCert, object content);
    }
}