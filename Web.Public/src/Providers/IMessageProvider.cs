using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Providers
{
    /// <summary>
    /// Provides means of getting requestIds and writing to the message queue.
    /// </summary>
    public interface IMessageProvider
    {
        /// <summary>
        /// Create a message for the message queue where the resulting action does not lead to a workflow item.
        /// </summary>
        /// <param name="apiAction">The api action being performed.</param>
        /// <param name="userID">The user id performing the action.</param>
        /// <param name="content">The object to be placed into the message queue.</param>
        void InsertIntoQueue(APIAction apiAction, long userID, object content);
        /// <summary>
        /// Create a message for the message queue where the resulting action leads to a workflow item.
        /// </summary>
        /// <param name="apiAction">The api action being performed.</param>
        /// <param name="requestID">The request ID of the action - returned to the user.</param>
        /// <param name="userID">The user id performing the action.</param>
        /// <param name="content">The object to be placed into the message queue.</param>
        void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content);
        long GetNextRequestID();
    }
}