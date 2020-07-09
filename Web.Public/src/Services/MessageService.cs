using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IUserProvider _userProvider;
        private readonly IRequestProvider _requestProvider;

        public MessageService(IMessageProvider messageProvider, IUserProvider userProvider, IRequestProvider requestProvider)
        {
            _messageProvider = messageProvider;
            _userProvider = userProvider;
            _requestProvider = requestProvider;
        }
        
        public long InsertIntoQueue(APIAction apiAction, string userCertSubject, object content)
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(userCertSubject);

            // We only need to wrap a with a request id wrapper in cases where the action being taken leads to a workflow.
            if (content is IWorkflowItemPayload)
            {
                var requestID = _requestProvider.GetNextRequestID();
                _messageProvider.InsertIntoQueue(apiAction, requestID, userID, content);

                return requestID;                
            }
            
            // Messages that don't lead to workflow items don't get a request id.
            _messageProvider.InsertIntoQueue(apiAction, userID, content);
            return 0;
        }
    }
}