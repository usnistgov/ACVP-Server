using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Services
{
    /// <summary>
    /// Interface for inserting messages into the message queue.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Insert a message into the message queue.
        /// </summary>
        /// <param name="apiAction">The API Action performed.</param>
        /// <param name="userCert">The user's certificate.</param>
        /// <param name="content">The object to place into the message queue.</param>
        /// <returns>The request ID, if applicable.  Only applicable when message leads to a workflow item.</returns>
        long InsertIntoQueue(APIAction apiAction, string userCertSubject, object content);
    }
}