using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IUserProvider _userProvider;

        public MessageService(IMessageProvider messageProvider, IUserProvider userProvider)
        {
            _messageProvider = messageProvider;
            _userProvider = userProvider;
        }
        
        public long InsertIntoQueue(APIAction apiAction, byte[] userCert, object content)
        {
            var requestID = _messageProvider.GetNextRequestID();
            var userID = _userProvider.GetUserIDFromCertificate(userCert);
            _messageProvider.InsertIntoQueue(apiAction, requestID, userID, content);

            return requestID;
        }
    }
}