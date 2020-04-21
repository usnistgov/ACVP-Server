using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.Services
{
    public interface IMessageService
    {
        long InsertIntoQueue(APIAction apiAction, byte[] userCert, object content);
    }
}