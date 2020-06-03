using System;
using System.IO;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
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
        Task<T> GetObjectFromBodyJsonAsync<T>(Stream jsonBody) where T : IJsonObject;

        /// <summary>
        /// Unwrap a JSON object that has a corresponding workflow action.
        /// </summary>
        /// <param name="jsonBody">The JSON to parse.</param>
        /// <param name="apiAction">The type of workflow item to create.</param>
        /// <typeparam name="T">The <see cref="IMessagePayload"/> to parse and return.</typeparam>
        /// <returns>The parsed <see cref="T"/>.</returns>
        Task<T> GetMessagePayloadFromBodyJsonAsync<T>(Stream jsonBody, APIAction apiAction) where T : IMessagePayload;
    }
}