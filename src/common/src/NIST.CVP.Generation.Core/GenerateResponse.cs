using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core
{
    public class GenerateResponse
    {
        public StatusCode StatusCode { get; }
        public string ErrorMessage { get; }

        public GenerateResponse()
        {
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
