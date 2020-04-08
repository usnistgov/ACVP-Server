using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IMessageService
    {
        long InsertIntoQueue(APIAction apiAction, byte[] userCert, object content);
    }
}