using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core
{
    public class GenerateResponse
    {
        /// <summary>
        /// Status code representing the outcome of the generation
        /// </summary>
        public StatusCode StatusCode { get; }
        /// <summary>
        /// A delimited Error response due to an error in generation.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// The json representing the internal projection of the generation.
        /// </summary>
        public string InternalProjection { get; }
        /// <summary>
        /// The json representing the prompt projection of the generation.
        /// </summary>
        public string PromptProjection { get; }
        /// <summary>
        /// The expected results from the IUT in validation (not always applicable).
        /// </summary>
        public string ResultProjection { get; }
        
        public GenerateResponse()
        {
            StatusCode = StatusCode.Success;
        }

        public GenerateResponse(string internalProjection, string promptProjection, string resultProjection)
        {
            InternalProjection = internalProjection;
            PromptProjection = promptProjection;
            ResultProjection = resultProjection;
            StatusCode = StatusCode.Success;
        }

        public GenerateResponse(string errorMessage, StatusCode statusCode = StatusCode.GeneratorError)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
