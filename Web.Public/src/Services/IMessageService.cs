using ACVPWorkflow;

namespace Web.Public.Services
{
    public interface IMessageService
    {
        void InsertIntoQueue(APIAction apiAction, byte[] userCert, object content);
    }
}