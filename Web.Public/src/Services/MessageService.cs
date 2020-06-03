using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
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
        
        public async Task<long> InsertIntoQueueAsync(APIAction apiAction, string userCertSubject, object content)
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(userCertSubject);

            // We only need to wrap a with a request id wrapper in cases where the action being taken leads to a workflow.
            if (content is IWorkflowItemPayload)
            {
                var requestID = _messageProvider.GetNextRequestID();
                await _messageProvider.InsertIntoQueueAsync(apiAction, requestID, userID, content);

                return requestID;                
            }
            
            // Messages that don't lead to workflow items don't get a request id.
            await _messageProvider.InsertIntoQueueAsync(apiAction, userID, content);
            return 0;
        }
    }
}