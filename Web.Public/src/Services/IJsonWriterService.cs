using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Services
{
    /// <summary>
    /// Provides an interface for writing json.
    /// </summary>
    public interface IJsonWriterService
    {
        /// <summary>
        /// Build the client expected versioned json object
        /// (array with 2 objects contained, the first of which being the "acv_version").
        /// </summary>
        /// <param name="content">The object to build a versioned object out of.</param>
        /// <returns></returns>
        object BuildVersionedObject(object content);
        /// <summary>
        /// Build the json that represents a request object - an object that leads to a workflow item.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="content">The object to jsonify.</param>
        /// <returns></returns>
        string BuildRequestWorkflowObject(long requestId, object content);
        /// <summary>
        /// Build the json that represents an object that does not lead to a workflow item.
        /// </summary>
        /// <param name="content">The object to jsonify.</param>
        /// <returns></returns>
        string BuildMessageObject(object content);
    }
}