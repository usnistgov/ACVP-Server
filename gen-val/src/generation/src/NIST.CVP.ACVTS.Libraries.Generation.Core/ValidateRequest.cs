namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    /// <summary>
    /// Object represents a request for vector validation.
    /// </summary>
    public class ValidateRequest
    {
        /// <summary>
        /// The JSON string representing the ACVP server internal projection.
        /// </summary>
        public string InternalJson { get; }

        /// <summary>
        /// The JSON string representing the IUT response. 
        /// </summary>
        public string ResultJson { get; }

        /// <summary>
        /// Should the validation process print out the expected results?
        /// </summary>
        /// <remarks>Not applicable for all algorithms.</remarks>
        public bool ShowExpected { get; }

        /// <summary>
        /// Construct the request.
        /// </summary>
        /// <param name="internalJson">The ACVP server internal projection json.</param>
        /// <param name="resultJson">The IUT response json.</param>
        /// <param name="showExpected">Should the expected answers be shown?</param>
        public ValidateRequest(string internalJson, string resultJson, bool showExpected)
        {
            InternalJson = internalJson;
            ResultJson = resultJson;
            ShowExpected = showExpected;
        }
    }
}
