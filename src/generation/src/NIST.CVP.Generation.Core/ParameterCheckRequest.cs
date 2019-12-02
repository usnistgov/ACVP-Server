namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Object represents a request for checking parameters for vector generation.
    /// </summary>
    public class ParameterCheckRequest
    {
        /// <summary>
        /// The JSON string representing an <see cref="IParameters"/>.
        /// </summary>
        public string RegistrationJson { get; }

        /// <summary>
        /// Construct the request.
        /// </summary>
        /// <param name="registrationJson">The json to package in the request.</param>
        public ParameterCheckRequest(string registrationJson)
        {
            RegistrationJson = registrationJson;
        }
    }
}