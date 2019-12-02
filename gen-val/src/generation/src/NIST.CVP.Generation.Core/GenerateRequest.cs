namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Object represents a request for vector generation.
    /// </summary>
    public class GenerateRequest
    {
        /// <summary>
        /// The JSON string representing an <see cref="IParameters"/>.
        /// </summary>
        public string RegistrationJson { get; }

        /// <summary>
        /// Construct the request.
        /// </summary>
        /// <param name="registrationJson">The json to package in the request.</param>
        public GenerateRequest(string registrationJson)
        {
            RegistrationJson = registrationJson;
        }
    }
}