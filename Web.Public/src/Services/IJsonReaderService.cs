using System;
using System.IO;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.JsonObjects;

namespace Web.Public.Services
{
    /// <summary>
    /// Interface used for communication between the client and ACVP Web.Public
    /// </summary>
    public interface IJsonReaderService
    {
        /// <summary>
        /// Unwrap a JSON object that does not have a corresponding workflow.
        /// </summary>
        /// <param name="jsonBody">The JSON to parse.</param>
        /// <typeparam name="T">The <see cref="IJsonObject"/> to parse and return.</typeparam>
        /// <returns>The parsed <see cref="T"/>.</returns>
        T GetObjectFromBodyJson<T>(string jsonBody) where T : IJsonObject;

        /// <summary>
        /// Unwrap a JSON object that has a corresponding workflow action.
        /// </summary>
        /// <param name="jsonBody">The JSON to parse.</param>
        /// <param name="apiAction">The type of workflow item to create.</param>
        /// <param name="postDeserializationAction">The action to take on the deserialized object, like setting an ID.  Can be null.</param>
        /// <typeparam name="T">The <see cref="IWorkflowItemPayload"/> to parse and return.</typeparam>
        /// <returns>The parsed <see cref="T"/>.</returns>
        T GetWorkflowItemPayloadFromBodyJson<T>(string jsonBody, APIAction apiAction, Action<T> postDeserializationAction = null) where T : IWorkflowItemPayload;
        string GetJsonFromBody(Stream body);
    }
}